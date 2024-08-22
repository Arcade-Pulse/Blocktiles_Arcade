using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Exit : MonoBehaviour
{
	private sealed class _Wait_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal bool isHome;

		internal Exit _this;

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

		public _Wait_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				MainState.SetState(MainState.State.Exit);
				this._current = new WaitForSeconds(this._this.duration);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this.isHome)
				{
					MainState.SetState(MainState.State.Home);
				}
				else
				{
					MainState.SetState(MainState.State.GameOver);
				}
				this._this.SetActive(false);
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

	public float duration;

	public void Reset(bool isActive)
	{
		this.SetActive(isActive);
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}

	public void StartWait(bool isHome)
	{
		this.SetActive(true);
		base.StartCoroutine(this.Wait(isHome));
	}

	private IEnumerator Wait(bool isHome)
	{
		Exit._Wait_c__Iterator0 _Wait_c__Iterator = new Exit._Wait_c__Iterator0();
		_Wait_c__Iterator.isHome = isHome;
		_Wait_c__Iterator._this = this;
		return _Wait_c__Iterator;
	}
}
