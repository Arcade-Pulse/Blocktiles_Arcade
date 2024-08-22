using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
	public CanvasGroup group;

	public Image soundIgame1;

	//public Image soundIgame2;

	public Sprite soundOn;

	public Sprite soundOff;

	public Image soundBGIgame1;

	//public Image soundBGIgame2;

	public Sprite soundBGOn;

	public Sprite soundBGOff;

	private bool isFading;

	public void Reset()
	{
		this.SetActive(false);
	}

	public void PreLoad()
	{
		this.CheckSound();
	}

	public void CheckSound()
	{
		if (Settings.GetSound == 0)
		{
			this.soundIgame1.sprite = this.soundOff;
			//this.soundIgame2.sprite = this.soundOff;
			MainAudio.Main.SoundOff();
		}
		else
		{
			this.soundIgame1.sprite = this.soundOn;
			//this.soundIgame2.sprite = this.soundOn;
			MainAudio.Main.SoundOn();
		}
		if (Settings.GetBGSound == 0)
		{
			this.soundBGIgame1.sprite = this.soundBGOff;
			//this.soundBGIgame2.sprite = this.soundBGOff;
			MainAudio.Main.SoundBgOff();
		}
		else
		{
			this.soundBGIgame1.sprite = this.soundBGOn;
			//this.soundBGIgame2.sprite = this.soundBGOn;
			MainAudio.Main.SoundBgOn();
		}
	}

	public void SoundClick()
	{
		if (Settings.GetSound == 1)
		{
			this.soundIgame1.sprite = this.soundOff;
			//this.soundIgame2.sprite = this.soundOff;
			Settings.SetSound(0);
			MainAudio.Main.SoundOff();
		}
		else
		{
			this.soundIgame1.sprite = this.soundOn;
			//this.soundIgame2.sprite = this.soundOn;
			Settings.SetSound(1);
			MainAudio.Main.SoundOn();
		}
		MainAudio.Main.PlaySound(TypeAudio.SoundClick);
	}

	public void SoundBGClick()
	{
		if (Settings.GetBGSound == 1)
		{
			this.soundBGIgame1.sprite = this.soundBGOff;
			//this.soundBGIgame2.sprite = this.soundBGOff;
			Settings.SetBGSound(0);
			MainAudio.Main.SoundBgOff();
		}
		else
		{
			this.soundBGIgame1.sprite = this.soundBGOn;
			//this.soundBGIgame2.sprite = this.soundBGOn;
			Settings.SetBGSound(1);
			MainAudio.Main.SoundBgOn();
		}
	}

	public void PauseGame()
	{
		if (MainState.typePlay == MainState.TypePlay.Normal && MainState.state != MainState.State.Waiting && !this.isFading)
		{
			this.isFading = true;
			MainCanvas.Main.faderScript.FadeInPanelFix(this.group, new FaderControl.Callback0(this.OffFading));
			MainState.SetState(MainState.State.Pause);
			MainAudio.Main.PlaySound(TypeAudio.SoundClick);
		}
	}

	public void UnPause()
	{
		if (!this.isFading)
		{
			MainState.SetState(MainState.State.Ingame);
			this.isFading = true;
			MainCanvas.Main.faderScript.FadeOutPanelFix(this.group, new FaderControl.Callback0(this.OffFading));
		}
	}

	public void Restart()
	{
		if (!MainObjControl.Instant.screenCtr.gridMoving)
		{
            // AdsControl.Instance.showAds();
			MainCanvas.Main.faderScript.Fade(new FaderControl.Callback0(this.MidleTryAgain));
		}
	}

	private void OffFading()
	{
		this.isFading = false;
	}

	public void MidleTryAgain()
	{
		Settings.SetContinue(0);
		MainObjControl.Instant.grid.SetAllBlock();
		MainObjControl.Instant.nextViewerCtr.SetAllBlock();
		MainObjControl.Instant.planeViewCrt.SetAllBlock();
		MainObjControl.Instant.helpCtr.HideAllBlock();
		MainCanvas.Main.faderScript.FadeInPanel(MainCanvas.Main.inGameScript.group, null);
		MainCanvas.Main.Reset(true);
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}

	public void RateClick()
	{
		Application.OpenURL("http://play.google.com/store/apps/details?id=" + Application.identifier);
	}

	public void MoreClick()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=HyBird+Games+Ltd.");
	}
}
