using CodeStage.AntiCheat.Storage;
using Global;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using PlayFabPersonal.Users;
using UnityEngine;

namespace PlayFabPersonal.Authentications
{
    public class PlayfabAuthentication : MonoBehaviour
    {
        [Header("Registration Fields")]
        [SerializeField] private PlayfabRegistrationFields playfabRegistrationFields;
        [Header("Login Fields")]
        [SerializeField] private PlayfabLoginFields playfabLoginFields;

        [Header("Game Panels")]
        [SerializeField] private GameObject loginPanel;
        [SerializeField] private GameObject playfabRegisterPanel;

        private void Start()
        {
            playfabLoginFields.registerPanelButton.onClick.AddListener(() =>
            {
                playfabRegisterPanel.SetActive(true);
                loginPanel.SetActive(false);
            });

            playfabRegistrationFields.backButton.onClick.AddListener(() =>
            {
                playfabRegisterPanel.SetActive(false);
                loginPanel.SetActive(true);
            });

            playfabLoginFields.loginButton.onClick.AddListener(LoginWithEmailButton);
            playfabRegistrationFields.registerAccountButton.onClick.AddListener(RegisterPlayfabAccount);

            if (
               ObscuredPrefs.HasKey(PlayerPrefNameString.LAST_LOGIN) &&
               ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) == PlayerPrefNameString.PLAYFAB
            )
            {
                AutoLoginWithEmailButton();
            }
        }

        private void AutoLoginWithEmailButton()
        {
            GameSceneManager.Instance.ShowLoadingPanel();
            var request = new LoginWithEmailAddressRequest
            {
                Email = ObscuredPrefs.Get("playfabEmail", null),
                Password = ObscuredPrefs.Get("playfabPassword", null)
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessLoginWithEmailAddress, OnErrorLoginWithEmailAddress);
        }

        private void LoginWithEmailButton()
        {
            if (playfabLoginFields.IsLoginInputEmpty())
            {
                var request = new LoginWithEmailAddressRequest
                {
                    Email = playfabLoginFields.emailAddressField.text,
                    Password = playfabLoginFields.passwordField.text,
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
                };

                PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessLoginWithEmailAddress, OnErrorLoginWithEmailAddress);
            }
        }

        private void OnSuccessLoginWithEmailAddress(LoginResult result)
        {
            GameSceneManager.Instance.ShowLoadingPanel();
           // Debug.Log("Login Success");
            if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) != PlayerPrefNameString.PLAYFAB)  // save access token to ObscuredPrefs so it can be automatically sign in
            {
                ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.PLAYFAB);
                ObscuredPrefs.Set("playfabEmail", playfabLoginFields.emailAddressField.text);
                ObscuredPrefs.Set("playfabPassword", playfabLoginFields.passwordField.text);
            }

            // CAll game next steps here
            Debug.Log("Next Step");
            PlayfabDataManager.Instance.SetPlayerID(result.PlayFabId);
            UserAccount.OnEverythingSuccessLoadGameScene?.Invoke();
        }

        private void OnErrorLoginWithEmailAddress(PlayFabError error)
        {
                GoogleAuthentication.Instance.GPGSLoginButton();
   //         Debug.Log(error.ErrorMessage);
        }

        private void RegisterPlayfabAccount()
        {
           // Debug.Log(playfabRegistrationFields.IsRegistrationInputEmpty());
            if (playfabRegistrationFields.IsRegistrationInputEmpty())
            {
               // Debug.Log("execute");
                var request = new RegisterPlayFabUserRequest
                {
                    // DisplayName = playfabRegistrationFields.fullnameField.text,
                    Email = playfabRegistrationFields.emailAddressField.text
                };

                if (playfabRegistrationFields.IsPasswordMatched())
                    request.Password = playfabRegistrationFields.passwordField.text;
                request.RequireBothUsernameAndEmail = false;

                PlayFabClientAPI.RegisterPlayFabUser(request, OnSuccess, OnError);
            }
        }

        private void OnSuccess(RegisterPlayFabUserResult result)
        {
            GameSceneManager.Instance.ShowLoadingPanel();
            if (ObscuredPrefs.Get(PlayerPrefNameString.LAST_LOGIN, null) != PlayerPrefNameString.PLAYFAB)  // save access token to ObscuredPrefs so it can be automatically sign in
            {
                ObscuredPrefs.Set(PlayerPrefNameString.LAST_LOGIN, PlayerPrefNameString.PLAYFAB);
                ObscuredPrefs.Set("playfabEmail", playfabRegistrationFields.emailAddressField.text);
                ObscuredPrefs.Set("playfabPassword", playfabRegistrationFields.passwordField.text);
            }

            PlayFabClientAPI.AddOrUpdateContactEmail(new AddOrUpdateContactEmailRequest { EmailAddress = playfabRegistrationFields.emailAddressField.text }, OnSuccessAddOrUpdateContactEmail, OnErrorAddOrUpdateContactEmail);
        }

        private void OnSuccessAddOrUpdateContactEmail(AddOrUpdateContactEmailResult result)
        {
            UserAccount.OnLoginSuccess?.Invoke(PlayerPrefNameString.PLAYFAB);
        }

        private void OnErrorAddOrUpdateContactEmail(PlayFabError error)
        {
            UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
        }

        private void OnError(PlayFabError error)
        {
            UserAccount.OnLoginFailed?.Invoke(error.ErrorMessage);
        }
    }
}
