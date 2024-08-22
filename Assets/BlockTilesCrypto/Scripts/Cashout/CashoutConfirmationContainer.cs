using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cashout
{
    [Serializable]
    public class CashoutConfirmationContainer
    {
        public TMP_Text coinInCryptoCurrency;
        public TMP_Text transferingToEmailText;
        public TMP_Text usageMessageText;
        
        public Image paymentMethodImage;
        public Texture2D[] paymentMethodTextures;

        public Button confirmButton;
        public Button cancelButton;

        public GameObject confirmationPanel;
    }
}