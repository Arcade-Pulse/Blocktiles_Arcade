using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CupShaker : MonoBehaviour
{
	private sealed class _StartMove_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal Vector2 _targetScale___0;

		internal CupShaker _this;

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

		public _StartMove_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0f;
				this._targetScale___0 = Vector2.one * this._this.scale;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationMove)
			{
				this._timer___0 += Time.deltaTime;
				this._this.rec.localScale = Vector2.Lerp(this._this.rec.localScale, this._targetScale___0, this._timer___0 / this._this.durationMove);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._timer___0 = 0f;
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

	private sealed class _StartEffect_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _time___0;

		internal float _timeCounter___1;

		internal float _rotateTime___1;

		internal float _zAngle___2;

		internal Quaternion _currentAngle___2;

		internal float _zAngle___3;

		internal Quaternion _currentAngle___3;

		internal float _zAngle___4;

		internal Quaternion _currentAngle___4;

		internal CupShaker _this;

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

		public _StartEffect_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._time___0 = 0;
				goto IL_31C;
			case 1u:
				break;
			case 2u:
				goto IL_203;
			case 3u:
				goto IL_2E1;
			case 4u:
				goto IL_31C;
			default:
				return false;
			}
			IL_11F:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___2 = Mathf.Lerp(0f, this._this.fromAngle, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___2 = this._this.gameObject.transform.rotation;
				this._this.transform.rotation = Quaternion.Euler(this._currentAngle___2.x, this._currentAngle___2.y, this._zAngle___2);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._timeCounter___1 = 0f;
			this._rotateTime___1 = this._this.duration * 0.5f;
			IL_203:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___3 = Mathf.Lerp(this._this.fromAngle, this._this.toAngle, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___3 = this._this.gameObject.transform.rotation;
				this._this.gameObject.transform.rotation = Quaternion.Euler(this._currentAngle___3.x, this._currentAngle___3.y, this._zAngle___3);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._timeCounter___1 = 0f;
			this._rotateTime___1 = this._this.duration * 0.25f;
			IL_2E1:
			if (this._timeCounter___1 < this._rotateTime___1)
			{
				this._timeCounter___1 += Time.deltaTime;
				this._zAngle___4 = Mathf.Lerp(this._this.toAngle, 0f, this._timeCounter___1 / this._rotateTime___1);
				this._currentAngle___4 = this._this.gameObject.transform.rotation;
				this._this.gameObject.transform.rotation = Quaternion.Euler(this._currentAngle___4.x, this._currentAngle___4.y, this._zAngle___4);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 3;
				}
				return true;
			}
			IL_2F2:
			this._current = new WaitForSeconds(this._this.delay);
			if (!this._disposing)
			{
				this._PC = 4;
			}
			return true;
			IL_31C:
			if (this._time___0 >= this._this.timeShake)
			{
				this._PC = -1;
			}
			else
			{
				this._time___0++;
				if (this._this.running)
				{
					this._timeCounter___1 = 0f;
					this._rotateTime___1 = this._this.duration * 0.25f;
					goto IL_11F;
				}
				goto IL_2F2;
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

	public RectTransform rec;

	public float duration;

	public float delay;

	public float fromAngle;

	public float toAngle;

	public int timeShake;

	public float durationMove;

	public float distanceMove;

	public float waitMoveDown;

	public float scale;

	private bool running = true;

	private Vector2 normalPos;

	private void OnEnable()
	{
		this.rec.localScale = Vector2.one;
	}

	public void StartShake()
	{
		this.rec.localScale = Vector2.one;
		base.StartCoroutine(this.StartMove());
	}

	private IEnumerator StartMove()
	{
		CupShaker._StartMove_c__Iterator0 _StartMove_c__Iterator = new CupShaker._StartMove_c__Iterator0();
		_StartMove_c__Iterator._this = this;
		return _StartMove_c__Iterator;
	}

	private IEnumerator StartEffect()
	{
		CupShaker._StartEffect_c__Iterator1 _StartEffect_c__Iterator = new CupShaker._StartEffect_c__Iterator1();
		_StartEffect_c__Iterator._this = this;
		return _StartEffect_c__Iterator;
	}
}
