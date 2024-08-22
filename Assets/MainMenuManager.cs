using System;
using System.Collections;
using System.Collections.Generic;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    
    [SerializeField] private Button playButton;
  //  [SerializeField] private bool isPlayed = false;

    public Button getLivesButton;
    public Button signOutButton;
    public Button cashoutButton;
    public Button getLivesButton1;

    public int zeroLivesTimesPlayed;

    [SerializeField] private LoadingScreen loadingScreen;

    public static MainMenuManager Instance { get; private set; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadGameScene()
    {
        // Disable button interaction to prevent multiple presses
        SetButtonInteractable(false);

        // Check if the player has no lives left
        if (PlayfabDataManager.Instance.GetLives() == 0)
        {
            // Increment the count for how many times the player has entered with no lives
            PlayfabDataManager.Instance.timesEnteredWithNoLives++;

            // Get the maximum number of times a player can enter with no lives
            int maxEnters = FirebaseSettings.Instance.numberItCanEnterWithNoLives;

            // If the player has exceeded the allowed number of entries with no lives
            if (PlayfabDataManager.Instance.timesEnteredWithNoLives > maxEnters)
            {
                // Show an error message prompting the player to get more lives
                MessageManager.OnErrorShowMessage?.Invoke("You have 0 lives, please press the 'Get Lives' button");

                // Re-enable the button so the player can press it again
                SetButtonInteractable(true);

                // Exit the function to prevent loading the game scene
                return;
            }

            // Optionally, show an ad when the player has no lives left (commented out in your original code)
            // ShowAdIfLoaded();

            // Load the game scene even if the player has no lives left
            SceneManager.LoadSceneAsync("GameScene");

            // Exit the function to prevent further execution
            return;
        }

        // If the player has lives, subtract one life
        VirtualCurrency.Instance.SubtractLife(1);

        // Load the game scene
        SceneManager.LoadSceneAsync("GameScene");
    }
    
    
    internal void SetButtonInteractable(bool value)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            playButton.interactable = value;
            getLivesButton.interactable = value;
            getLivesButton1.interactable = value;
            signOutButton.interactable = value;
        }
    }
    
    public void ShowLoadingPanel()
    {
        if (loadingScreen!=null && loadingScreen.loadingPanel!=null)
        {
            loadingScreen.loadingPanel.SetActive(true);
        }
    }

    public void HideLoadingPanel()
    {
        loadingScreen.loadingPanel.SetActive(false);
    }
}
