using CodeStage.AntiCheat.ObscuredTypes;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    [SerializeField] private Transform verificationPopupContainer;

    private const int ChallengeProbability = 0;
    private VerificationManager verificationManager;
    public TextMeshProUGUI textOfNumbers;
    public TMP_InputField numField;
    public Button sendResponseButton;
    
    
    // Call to get the current challenge expression from the server
    public void RequestCurrentChallengeExpression() {
        
        ObscuredString funcName1 = "getCurrentChallengeExpression1A";
        var requestCurrentChallenge = new ExecuteCloudScriptRequest
        {
            FunctionName = funcName1,
            GeneratePlayStreamEvent = true,
            RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision()
        };
        
        PlayFabClientAPI.ExecuteCloudScript(requestCurrentChallenge, result => {
            // if (result.Error != null) {
            //     Debug.LogError(result.Error.Message);
            //     return;
            // }

            var functionResult = JsonUtility.FromJson<FunctionResult>(result.FunctionResult.ToString());
            if (functionResult.expression != null) {
                UpdateChallengeUI(functionResult.expression);
            } else {
                Debug.LogError(functionResult.error);
            }
        }, error => {
            Debug.LogError("Error calling cloud script: " + error.GenerateErrorReport());
        });
    }

    // Call to verify the player's response with the server
    public void SendPlayerResponse()
    {
        sendResponseButton.interactable = false;
        ObscuredString funcName1 = "verifyPlayerResponse1A";
        var sendPlayerResponse = new ExecuteCloudScriptRequest
        {
            FunctionName = funcName1,
            GeneratePlayStreamEvent = true,
            RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
            FunctionParameter = new
            {
                sentNum = numField.text.ToString()
            }
        };
        
        PlayFabClientAPI.ExecuteCloudScript(sendPlayerResponse, result => {
           // Debug.Log("send command succeess");
            if (result.Error != null) {
                // Debug.LogError(result.Error.Message);
                this.gameObject.SetActive(false);
                return;
            }

           // var functionResult = JsonUtility.FromJson<FunctionResult>(result.FunctionResult.ToString());
            this.gameObject.SetActive(false);
            Debug.Log("send command succeess");
        }, error => {
            Debug.Log("error sending command");

            this.gameObject.SetActive(false);
          //  Debug.LogError("Error calling cloud script: " + error.GenerateErrorReport());
        });
        
    }

    // Update the UI with the received expression
    private void UpdateChallengeUI(string expression) {
        textOfNumbers.text = "Solve the following:\n" + expression;
    }

    

    private void OnEnable()
    {
        RequestCurrentChallengeExpression();
     //  StartCoroutine( SumDigits(PlayFabDataManager.Instance.Totalcoins.ToString()));
    }
    


}
