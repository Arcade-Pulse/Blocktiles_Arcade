using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeUnit : MonoBehaviour
{
	private sealed class _StartDrop_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal float durationMove;

		internal Vector2 startPos;

		internal Vector2 targetPos;

		internal Vector3 _targetScale___0;

		internal CubeUnit _this;

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

		public _StartDrop_c__Iterator0()
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
				break;
			case 1u:
				break;
			case 2u:
				goto IL_146;
			default:
				return false;
			}
			if (this._timer___0 < this.durationMove)
			{
				this._timer___0 += Time.deltaTime;
				this._this.transform.position = Vector2.Lerp(this.startPos, this.targetPos, this._timer___0 / this.durationMove);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.transform.position = this.targetPos;
			this._timer___0 = 0f;
			this._targetScale___0 = Vector3.one;
			IL_146:
			if (this._timer___0 < this._this.durationDrop)
			{
				this._timer___0 += Time.deltaTime;
				this._this.transform.localScale = Vector3.Lerp(this._this.transform.localScale, this._targetScale___0, this._timer___0 / this._this.durationDrop);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.transform.localScale = this._targetScale___0;
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

	private sealed class _EffectHideBlock_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _directCheck___0;

		internal float _rSpeed___0;

		internal float _startX___0;

		internal float _startY___0;

		internal float _randRotate___0;

		internal float _t___0;

		internal CubeUnit _this;

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

		public _EffectHideBlock_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.SetLayer(3);
				this._this.speedX = UnityEngine.Random.Range(this._this.speedXMin, this._this.speedXMax);
				this._this.speed0 = UnityEngine.Random.Range(this._this.speed0Min, this._this.speed0Max);
				this._this.duration = UnityEngine.Random.Range(this._this.durationMin, this._this.durationMax);
				this._directCheck___0 = ((UnityEngine.Random.value <= 0.5f) ? (-1) : 1);
				this._rSpeed___0 = this._this.speedRotate * this._this.speedX;
				this._startX___0 = this._this.transform.position.x;
				this._startY___0 = this._this.transform.position.y;
				this._randRotate___0 = UnityEngine.Random.Range(-this._this.speedRotate, this._this.speedRotate);
				this._t___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._t___0 < this._this.duration)
			{
				this._t___0 += Time.deltaTime;
				this._startX___0 += (float)this._directCheck___0 * this._this.speedX * Time.deltaTime;
				this._this.transform.position = new Vector2(this._startX___0, this._startY___0 + this._this.GetY(this._t___0));
				this._this.transform.localScale = Vector3.one * Mathf.LerpUnclamped(1f, this._this.minScale, this._this.acScale.Evaluate(this._t___0 / this._this.duration));
				this._this.transform.Rotate(Vector3.forward * Time.deltaTime * this._randRotate___0);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			MainObjControl.Instant.pattemCreater.SetCube(this._this);
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

	public global::Types thisType;

	public SpriteRenderer render;

	public int ID;

	public int row;

	public int col;

	public int indexRow;

	public int indexCol;

	public float scale;

	public float targetGray;

	public float durationDrop;

	public Vector3 dropScaleMin;

	private float speed0;

	private float speedX;

	private float duration;

	public float speed0Min;

	public float speed0Max;

	public float speedXMin;

	public float speedXMax;

	public float durationMin;

	public float durationMax;

	public float G;

	public AnimationCurve acScale;

	public float minScale;

	public float speedRotate;

	public GameObject GetObj
	{
		get
		{
			return base.gameObject;
		}
	}

	public void SetSprite(Sprite data, int newID)
	{
		this.ID = newID;
		this.render.sprite = data;
	}

	public void SetSprite(Sprite sprite)
	{
		this.render.sprite = sprite;
	}

	public void SetLayer(int layer)
	{
		this.render.sortingOrder = layer;
	}

	public void SetAlpha(float alpha)
	{
		this.render.color = new Color(1f, 1f, 1f, alpha);
	}

	public void RotateUnit()
	{
		float x = base.transform.position.x - (float)(this.indexCol - this.indexRow) * this.scale;
		float y = base.transform.position.y - (float)(this.indexRow + this.indexCol) * this.scale;
		int num = this.indexCol;
		this.indexCol = this.indexRow;
		this.indexRow = -num;
		base.transform.position = new Vector3(x, y);
	}

	public void MoveCenterPos(Vector2 pos, Vector2 center)
	{
		base.transform.position = pos + new Vector2(((float)this.indexCol - center.x) * this.scale, ((float)this.indexRow - center.y) * this.scale);
	}

	public void ApplyRotate()
	{
		if (this.thisType != global::Types.O1)
		{
			base.transform.position = new Vector3((float)this.col, (float)this.row);
		}
	}

	public void DropDown(Vector2 startPos, Vector2 targetPos, float durationMove)
	{
		base.StartCoroutine(this.StartDrop(startPos, targetPos, durationMove));
	}

	private IEnumerator StartDrop(Vector2 startPos, Vector2 targetPos, float durationMove)
	{
		CubeUnit._StartDrop_c__Iterator0 _StartDrop_c__Iterator = new CubeUnit._StartDrop_c__Iterator0();
		_StartDrop_c__Iterator.durationMove = durationMove;
		_StartDrop_c__Iterator.startPos = startPos;
		_StartDrop_c__Iterator.targetPos = targetPos;
		_StartDrop_c__Iterator._this = this;
		return _StartDrop_c__Iterator;
	}

	public void Effect()
	{
		base.StartCoroutine(this.EffectHideBlock());
	}

	private IEnumerator EffectHideBlock()
	{
		CubeUnit._EffectHideBlock_c__Iterator1 _EffectHideBlock_c__Iterator = new CubeUnit._EffectHideBlock_c__Iterator1();
		_EffectHideBlock_c__Iterator._this = this;
		return _EffectHideBlock_c__Iterator;
	}

	private float GetY(float t)
	{
		return this.speed0 * t + 0.5f * this.G * t * t;
	}

	public Vector2 GetCorrectPos()
	{
		return new Vector2((float)this.col, (float)this.row);
	}
}
