using System;
using PlayFab;
using PlayFab.ClientModels;

namespace PlayFabPersonal.Managers
{
    // public static class PlayfabRequests
    // {
    //     private static CloudScriptRevisionOption revisionOption;
    //
    //     public static bool DevelopmentMode { get; set; } = false;
    //
    //     static PlayfabRequests()
    //     {
    //         UpdateRevisionOption();
    //     }
    //
    //     public static void CloudScriptExecution(string functionName, Action<ExecuteCloudScriptResult> OnSuccess, Action<PlayFabError> OnError, object functionParameter = null, bool playStreamEvent = true)
    //     {
    //         ExecuteCloudScriptRequest request = new()
    //         {
    //             FunctionName = functionName,
    //             FunctionParameter = functionParameter,
    //             GeneratePlayStreamEvent = playStreamEvent,
    //             RevisionSelection = revisionOption
    //         };
    //
    //         PlayFabClientAPI.ExecuteCloudScript(request, OnSuccess, OnError);
    //     }
    //
    //     public static void UpdateRevisionOption()
    //     {
    //         if (DevelopmentMode)
    //         {
    //             // Use latest revision in development mode
    //             revisionOption = CloudScriptRevisionOption.Latest;
    //         }
    //         else
    //         {
    //             // Use live revision in production mode
    //             revisionOption = CloudScriptRevisionOption.Live;
    //         }
    //     }
    // }
}