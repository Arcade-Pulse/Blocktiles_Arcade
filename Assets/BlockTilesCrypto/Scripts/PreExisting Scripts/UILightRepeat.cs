using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UILightRepeat : MonoBehaviour
{
	private sealed class _StartLight_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal UILightRepeat _this;

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

		public _StartLight_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.image.enabled = false;
				this._current = new WaitForSeconds(this._this.delayStart);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				break;
			case 2u:
				this._this.image.enabled = false;
				this._current = new WaitForSeconds(this._this.delayLoop);
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			case 3u:
				break;
			default:
				return false;
			}
			this._this.image.enabled = true;
			this._current = new WaitForSeconds(this._this.liveTime);
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

	public float delayStart;

	public float delayLoop;

	public float liveTime;

	private SpriteRenderer image;

	private void Start()
	{
		this.image = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.StartLight());
	}

	private IEnumerator StartLight()
	{
		UILightRepeat._StartLight_c__Iterator0 _StartLight_c__Iterator = new UILightRepeat._StartLight_c__Iterator0();
		_StartLight_c__Iterator._this = this;
		return _StartLight_c__Iterator;
	}
}
