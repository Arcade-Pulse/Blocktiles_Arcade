using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScaleInOut : MonoBehaviour
{
	private sealed class _StartScaleInOut_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___1;

		internal ScaleInOut _this;

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
				break;
			case 1u:
				goto IL_9D;
			case 2u:
				this._timer___1 = 0f;
				goto IL_142;
			case 3u:
				goto IL_142;
			default:
				return false;
			}
			IL_29:
			this._timer___1 = 0f;
			IL_9D:
			if (this._timer___1 > this._this.duration)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._timer___1 += Time.deltaTime;
			this._this.transform.localScale = Vector3.Lerp(Vector3.one, this._this.to, this._timer___1 / this._this.duration);
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
			IL_142:
			if (this._timer___1 > this._this.duration)
			{
				goto IL_29;
			}
			this._timer___1 += Time.deltaTime;
			this._this.transform.localScale = Vector3.Lerp(this._this.to, Vector3.one, this._timer___1 / this._this.duration);
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 3;
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

	public Vector3 to;

	public float duration;

	public bool iSActive
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	public void StartAnim()
	{
		base.StartCoroutine(this.StartScaleInOut());
	}

	private IEnumerator StartScaleInOut()
	{
		ScaleInOut._StartScaleInOut_c__Iterator0 _StartScaleInOut_c__Iterator = new ScaleInOut._StartScaleInOut_c__Iterator0();
		_StartScaleInOut_c__Iterator._this = this;
		return _StartScaleInOut_c__Iterator;
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}
}
