using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartGameScript : MonoBehaviour
{
	public RectTransform rec;

	public CanvasGroup group;

	public float duration;

	public Text tetrisText;

	public Text tapText;

	public Image moreImage;

	public Image rateImage;

	public Image likeImage;

	public void Reset()
	{
		this.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && MainState.GetState == MainState.State.Home && !EventSystem.current.IsPointerOverGameObject(0))
		{
			this.StartPlayGame();
		}
	}

	public void StartPlayGame()
	{
		MainAudio.Main.PlaySound(TypeAudio.SoundClick);
		this.Middle();
	}

	private void Middle()
	{
		MainState.SetState(MainState.State.Ingame);
		CanvasGroup group = MainCanvas.Main.inGameScript.group;
		MainCanvas.Main.faderScript.SwichPanel(this.group, group, null);
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}

	public void FbClick()
	{
	}

	public void RateClick()
	{
	}

	public void MoreClick()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=WaterApp");
	}
}
