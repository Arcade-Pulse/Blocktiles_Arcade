using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClickCanvasButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private sealed class _ButtonClick_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentTime___0;

		internal OnClickCanvasButton _this;

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
				goto IL_14A;
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
			this._this.onClick.Invoke();
			MainAudio.Main.PlaySound(TypeAudio.SoundClick);
			this._currentTime___0 = 0f;
			IL_14A:
			if (this._currentTime___0 < this._this.timeZoomIn)
			{
				this._this.transform.localScale = Vector3.Lerp(this._this.scaleZoom, Vector3.one, this._currentTime___0 / this._this.timeZoomIn);
				this._currentTime___0 += Time.unscaledDeltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.running = false;
			this._PC = -1;
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

	public UnityEvent onClick;

	public Vector3 scaleZoom = new Vector3(1.2f, 1.2f, 1.2f);

	private float timeZoomOut = 0.05f;

	private float timeZoomIn = 0.1f;

	private bool running;

	private void OnEnable()
	{
		this.running = false;
		base.transform.localScale = Vector3.one;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!this.running)
		{
			base.StartCoroutine(this.ButtonClick());
		}
	}

	private void OnApplicationPause()
	{
		this.running = false;
		base.transform.localScale = Vector3.one;
	}

	public IEnumerator ButtonClick()
	{
		OnClickCanvasButton._ButtonClick_c__Iterator0 _ButtonClick_c__Iterator = new OnClickCanvasButton._ButtonClick_c__Iterator0();
		_ButtonClick_c__Iterator._this = this;
		return _ButtonClick_c__Iterator;
	}
}
