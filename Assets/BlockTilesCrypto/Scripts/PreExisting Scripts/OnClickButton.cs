using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class OnClickButton : MonoBehaviour
{
	private sealed class _ButtonClickZoomOut_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentTime___0;

		internal OnClickButton _this;

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

		public _ButtonClickZoomOut_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._currentTime___0 = this._this.timeZoomOut * (this._this.transform.localScale.x - this._this.fromScale.x) / this._this.distanceScale;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._currentTime___0 < this._this.timeZoomOut)
			{
				this._this.transform.localScale = Vector3.Lerp(this._this.fromScale, this._this.toScale, this._currentTime___0 / this._this.timeZoomOut);
				this._currentTime___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
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

	private sealed class _ButtonClickZoomIn_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentTime___0;

		internal OnClickButton _this;

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

		public _ButtonClickZoomIn_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._currentTime___0 = this._this.timeZoomIn * (-this._this.transform.localScale.x + this._this.toScale.x) / this._this.distanceScale;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._currentTime___0 < this._this.timeZoomIn)
			{
				this._this.transform.localScale = Vector3.Lerp(this._this.toScale, this._this.fromScale, this._currentTime___0 / this._this.timeZoomIn);
				this._currentTime___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
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

	private sealed class _ButtonClickEnd_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _currentTime___0;

		internal OnClickButton _this;

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

		public _ButtonClickEnd_c__Iterator2()
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
				this._currentTime___0 = this._this.timeZoomOut * (this._this.transform.localScale.x - this._this.fromScale.x) / this._this.distanceScale;
				break;
			case 1u:
				break;
			case 2u:
				goto IL_177;
			default:
				return false;
			}
			if (this._currentTime___0 < this._this.timeZoomOut)
			{
				this._this.transform.localScale = Vector3.Lerp(this._this.fromScale, this._this.toScale, this._currentTime___0 / this._this.timeZoomOut);
				this._currentTime___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._currentTime___0 = 0f;
			IL_177:
			if (this._currentTime___0 < this._this.timeZoomIn)
			{
				this._this.transform.localScale = Vector3.Lerp(this._this.toScale, this._this.fromScale, this._currentTime___0 / this._this.timeZoomIn);
				this._currentTime___0 += Time.deltaTime;
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

	public int number;

	public float scaleZoom = 1.2f;

	public float distanceScale;

	private float timeZoomOut = 0.1f;

	private float timeZoomIn = 0.2f;

	private Vector3 fromScale;

	private Vector3 toScale;

	public bool onPos;

	public bool running;

	private void Start()
	{
		this.fromScale = base.transform.localScale;
		this.toScale = this.fromScale * this.scaleZoom;
		this.distanceScale = this.toScale.x - this.fromScale.x;
	}

	public abstract void ClickedButton();

	private void Update()
	{
		if (Input.GetMouseButtonUp(0) && this.onPos && !this.running)
		{
			this.onPos = false;
			this.ClickedButton();
			base.StopAllCoroutines();
			base.StartCoroutine("ButtonClickEnd");
		}
	}

	private void OnMouseEnter()
	{
		this.onPos = true;
		if (!this.running)
		{
			base.StopAllCoroutines();
			base.StartCoroutine("ButtonClickZoomOut");
		}
	}

	private void OnMouseDown()
	{
		this.onPos = true;
	}

	private void OnMouseExit()
	{
		this.onPos = false;
		if (!this.running)
		{
			base.StopAllCoroutines();
			base.StartCoroutine("ButtonClickZoomIn");
		}
	}

	public IEnumerator ButtonClickZoomOut()
	{
		OnClickButton._ButtonClickZoomOut_c__Iterator0 _ButtonClickZoomOut_c__Iterator = new OnClickButton._ButtonClickZoomOut_c__Iterator0();
		_ButtonClickZoomOut_c__Iterator._this = this;
		return _ButtonClickZoomOut_c__Iterator;
	}

	public IEnumerator ButtonClickZoomIn()
	{
		OnClickButton._ButtonClickZoomIn_c__Iterator1 _ButtonClickZoomIn_c__Iterator = new OnClickButton._ButtonClickZoomIn_c__Iterator1();
		_ButtonClickZoomIn_c__Iterator._this = this;
		return _ButtonClickZoomIn_c__Iterator;
	}

	public IEnumerator ButtonClickEnd()
	{
		OnClickButton._ButtonClickEnd_c__Iterator2 _ButtonClickEnd_c__Iterator = new OnClickButton._ButtonClickEnd_c__Iterator2();
		_ButtonClickEnd_c__Iterator._this = this;
		return _ButtonClickEnd_c__Iterator;
	}
}
