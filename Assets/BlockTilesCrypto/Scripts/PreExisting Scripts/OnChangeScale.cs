using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OnChangeScale : MonoBehaviour
{
	private sealed class _StartScaleInOut_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal OnChangeScale _this;

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

		public _StartScaleInOut_c__Iterator0()
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
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			case 2u:
				this._timer___0 = 0f;
				goto IL_164;
			case 3u:
				goto IL_164;
			default:
				return false;
			}
			if (this._timer___0 > this._this.duration)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._timer___0 += Time.deltaTime;
			this._this.transform.localScale = Vector3.Lerp(this._this.transform.localScale, this._this.to, this._timer___0 / this._this.duration);
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
			IL_164:
			if (this._timer___0 <= this._this.duration)
			{
				this._timer___0 += Time.deltaTime;
				this._this.transform.localScale = Vector3.Lerp(this._this.transform.localScale, this._this.from, this._timer___0 / this._this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			}
			this._this.transform.localScale = this._this.from;
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

	public float duration;

	public float f;

	private Vector3 to;

	private Vector3 from;

	private IEnumerator scale;

	private bool running;

	private void Awake()
	{
		this.from = base.transform.localScale;
		this.to = this.from * this.f;
		this.running = false;
	}

	private void OnEnable()
	{
		base.transform.localScale = this.from;
	}

	public void OnChange()
	{
		if (this.running)
		{
			this.running = false;
			base.StopCoroutine(this.scale);
		}
		this.scale = this.StartScaleInOut();
		base.StartCoroutine(this.scale);
	}

	private IEnumerator StartScaleInOut()
	{
		OnChangeScale._StartScaleInOut_c__Iterator0 _StartScaleInOut_c__Iterator = new OnChangeScale._StartScaleInOut_c__Iterator0();
		_StartScaleInOut_c__Iterator._this = this;
		return _StartScaleInOut_c__Iterator;
	}
}
