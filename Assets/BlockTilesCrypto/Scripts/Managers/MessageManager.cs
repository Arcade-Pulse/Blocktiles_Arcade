using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] messageTexures;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Image icon;

    public static UnityEvent<string> OnSuccessShowMessage = new();
    public static UnityEvent<string> OnErrorShowMessage = new();

    private void OnEnable()
    {
        cancelButton.onClick.AddListener(OnCancelHideErrorMessage);
        OnSuccessShowMessage.AddListener(ShowSuccessMessage);
        OnErrorShowMessage.AddListener(ShowErrorMessage);
    }

    private void OnDisable()
    {
        cancelButton.onClick.RemoveListener(OnCancelHideErrorMessage);
        OnErrorShowMessage.RemoveListener(ShowErrorMessage);
    }

    private void OnCancelHideErrorMessage()
    {
        messagePanel.SetActive(false);
    }

    private void ShowSuccessMessage(string message)
    {
        var successLogo = messageTexures[0];
        icon.sprite = Sprite.Create(successLogo, new Rect(0.0f, 0.0f, successLogo.width, successLogo.height), new Vector2(), 100.0f);
        messagePanel.SetActive(true);
        messageText.text = message;
    }

    private void ShowErrorMessage(string message)
    {
        var errorLogo = messageTexures[1];
        icon.sprite = Sprite.Create(errorLogo, new Rect(0.0f, 0.0f, errorLogo.width, errorLogo.height), new Vector2(), 100.0f);
        messagePanel.SetActive(true);
        messageText.text = message;
    }
}
