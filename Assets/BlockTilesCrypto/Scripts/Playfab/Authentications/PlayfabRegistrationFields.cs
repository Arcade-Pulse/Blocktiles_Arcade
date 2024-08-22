using System;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class PlayfabRegistrationFields
{
    // public TMP_InputField fullnameField;
    public TMP_InputField emailAddressField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordAgainField;

    public TMP_Text errorMessage;

    public Button registerAccountButton;
    public Button backButton;


    public bool IsPasswordMatched()
    {
        if (passwordField.text == passwordAgainField.text)
            return true;
        else
            return false;
    }

    public bool IsRegistrationInputEmpty()
    {
        // Check if both email and password fields are not empty
        if (string.IsNullOrEmpty(emailAddressField.text) || string.IsNullOrEmpty(passwordField.text) || string.IsNullOrEmpty(passwordAgainField.text))
        {
            errorMessage.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(emailAddressField.text))
                errorMessage.text = "Email address field empty";
            else if (string.IsNullOrEmpty(passwordField.text))
                errorMessage.text = "Password field empty";
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
