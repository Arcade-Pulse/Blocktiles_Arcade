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
    public object payload; // Adjust type based on expected payload structure
}

public class PlayIntegrityManager : MonoBehaviour
{
    private string playFabFunctionUrl;
    private StandardIntegrityTokenProvider integrityTokenProvider;
    private long cloudProjectNumber;

    private void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
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
            Debug.LogError($"[PlayIntegrity] ERROR: Configuration file not found at path: {path}");
            yield break;
        }

        string json = request.downloadHandler.text;
#else
        if (!File.Exists(path))
        {
            Debug.LogError($"[PlayIntegrity] ERROR: Configuration file not found at path: {path}");
            yield break;
        }

        string json = File.ReadAllText(path);
#endif

        Configuration config = JsonUtility.FromJson<Configuration>(json);
        cloudProjectNumber = config.cloudProjectNumber;
        playFabFunctionUrl = config.playFabFunctionUrl;

        Debug.Log($"[PlayIntegrity] Configuration Loaded ✅ | Cloud Project: {cloudProjectNumber} | PlayFab URL: {playFabFunctionUrl}");

#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(PrepareIntegrityTokenCoroutine());
#else
        Debug.LogWarning("[PlayIntegrity] WARNING: Token Request can only be performed on an Android device.");
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
        Debug.Log("[PlayIntegrity] Preparing Integrity Token...");

        var standardIntegrityManager = new StandardIntegrityManager();
        var integrityTokenProviderOperation = standardIntegrityManager.PrepareIntegrityToken(
            new PrepareIntegrityTokenRequest(cloudProjectNumber));

        yield return integrityTokenProviderOperation;

        if (integrityTokenProviderOperation.Error != StandardIntegrityErrorCode.NoError)
        {
            Debug.LogError($"[PlayIntegrity] ERROR: Token Provider Preparation Failed - {integrityTokenProviderOperation.Error}");
            yield break;
        }

        integrityTokenProvider = integrityTokenProviderOperation.GetResult();
        Debug.Log("[PlayIntegrity] ✅ Token Provider Prepared Successfully!");

        StartCoroutine(RequestIntegrityTokenCoroutine(PlayfabDataManager.Instance.currentPlayerID));
    }

    private string GenerateRequestHash(string userId)
    {
        string dataToHash = userId + DateTime.UtcNow.ToString("o");
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
        Debug.Log($"[PlayIntegrity] Requesting Integrity Token for PlayFab ID: {playFabId}");

        if (integrityTokenProvider == null)
        {
            Debug.LogError("[PlayIntegrity] ERROR: TokenProvider is not prepared.");
            yield break;
        }

        string requestHash = GenerateRequestHash(playFabId);
        var integrityTokenOperation = integrityTokenProvider.Request(new StandardIntegrityTokenRequest(requestHash));

        yield return integrityTokenOperation;

        if (integrityTokenOperation.Error != StandardIntegrityErrorCode.NoError)
        {
            Debug.LogError($"[PlayIntegrity] ERROR: Token Request Failed - {integrityTokenOperation.Error}");
            yield break;
        }

        var integrityToken = integrityTokenOperation.GetResult();
        string tokenString = integrityToken.Token;

        Debug.Log($"[PlayIntegrity] ✅ Integrity Token Retrieved Successfully! Token Length: {tokenString.Length}");
        SendTokenToPlayFab(tokenString);
    }

    private void SendTokenToPlayFab(string token)
    {
        Debug.Log($"[PlayIntegrity] Sending Token to PlayFab: {token.Substring(0, 20)}... (truncated)");

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
        Debug.Log($"[PlayIntegrity] ✅ PlayFab Cloud Script Executed Successfully!");
        
        if (result.Error != null)
        {
         //   Debug.LogError($"[PlayIntegrity] ERROR: Cloud Script Error - {result.Error.Message}");
            return;
        }

        var functionResultJson = JsonUtility.ToJson(result.FunctionResult);
        Debug.Log($"[PlayIntegrity] Cloud Script Function Result: {functionResultJson}");

        var response = (JsonObject)result.FunctionResult;

        if (response.ContainsKey("error") && (bool)response["error"])
        {
            var errorMessage = response.ContainsKey("message") ? response["message"].ToString() : "Unknown error";
            KickPlayer();
            //  Debug.LogError($"[PlayIntegrity] ERROR: Server Response - {errorMessage}");
        }
        else
        {
            var successMessage = response.ContainsKey("message") ? response["message"].ToString() : "Operation successful";
            Debug.Log($"[PlayIntegrity] ✅ Server Response Success: {successMessage}");
        }
    }

    private void OnCloudScriptFailure(PlayFabError error)
    {
        Debug.LogError($"[PlayIntegrity] ERROR: PlayFab Cloud Script Execution Failed - {error.GenerateErrorReport()}");
    }
    
    
    private void KickPlayer()
    {
        Debug.LogError("[PlayIntegrity] 🚨 Player integrity check failed! Kicking player...");
    
        // **Disconnect player and return to main menu or login screen**
        PlayFabClientAPI.ForgetAllCredentials();
        Application.Quit();
        
    }
}
