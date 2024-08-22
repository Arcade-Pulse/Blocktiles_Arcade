using CodeStage.AntiCheat.ObscuredTypes;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
class FunctionResult {
    public string expression;
    public string result;
    public string error;
}


public class ChallengePlayer : MonoBehaviour
{
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
            if (result.Error != null) {
                // Debug.LogError(result.Error.Message);
                this.gameObject.SetActive(false);
                return;
            }

            var functionResult = JsonUtility.FromJson<FunctionResult>(result.FunctionResult.ToString());
            this.gameObject.SetActive(false);
           // Debug.Log(functionResult.result);
        }, error => {
            this.gameObject.SetActive(false);
          //  Debug.LogError("Error calling cloud script: " + error.GenerateErrorReport());
        });
      
    }

    // Update the UI with the received expression
    private void UpdateChallengeUI(string expression) {
        textOfNumbers.text = "Solve the following:\n" + expression;
    }



    // public IEnumerator SumDigits(string input)
    // {
    //     yield return new WaitUntil(()=>PlayFabDataManager.Instance.receivedCoin);
    //
    //     int sum = 0;
    //     
    //   //  Debug.Log(" input is "+input);
    //   textOfNumbers.text = "What is the result of: 0";
    //
    //     foreach (char digitChar in input)
    //     {
    //      //   Debug.Log(" char is "+digitChar);
    //
    //         if (char.IsDigit(digitChar))
    //         {
    //             int digit = digitChar - '0'; // Convert character to integer
    //             textOfNumbers.text += "+"+digit;
    //             //Debug.Log("digit is "+digit);
    //             sum += digit;
    //         }
    //     }
    // }

    private void OnEnable()
    {
        RequestCurrentChallengeExpression();
     //  StartCoroutine( SumDigits(PlayFabDataManager.Instance.Totalcoins.ToString()));
    }
    

    // public void SendVerificationNumber()
    // {
    //     var myNum = numField.text.ToString();
    //     
    //     ObscuredString funcName = "checkVerification";
    //     var request = new ExecuteCloudScriptRequest
    //     {
    //         FunctionName = funcName,
    //         GeneratePlayStreamEvent = true,
    //         FunctionParameter = new
    //         {
    //             sentNum = myNum
    //         },
    //         RevisionSelection = PlayFabDataManager.Instance.cloudScriptVersion
    //        
    //     };
    //  
    //     PlayFabClientAPI.ExecuteCloudScript(request,OnRecieveTransactionDetails,OnErrorCloud);
    //     this.gameObject.SetActive(false);
    //
    // }

    // private void OnErrorCloud(PlayFabError obj)
    // {
    //    // throw new NotImplementedException();
    //    this.gameObject.SetActive(false);
    // }
    //
    // private void OnRecieveTransactionDetails(ExecuteCloudScriptResult obj)
    // {
    //     //  throw new NotImplementedException();
    // }
}
