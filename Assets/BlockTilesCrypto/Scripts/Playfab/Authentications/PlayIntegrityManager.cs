using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using PlayFab;
using PlayFab.ClientModels;
using Google.Play.Integrity;
using PlayFab.Json;
using PlayFabPersonal.Managers;

[Serializable]
public class ServerResponse
{
    public bool error;
    public string message;
    public object payload; // Adjust the type based on the expected payload structure
}

public class PlayIntegrityManager : MonoBehaviour
{
    private string playFabFunctionUrl;
    private StandardIntegrityTokenProvider integrityTokenProvider;
    private long cloudProjectNumber;
    

    private void OnEnable()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(LoadConfiguration());
#endif
    }

    private IEnumerator LoadConfiguration()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Configuration file not found at path: " + path);
            yield break;
        }

        string json = request.downloadHandler.text;
#else
        if (!File.Exists(path))
        {
            Debug.LogError("Configuration file not found at path: " + path);
            yield break;
        }

       string json = File.ReadAllText(path);
#endif

        Configuration config = JsonUtility.FromJson<Configuration>(json);
        cloudProjectNumber = config.cloudProjectNumber;
        playFabFunctionUrl = config.playFabFunctionUrl;

     //   Debug.Log("Configuration loaded successfully.");

#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(PrepareIntegrityTokenCoroutine());
#else
     //   Debug.LogWarning(" Token Request can only be performed on an Android device.");
#endif
    }

    [Serializable]
    public class Configuration
    {
        public long cloudProjectNumber;
        public string playFabFunctionUrl;
        public string app_open_android;
        public string banner_android;
        public string interstitial_android;
        public string rewarded_android;
        public string rewarded_interstitial;
    }

    private IEnumerator PrepareIntegrityTokenCoroutine()
    {
        
        // Create an instance of a standard integrity manager.
        var standardIntegrityManager = new StandardIntegrityManager();

        // Request the token provider.
        var integrityTokenProviderOperation = standardIntegrityManager.PrepareIntegrityToken(
            new PrepareIntegrityTokenRequest(cloudProjectNumber));
        
        // Wait for PlayAsyncOperation to complete.
        yield return integrityTokenProviderOperation;
        
        // Check the resulting error code.
        if (integrityTokenProviderOperation.Error != StandardIntegrityErrorCode.NoError)
        {
          //  Debug.LogError("AsyncOperation failed with error: " + integrityTokenProviderOperation.Error);
            yield break;
        }
        
        // Get the response.
        integrityTokenProvider = integrityTokenProviderOperation.GetResult();
       // Debug.Log("Token Provider prepared successfully!");
        
        //RegisterPlayer();
        StartCoroutine(RequestIntegrityTokenCoroutine(PlayfabDataManager.Instance.currentPlayerID));
    }
    

    private string GenerateRequestHash(string userId)
    {
        string dataToHash = userId + DateTime.UtcNow.ToString("o"); // Using userId and timestamp for the hash
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private IEnumerator RequestIntegrityTokenCoroutine(string playFabId)
    {
        if (integrityTokenProvider == null)
        {
           // Debug.LogError("TokenProvider is not prepared.");
            yield break;
        }

        string requestHash = GenerateRequestHash(playFabId); // Generate request hash

        var integrityTokenOperation = integrityTokenProvider.Request(new StandardIntegrityTokenRequest(requestHash));

        // Wait for PlayAsyncOperation to complete.
        yield return integrityTokenOperation;

        // Check the resulting error code.
        if (integrityTokenOperation.Error != StandardIntegrityErrorCode.NoError)
        {
          //  Debug.LogError("StandardIntegrityAsyncOperation failed with error: " + integrityTokenOperation.Error);
            yield break;
        }

        // Get the response.
        var integrityToken = integrityTokenOperation.GetResult();
        string tokenString = integrityToken.Token; // Convert to string
        SendTokenToPlayFab(tokenString); // Ensure this sends to your PlayFab function, which will forward to your server
    }


    private void SendTokenToPlayFab(string token)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "validateIntegrityToken",
            RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
            FunctionParameter = new { token },
            GeneratePlayStreamEvent = true
        };


        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptFailure);
    }
    
    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
       // Debug.Log("Cloud Script executed successfully.");
        if (result.Error != null)
        {
            //Debug.LogError("Cloud Script error: " + result.Error.Message);
            return;
        }

        var functionResultJson = JsonUtility.ToJson(result.FunctionResult);
       // Debug.Log("Cloud Script function result: " + functionResultJson);

        var response = (JsonObject)result.FunctionResult;

        if (response.ContainsKey("error") && (bool)response["error"])
        {
            var errorMessage = response.ContainsKey("message") ? response["message"].ToString() : "Unknown error";
           // Debug.LogError("Server response error: " + errorMessage);
            // Handle error (e.g., notify the player, log the error, etc.)
        }
        else
        {
            var successMessage = response.ContainsKey("message") ? response["message"].ToString() : "Operation successful";
            //Debug.Log("Server response success: " + successMessage);
            // Handle success (e.g., proceed with the game, update UI, etc.)
        }
    }

    private void OnCloudScriptFailure(PlayFabError error)
    {
      //  Debug.LogError("Error sending token: " + error.GenerateErrorReport());
    }


    // private void HandleServerResponse(string response)
    // {
    //     var jsonResponse = JsonUtility.FromJson<ServerResponse>(response);
    //
    //     if (jsonResponse.error)
    //     {
    //         Debug.LogError("Server response error: " + jsonResponse.message);
    //         // Handle error (e.g., notify the player, log the error, etc.)
    //     }
    //     else
    //     {
    //         Debug.Log("Server response success: " + jsonResponse.message);
    //         // Handle success (e.g., proceed with the game, update UI, etc.)
    //     }
    // }
}
