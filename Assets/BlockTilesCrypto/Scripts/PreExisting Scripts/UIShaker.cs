using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIShaker : MonoBehaviour
{
	private sealed class _StartRotate_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timeCounter___1;

		internal float _rotateTime___1;

		internal float _zAngle___2;

		internal Quaternion _currentAngle___2;

		internal float _zAngle___3;

		internal Quaternion _currentAngle___3;

		internal float _zAngle___4;

		internal Quaternion _currentAngle___4;

		internal UIShaker _this;

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

		public _StartRotate_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (this._this.preDelay > 0f)
				{
					this._current = new WaitForSeconds(this._this.preDelay);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				break;
			case 1u:
				break;
			case 2u:
				goto IL_14D;
			case 3u:
				goto IL_231;
			case 4u:
				goto IL_30F;
			case 5u:
				break;
			default:
				return false;
			}
			if (!this._this.running)
			{
				goto IL_320;
			}
			this._timeCounter___1 = 0f;
			this._rotateTime___1 = this._this.duration * 0.25f;
			IL_14D:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___2 = Mathf.Lerp(0f, this._this.fromAngle, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___2 = this._this.gameObject.transform.rotation;
				this._this.gameObject.transform.rotation = Quaternion.Euler(this._currentAngle___2.x, this._currentAngle___2.y, this._zAngle___2);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._timeCounter___1 = 0f;
			this._rotateTime___1 = this._this.duration * 0.5f;
			IL_231:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___3 = Mathf.Lerp(this._this.fromAngle, this._this.toAngle, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___3 = this._this.gameObject.transform.rotation;
				this._this.gameObject.transform.rotation = Quaternion.Euler(this._currentAngle___3.x, this._currentAngle___3.y, this._zAngle___3);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			}
			this._timeCounter___1 = 0f;
			this._rotateTime___1 = this._this.duration * 0.25f;
			IL_30F:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___4 = Mathf.Lerp(this._this.toAngle, 0f, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___4 = this._this.gameObject.transform.rotation;
				this._this.gameObject.transform.rotation = Quaternion.Euler(this._currentAngle___4.x, this._currentAngle___4.y, this._zAngle___4);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 4;
				}
				return true;
			}
			IL_320:
			this._current = new WaitForSeconds(this._this.delay);
			if (!this._disposing)
			{
				this._PC = 5;
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

	public float preDelay;

	public float duration;

	public float delay;

	public float fromAngle;

	public float toAngle;

	private float preDelayOrigin;

	private float durationOrigin;

	private float delayOrigin;

	private float fromAngleOrigin;

	private float toAngleOrigin;

	private bool running = true;

	private void OnEnable()
	{
		base.gameObject.transform.rotation = Quaternion.identity;
		base.StartCoroutine(this.StartRotate());
	}

	private void Start()
	{
		if (this.duration != 0f)
		{
			this.preDelayOrigin = this.preDelay;
			this.durationOrigin = this.duration;
			this.delayOrigin = this.delay;
			this.fromAngleOrigin = this.fromAngle;
			this.toAngleOrigin = this.toAngle;
		}
	}

	public void Reset()
	{
		if (this.delayOrigin != 0f)
		{
			this.running = true;
			this.preDelay = this.preDelayOrigin;
			this.duration = this.durationOrigin;
			this.delay = this.delayOrigin;
			this.fromAngle = this.fromAngleOrigin;
			this.toAngle = this.toAngleOrigin;
		}
	}

	public void Pause()
	{
		this.running = false;
	}

	public void Resume()
	{
		this.running = true;
	}

	public void ShakeVibrate(float duration = 0.02f, float fromAngle = 5f, float toAngle = -5f)
	{
		this.delay = 0f;
		this.duration = duration;
		this.fromAngle = fromAngle;
		this.toAngle = toAngle;
	}

	private IEnumerator StartRotate()
	{
		UIShaker._StartRotate_c__Iterator0 _StartRotate_c__Iterator = new UIShaker._StartRotate_c__Iterator0();
		_StartRotate_c__Iterator._this = this;
		return _StartRotate_c__Iterator;
	}
}
