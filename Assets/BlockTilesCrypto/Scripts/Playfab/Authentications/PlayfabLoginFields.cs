using System;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class PlayfabLoginFields
{
    public TMP_InputField emailAddressField;
    public TMP_InputField passwordField;
    public TMP_Text errorMessage;
    public Button loginButton;
    public Button registerPanelButton;


    public bool IsLoginInputEmpty()
    {
        // Check if both email and password fields are not empty
        if (string.IsNullOrEmpty(emailAddressField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            errorMessage.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(emailAddressField.text))
                errorMessage.text = "Email address field empty";
            else
                errorMessage.text = "Password field empty";
            return false; // Input is not valid
        }
        else
        {
            errorMessage.gameObject.SetActive(false);
            errorMessage.text = "";
            return true; // Input is valid
        }
    }
}
