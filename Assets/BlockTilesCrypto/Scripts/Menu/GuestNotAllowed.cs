using UnityEngine;
using UnityEngine.UI;

public class GuestNotAllowed : MonoBehaviour
{
    [SerializeField] private Button registrationButton;
    [SerializeField] private Button cancelButton;

    [SerializeField] private GameObject accountLinkerPanel;
    public GameObject cashoutPanel;
    public GameObject mainMenu;

    private void OnEnable()
    {
        registrationButton.onClick.AddListener(ShowAccountLinkerPanel);
        cancelButton.onClick.AddListener(HideGuestNotAllowedPanel);
     //   Debug.Log("it is enabled");
    }

    private void OnDisable()
    {
        registrationButton.onClick.RemoveListener(ShowAccountLinkerPanel);
        cancelButton.onClick.RemoveListener(HideGuestNotAllowedPanel);
    }

    private void HideGuestNotAllowedPanel()
    {
        mainMenu.SetActive(true);
        cashoutPanel.SetActive(false);
        this.gameObject.SetActive(false);
        

    }

    private void ShowAccountLinkerPanel()
    {
        HideGuestNotAllowedPanel();
        accountLinkerPanel.SetActive(true);
    }
}