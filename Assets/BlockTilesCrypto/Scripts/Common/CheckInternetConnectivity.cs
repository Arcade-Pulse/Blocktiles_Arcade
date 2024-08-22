using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CommonScripts
{
    public static class CheckInternetConnectivity
    {
        public static IEnumerator CheckForInternetConnection(string uri, Action<bool> action)
        {
            using var webRequest = UnityWebRequest.Get(uri);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

           // string[] pages = uri.Split('/');
            //int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    // Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    action(false);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    action(false);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    action(true);
                    break;
            }
        }
        
        
    }
}