using CodeStage.AntiCheat.Storage;
using Global;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace PlayFabPersonal.Users
{
    [System.Serializable]
    public class LinkedAccountCheckResult
    {
        public bool isLinked;
    }
    
    public class UserAccount : MonoBehaviour
    {
        private string loginType;
        public static UnityEvent<string> OnLoginSuccess = new();
        public static UnityEvent<string> OnLoginFailed = new();
        public static UnityEvent OnEverythingSuccessLoadGameScene = new();

        private void Start()
        {
            OnLoginSuccess.AddListener(GetUserAccountInfo_OnLoginSuccess);
            OnLoginFailed.AddListener(Error_OnLoginFailed);
        }
        public static UserAccount Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }
        private void OnDisable()
        {
            OnLoginSuccess.RemoveListener(GetUserAccountInfo_OnLoginSuccess);
            OnLoginFailed.RemoveListener(Error_OnLoginFailed);
        }

        private void GetUserAccountInfo_OnLoginSuccess(string loginType)
        {
            this.loginType = loginType;
            if (loginType == PlayerPrefNameString.GUEST)
            {
                if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) != PlayerPrefNameString.GUEST)  // save access token to PlayerPrefs so it can be automatically sign in
                {
                    ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.GUEST);
                }
            }

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnSuccessGetAccountInfo, OnErrorGetAccountInfo);
        }

        private void OnSuccessGetAccountInfo(GetAccountInfoResult result)
        {
            // if (result.AccountInfo.TitleInfo.DisplayName == null)
            // {
            if (loginType == PlayerPrefNameString.GUEST)
            {
                PlayfabDataManager.Instance.SetPlayerID(result.AccountInfo.PlayFabId);
                //         userName = "Guest" + GeneralFunctions.RandomDigitCodeGeneration(1);
            }
            //     else if (loginType == PlayerPrefNameString.GOOGLE)
            //     {
            //         userName = result.AccountInfo.GooglePlayGamesInfo.GooglePlayGamesPlayerDisplayName;
            //     }
            //     else if (loginType == PlayerPrefNameString.FACEBOOK)
            //     {
            //         userName = result.AccountInfo.FacebookInfo.FullName;
            //     }

            //     var request = new UpdateUserTitleDisplayNameRequest
            //     {
            //         DisplayName = userName
            //     };

            //     PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccessUpdateUserTitleDisplayName, OnErrorUpdateUserTitleDisplayName);
            // }
            // else
            // {
            //     // CAll game next steps here
            //     Debug.Log("Next Step");
            OnEverythingSuccessLoadGameScene?.Invoke();

            
            // }
        }

        public void CheckLinkedAccounts()
        {
            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "checkLinkedAccounts1A", // This must match your Cloud Script function name
                GeneratePlayStreamEvent = true,
                RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision()
            };
            PlayFabClientAPI.ExecuteCloudScript(request, OnCheckLinkedAccountsSuccess, OnCheckLinkedAccountsFailure);
        }
        
        
        private void OnCheckLinkedAccountsSuccess(ExecuteCloudScriptResult result)
        {
            //Debug.Log("Raw server response: " + result.FunctionResult.ToString());  // This will show the exact server response
            if (result.FunctionResult != null)
            {
                var functionResult = JsonUtility.FromJson<LinkedAccountCheckResult>(result.FunctionResult.ToString());
                //Debug.Log("Is linked to Facebook or Google: " + functionResult.isLinked);
                PlayfabDataManager.Instance.isGuestAccountLinked = functionResult.isLinked;
                // OnEverythingSuccessLoadGameScene?.Invoke();

            }
            else
            {
                PlayfabDataManager.Instance.isGuestAccountLinked = false;
               // OnEverythingSuccessLoadGameScene?.Invoke();
                Debug.Log("No function result!");
            }
        }

        private void OnCheckLinkedAccountsFailure(PlayFabError error)
        {
            // Debug.LogError("Cloud Script failed");
            // Debug.LogError(error.GenerateErrorReport());
            PlayfabDataManager.Instance.isGuestAccountLinked = false;
            OnEverythingSuccessLoadGameScene?.Invoke();
        }
        
        // private void OnSuccessUpdateUserTitleDisplayName(UpdateUserTitleDisplayNameResult result)
        // {
        //     // CAll game next steps here
        //     Debug.Log("Next Step");
        //     OnEverythingSuccessLoadGameScene?.Invoke();
        // }

        // private void OnErrorUpdateUserTitleDisplayName(PlayFabError error)
        // {
        //     var request = new UpdateUserTitleDisplayNameRequest
        //     {
        //         DisplayName = userName + GeneralFunctions.RandomDigitCodeGeneration(1)
        //     };
        //     PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccessUpdateUserTitleDisplayName, OnErrorUpdateUserTitleDisplayName);
        // }

        private void OnErrorGetAccountInfo(PlayFabError error)
        {
           // Debug.Log(error.ErrorMessage);
        }

        private void Error_OnLoginFailed(string error)
        {
            GameSceneManager.Instance.HideLoadingPanel();
            ObscuredPrefs.DeleteKey(PlayerPrefNameString.LAST_LOGIN);
           // Debug.Log(error);
        }
    }
}
