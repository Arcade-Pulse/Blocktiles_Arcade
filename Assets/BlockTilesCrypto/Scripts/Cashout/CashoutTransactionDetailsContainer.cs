using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cashout
{
    [Serializable]
    public class CashoutTransactionDetailsContainer
    {
        public TMP_Text coinInUSDCurrency;
        public TMP_Text transferingToEmailText;
        public TMP_Text transactionStatusText;
        public TMP_Text transactionDateText;

        public Image paymentMethodImage;
        public Texture2D[] paymentMethodTextures;

        public Button doneButton;
        public Button shareReceiptButton;

        public GameObject transactionDetailPanel;
    }
}