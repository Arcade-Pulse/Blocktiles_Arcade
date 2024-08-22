using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class ListenBackButton : MonoBehaviour
{
    private float time = -1f;

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (Keyboard.current.escapeKey.wasPressedThisFrame && Time.time - this.time > 0.2f)
#else
        if (Input.GetKeyDown(KeyCode.Escape) && Time.time - this.time > 0.2f)
#endif
        {
            switch (MainState.GetState)
            {
                case MainState.State.Home:
                    HomeBack();
                    break;
                case MainState.State.Ingame:
                    IngameBack();
                    break;
                case MainState.State.GameOver:
                    GameOverBack();
                    break;
                case MainState.State.Pause:
                    PauseBack();
                    break;
            }
            this.time = Time.time;
        }
    }

    private void HomeBack()
    {
        Application.Quit();
    }

    private void IngameBack()
    {
        MainCanvas.Main.pauseScript.PauseGame();
    }

    private void GameOverBack()
    {
        MainCanvas.Main.lostScript.TryAgainButton();
    }

    private void PauseBack()
    {
        MainCanvas.Main.pauseScript.UnPause();
    }
}