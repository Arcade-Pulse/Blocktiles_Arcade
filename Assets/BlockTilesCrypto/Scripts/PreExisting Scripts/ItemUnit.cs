using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUnit : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private sealed class _ButtonClick_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentTime___0;

		internal ItemUnit _this;

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

		public _ButtonClick_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.running = true;
				this._currentTime___0 = 0f;
				break;
			case 1u:
				break;
			case 2u:
				goto IL_13E;
			case 3u:
				this._this.running = false;
				this._PC = -1;
				return false;
			default:
				return false;
			}
			if (this._currentTime___0 < this._this.timeZoomOut)
			{
				this._this.transform.localScale = Vector3.Lerp(Vector3.one, this._this.scaleZoom, this._currentTime___0 / this._this.timeZoomOut);
				this._currentTime___0 += Time.unscaledDeltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			MainAudio.Main.PlaySound(TypeAudio.SoundClick);
			this._currentTime___0 = 0f;
			IL_13E:
			if (this._currentTime___0 >= this._this.timeZoomIn)
			{
				this._current = new WaitForSeconds(0.3f);
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			}
			this._this.transform.localScale = Vector3.Lerp(this._this.scaleZoom, Vector3.one, this._currentTime___0 / this._this.timeZoomIn);
			this._currentTime___0 += Time.unscaledDeltaTime;
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 2;
			}
			return true;
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

	public int index;

	public StarLevelControl star1;

	public StarLevelControl star2;

	public StarLevelControl star3;

	public Text numberText;

	public GameObject lockObj;

	public GameObject openObj;

	public Vector3 scaleZoom = new Vector3(1.5f, 1.5f, 1.5f);

	private float timeZoomOut = 0.1f;

	private float timeZoomIn = 0.2f;

	private bool running;

	private void OnEnable()
	{
		this.running = false;
		base.transform.localScale = Vector3.one;
	}

	public void SetLock()
	{
		this.lockObj.SetActive(true);
		this.openObj.SetActive(false);
		this.star1.SetActiveObj(false);
		this.star2.SetActiveObj(false);
		this.star3.SetActiveObj(false);
	}

	public void SetOpen(int maxLv)
	{
		this.lockObj.SetActive(false);
		this.openObj.SetActive(true);
		this.numberText.text = this.index.ToString();
		if (this.index == maxLv)
		{
			this.star1.SetActiveObj(false);
			this.star2.SetActiveObj(false);
			this.star3.SetActiveObj(false);
		}
		else
		{
			this.star1.SetActive(true);
			this.star2.SetActive(true);
			this.star3.SetActive(true);
			int star = Settings.GetStar(this.index);
			if (star == 1)
			{
				this.star1.SetActive(true);
				this.star2.SetActive(false);
				this.star3.SetActive(false);
			}
			else if (star == 2)
			{
				this.star1.SetActive(true);
				this.star2.SetActive(true);
				this.star3.SetActive(false);
			}
			else
			{
				this.star1.SetActive(true);
				this.star2.SetActive(true);
				this.star3.SetActive(true);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}

	public IEnumerator ButtonClick()
	{
		ItemUnit._ButtonClick_c__Iterator0 _ButtonClick_c__Iterator = new ItemUnit._ButtonClick_c__Iterator0();
		_ButtonClick_c__Iterator._this = this;
		return _ButtonClick_c__Iterator;
	}
}
