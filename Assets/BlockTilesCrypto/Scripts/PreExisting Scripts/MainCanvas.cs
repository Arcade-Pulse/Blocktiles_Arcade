using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
	private sealed class _loadPattemDelay_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _loadPattemDelay_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(0.7f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				MainObjControl.Instant.nextViewerCtr.GetNewPattems();
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private static MainCanvas mainCanvas;

	public InGameScript inGameScript;

	public LostScript lostScript;

	public PauseScript pauseScript;

	public FaderControl faderScript;

	public static MainCanvas Main
	{
		get
		{
			return MainCanvas.mainCanvas;
		}
	}

	private void Awake()
	{
		MainCanvas.mainCanvas = this;
	}

	private void Preload()
	{
		MainState.state = MainState.State.Ingame;
		this.CheckPreloadTutorial();
		MainObjControl.Instant.Preload();
		MainState.TypePlay typePlay = MainState.typePlay;
		if (typePlay != MainState.TypePlay.Tutorial)
		{
			if (typePlay != MainState.TypePlay.Normal)
			{
			}
		}
		else
		{
			MainObjControl.Instant.tutorial.Show();
		}
		this.inGameScript.Preload();
		this.lostScript.Preload();
		this.pauseScript.PreLoad();
	}

	private void CheckPreloadTutorial()
	{
		int getTimePlay = Settings.GetTimePlay;
		if (getTimePlay == 0)
		{
			MainState.typePlay = MainState.TypePlay.Tutorial;
			Settings.SetTimePlay(getTimePlay + 1);
		}
		else
		{
			MainState.typePlay = MainState.TypePlay.Normal;
		}
	}

	private void Start()
	{
		this.Preload();
		this.Reset(false);
	}

	public void Reset(bool isIngame)
	{
		if (isIngame)
		{
			MainState.state = MainState.State.Ingame;
			this.faderScript.FadeInPanel(this.inGameScript.group, null);
		}
		else
		{
			this.inGameScript.group.alpha = 1f;
			this.inGameScript.SetActive(true);
		}
		this.inGameScript.Reset();
		this.lostScript.Reset();
		this.pauseScript.Reset();
		this.faderScript.Reset();
		this.lostScript.Reset();
		this.EffectGrid();
        // AdsControl.Instance.ShowBanner();
	}

	private void EffectGrid()
	{
		MainState.TypePlay typePlay = MainState.typePlay;
		if (typePlay != MainState.TypePlay.Tutorial)
		{
			if (typePlay == MainState.TypePlay.Normal)
			{
				if (Settings.GetContinue == 0)
				{
					MainObjControl.Instant.screenCtr.StartAnim();
					base.StartCoroutine(this.loadPattemDelay());
				}
				else
				{
					MainObjControl.Instant.screenCtr.StartNoAnim();
					MainObjControl.Instant.grid.LoadDataSave();
				}
			}
		}
		else
		{
			MainObjControl.Instant.screenCtr.StartNoAnim();
		}
	}

	private IEnumerator loadPattemDelay()
	{
		return new MainCanvas._loadPattemDelay_c__Iterator0();
	}
}
