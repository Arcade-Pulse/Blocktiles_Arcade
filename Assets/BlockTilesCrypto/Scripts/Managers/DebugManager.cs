using Global;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private Transform inGameDebug;

    private void Awake()
    {
        inGameDebug.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Check if the player is a developer
        if (!PlayerPrefs.HasKey(PlayerPrefNameString.DEVELOPER)) return;
        bool isDeveloper = PlayerPrefs.GetInt(PlayerPrefNameString.DEVELOPER, 0) == 1;

        // Show something based on whether it's a developer account
        if (isDeveloper)
        {
            // Display something specific for developer accounts
        //    Debug.Log("This is a developer account.");
            ShowDebug();
        }
        else
        {
            // Display something for non-developer accounts
           // Debug.Log("This is not a developer account.");
        }
    }

    public void ShowDebug()
    {
        inGameDebug.gameObject.SetActive(true);
    }
}
