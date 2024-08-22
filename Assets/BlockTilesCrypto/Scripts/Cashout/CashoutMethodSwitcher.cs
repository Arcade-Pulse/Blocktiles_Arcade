using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Cashout
{
    public class CashoutMethodSwitcher : MonoBehaviour
    {
        CashoutManager cashoutManager;
        CashoutMainContainer mainContainer;
        CashoutConfirmationContainer confirmationContainer;

        private void Start()
        {
            cashoutManager = GetComponent<CashoutManager>();
            mainContainer = cashoutManager.mainContainer;
            confirmationContainer = cashoutManager.confirmationContainer;

            SelectCoinbase();
            mainContainer.coinbaseButton.onClick.AddListener(SelectCoinbase);
            mainContainer.binanceButton.onClick.AddListener(SelectBinance);
        }

        private void SelectCoinbase()
        {
            var coinbaseUnselectTexture = mainContainer.coinbaseTextures[0];
            var binanceUnselectTexture = mainContainer.binanceTextures[1];
            var coinbasePaymentMethodTexture = confirmationContainer.paymentMethodTextures[0];

            mainContainer.coinbaseButton.image.sprite = Sprite.Create(coinbaseUnselectTexture, new Rect(0.0f, 0.0f, coinbaseUnselectTexture.width, coinbaseUnselectTexture.height), new Vector2(), 100.0f);
            mainContainer.binanceButton.image.sprite = Sprite.Create(binanceUnselectTexture, new Rect(0.0f, 0.0f, binanceUnselectTexture.width, binanceUnselectTexture.height), new Vector2(), 100.0f);
            
            SetTextBasedOnSelection("coinbase");
            SetPaymentMethod(coinbasePaymentMethodTexture);
        }

        private void SelectBinance()
        {
            var binanceUnselectTexture = mainContainer.binanceTextures[0];
            var coinbaseUnselectTexture = mainContainer.coinbaseTextures[1];
            var binancePaymentMethodTexture = confirmationContainer.paymentMethodTextures[1];

            mainContainer.binanceButton.image.sprite = Sprite.Create(binanceUnselectTexture, new Rect(0.0f, 0.0f, binanceUnselectTexture.width, binanceUnselectTexture.height), new Vector2(), 100.0f);
            mainContainer.coinbaseButton.image.sprite = Sprite.Create(coinbaseUnselectTexture, new Rect(0.0f, 0.0f, coinbaseUnselectTexture.width, coinbaseUnselectTexture.height), new Vector2(), 100.0f);
           
            SetTextBasedOnSelection("binance");
            SetPaymentMethod(binancePaymentMethodTexture);
        }

        private void SetTextBasedOnSelection(string selection)
        {
            mainContainer.cashoutEmailField.placeholder.GetComponent<TMP_Text>().text = $"Enter your {selection} email";
            mainContainer.cashoutEmailUsageText.text = $"we will use this {selection} email to redeem your game points to BTC";
            cashoutManager.SetCashoutMethod(selection);
        }

        private void SetPaymentMethod(Texture2D paymentMethodTexture)
        {
            confirmationContainer.paymentMethodImage.sprite = Sprite.Create(paymentMethodTexture, new Rect(0.0f, 0.0f, paymentMethodTexture.width, paymentMethodTexture.height), new Vector2(), 100.0f);
        }
    }
}