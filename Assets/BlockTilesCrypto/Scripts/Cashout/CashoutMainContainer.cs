using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cashout
{
    [Serializable]
    public class CashoutMainContainer
    {
        public TMP_Text gameSocCoinText;
        public TMP_Text coinInCryptoCurrency;

        public Button coinbaseButton;
        public Button binanceButton;

        public Texture2D[] coinbaseTextures;
        public Texture2D[] binanceTextures;
        
        public GameObject mainContainerPanel;

        [Header("Payment Allow")]
        public GameObject paymentAllowedContainer;
        public TMP_InputField cashoutEmailField; 
        public TMP_Text cashoutEmailUsageText;
        public Button proceedButton;
        public Button cancelButton;
        public Button lastTransactionStatusButton;

        [Header("Insufficient Amount")]
        public GameObject sufficientAmountContainer;
        public TMP_Text requiredAmountText;
        public Image currencyProgressBar;

        [Header("Last Transaction")]
        public GameObject lastTransactionContainer;        
        public TMP_Text lastTransactionEmail;
        public TMP_Text lastTransactionAmount;
        public TMP_Text lastTransactionDate;
        public TMP_Text messageText;
        public TMP_Text message1Text;
        public TMP_Text emailLabelText;

        [Header("Next Cashout")]
        public GameObject nextCashoutContainer;
        public TMP_Text nextCashoutTimerText;
    }
}