using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EffectControl
{
	public delegate void Callback0();

	private sealed class _WaitForRealSeconds_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _start___0;

		internal float time;

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

		public _WaitForRealSeconds_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._start___0 = Time.realtimeSinceStartup;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (Time.realtimeSinceStartup < this._start___0 + this.time)
			{
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

	private sealed class _MoveLocal_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

		internal EffectControl.Callback0 fn;

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

		public _MoveLocal_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.obj.transform.localPosition;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration && this.obj.activeSelf)
			{
				this.obj.transform.localPosition = Vector3.Lerp(this._from___0, this.to, this._elapsed___0 / this.duration);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.transform.localPosition = this.to;
			if (this.fn != null)
			{
				this.fn();
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

	private sealed class _MoveLocalWaiter_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float time;

		internal float _elapsed___0;

		internal GameObject obj;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

		internal Vector3 _current___1;

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

		public _MoveLocalWaiter_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.time);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._elapsed___0 = 0f;
				this._from___0 = this.obj.transform.localPosition;
				break;
			case 2u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 < this.duration)
			{
				this._current___1 = Vector3.Lerp(this._from___0, this.to, this._elapsed___0 / this.duration);
				this.obj.transform.localPosition = new Vector3(this._current___1.x, this.obj.transform.localPosition.y, this._current___1.z);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this.obj.transform.localPosition = new Vector3(this.to.x, this.obj.transform.localPosition.y, this.to.z);
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

	private sealed class _Move_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

		internal EffectControl.Callback0 fn;

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

		public _Move_c__Iterator3()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.obj.transform.position;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this.obj.transform.position = Vector3.Lerp(this._from___0, this.to, this._elapsed___0 / this.duration);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.transform.position = this.to;
			if (this.fn != null)
			{
				this.fn();
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

	private sealed class _MoveAnchor_c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal RectTransform rec;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector2 to;

		internal EffectControl.Callback0 fn;

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

		public _MoveAnchor_c__Iterator4()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.rec.anchoredPosition;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this.rec.anchoredPosition = Vector2.Lerp(this._from___0, this.to, this._elapsed___0 / this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			if (this.fn != null)
			{
				this.fn();
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

	private sealed class _Scale_c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal Transform trans;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

		internal EffectControl.Callback0 fn;

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

		public _Scale_c__Iterator5()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.trans.localScale;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this.trans.localScale = Vector3.Lerp(this._from___0, this.to, EffectControl.quartEaseInOut(this._elapsed___0 / this.duration));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.trans.localScale = this.to;
			if (this.fn != null)
			{
				this.fn();
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

	private sealed class _ScaleInOut_c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal float duration;

		internal Transform trans;

		internal Vector3 _from___0;

		internal Vector3 outScale;

		internal Vector3 to;

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

		public _ScaleInOut_c__Iterator6()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this.duration *= 0.5f;
				this._from___0 = this.trans.localScale;
				break;
			case 1u:
				break;
			case 2u:
				goto IL_12F;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this.trans.localScale = Vector3.Lerp(this.trans.localScale, this.outScale, this._elapsed___0 / this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._elapsed___0 = 0f;
			IL_12F:
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this.trans.localScale = Vector3.Lerp(this.trans.localScale, this.to, this._elapsed___0 / this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this.trans.localScale = this.to;
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

	private sealed class _FadeIn_c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal Transform obj;

		internal Material _mat___0;

		internal float duration;

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

		public _FadeIn_c__Iterator7()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._mat___0 = this.obj.GetComponent<MeshRenderer>().material;
				this._mat___0.SetFloat("_alpha", 0f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._mat___0.SetFloat("_alpha", this._elapsed___0 / this.duration);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._mat___0.SetFloat("_alpha", 1f);
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

	private sealed class _FadeOut_c__Iterator8 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal Transform obj;

		internal Material _mat___0;

		internal float duration;

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

		public _FadeOut_c__Iterator8()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._mat___0 = this.obj.GetComponent<MeshRenderer>().material;
				this._mat___0.SetFloat("_alpha", 1f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._mat___0.SetFloat("_alpha", 1f - this._elapsed___0 / this.duration);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._mat___0.SetFloat("_alpha", 0f);
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

	private sealed class _FadeOutSprite_c__Iterator9 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal SpriteRenderer _render___0;

		internal Color _color___0;

		internal float duration;

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

		public _FadeOutSprite_c__Iterator9()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._render___0 = this.obj.GetComponent<SpriteRenderer>();
				this._color___0 = this._render___0.color;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this._render___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 1f - this._elapsed___0 / this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.SetActive(false);
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

	private sealed class _FadeOutCanvas_c__IteratorA : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Graphic _graphic___0;

		internal Color _color___0;

		internal float duration;

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

		public _FadeOutCanvas_c__IteratorA()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._graphic___0 = this.obj.GetComponent<Graphic>();
				this._color___0 = this._graphic___0.color;
				this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 1f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.unscaledDeltaTime;
				this._color___0.a = Mathf.Lerp(1f, 0f, this._elapsed___0 / this.duration);
				this._graphic___0.color = this._color___0;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 0f);
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

	private sealed class _FadeInCanvas_c__IteratorB : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Graphic _graphic___0;

		internal Color _color___0;

		internal float duration;

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

		public _FadeInCanvas_c__IteratorB()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._graphic___0 = this.obj.GetComponent<Graphic>();
				this._color___0 = this._graphic___0.color;
				this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 0f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.unscaledDeltaTime;
				this._color___0.a = Mathf.Lerp(0f, 1f, this._elapsed___0 / this.duration);
				this._graphic___0.color = this._color___0;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 1f);
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

	private sealed class _FadeInCanvasTo_c__IteratorC : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Graphic _graphic___0;

		internal Color _color___0;

		internal float duration;

		internal float a;

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

		public _FadeInCanvasTo_c__IteratorC()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._graphic___0 = this.obj.GetComponent<Graphic>();
				this._color___0 = this._graphic___0.color;
				this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 0f);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.unscaledDeltaTime;
				this._color___0.a = Mathf.Lerp(0f, this.a, this._elapsed___0 / this.duration);
				this._graphic___0.color = this._color___0;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._graphic___0.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, this.a);
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

	private sealed class _DeActive_c__IteratorD : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float duration;

		internal GameObject obj;

		internal EffectControl.Callback0 fn;

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

		public _DeActive_c__IteratorD()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.duration);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this.obj.SetActive(false);
				if (this.fn != null)
				{
					this.fn();
				}
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

	private sealed class _StartShakeGO_c__IteratorE : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _shakeTimeCounter___0;

		internal GameObject obj;

		internal Vector3 _originalPos___0;

		internal float duration;

		internal float shakeRange;

		internal float _randX___1;

		internal float _randY___1;

		internal float _randZ___1;

		internal Vector3 _range___1;

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

		public _StartShakeGO_c__IteratorE()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._shakeTimeCounter___0 = 0f;
				this._originalPos___0 = this.obj.transform.position;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._shakeTimeCounter___0 < this.duration)
			{
				this._shakeTimeCounter___0 += Time.unscaledDeltaTime;
				this._randX___1 = EffectControl.RandomShakeRange(this.shakeRange);
				this._randY___1 = EffectControl.RandomShakeRange(this.shakeRange);
				this._randZ___1 = EffectControl.RandomShakeRange(this.shakeRange);
				this._range___1 = new Vector3(this._randX___1, this._randY___1, this._randZ___1);
				this.obj.transform.position = this._originalPos___0 + this._range___1;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.transform.position = this._originalPos___0;
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

	private sealed class _MoveLocalSpecial_c__IteratorF : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

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

		public _MoveLocalSpecial_c__IteratorF()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.obj.transform.localPosition;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this.obj.transform.localPosition = new Vector3(this.obj.transform.localPosition.x, Mathf.Lerp(this._from___0.y, this.to.y, this._elapsed___0 / this.duration), this.obj.transform.localPosition.z);
				this._elapsed___0 += Time.deltaTime;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.transform.localPosition = new Vector3(this.obj.transform.localPosition.x, this.to.y, this.obj.transform.localPosition.z);
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

	private sealed class _RotateLocalZ_c__Iterator10 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _elapsed___0;

		internal GameObject obj;

		internal Vector3 _from___0;

		internal float duration;

		internal Vector3 to;

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

		public _RotateLocalZ_c__Iterator10()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._elapsed___0 = 0f;
				this._from___0 = this.obj.transform.localEulerAngles;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._elapsed___0 <= this.duration)
			{
				this._elapsed___0 += Time.deltaTime;
				this.obj.transform.localEulerAngles = Vector3.Lerp(this._from___0, this.to, this._elapsed___0 / this.duration);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.obj.transform.localEulerAngles = this.to;
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

	public static IEnumerator WaitForRealSeconds(float time)
	{
		EffectControl._WaitForRealSeconds_c__Iterator0 _WaitForRealSeconds_c__Iterator = new EffectControl._WaitForRealSeconds_c__Iterator0();
		_WaitForRealSeconds_c__Iterator.time = time;
		return _WaitForRealSeconds_c__Iterator;
	}

	public static IEnumerator MoveLocal(GameObject obj, Vector3 to, float duration, EffectControl.Callback0 fn)
	{
		EffectControl._MoveLocal_c__Iterator1 _MoveLocal_c__Iterator = new EffectControl._MoveLocal_c__Iterator1();
		_MoveLocal_c__Iterator.obj = obj;
		_MoveLocal_c__Iterator.duration = duration;
		_MoveLocal_c__Iterator.to = to;
		_MoveLocal_c__Iterator.fn = fn;
		return _MoveLocal_c__Iterator;
	}

	public static IEnumerator MoveLocalWaiter(GameObject obj, Vector3 to, float duration, float time)
	{
		EffectControl._MoveLocalWaiter_c__Iterator2 _MoveLocalWaiter_c__Iterator = new EffectControl._MoveLocalWaiter_c__Iterator2();
		_MoveLocalWaiter_c__Iterator.time = time;
		_MoveLocalWaiter_c__Iterator.obj = obj;
		_MoveLocalWaiter_c__Iterator.duration = duration;
		_MoveLocalWaiter_c__Iterator.to = to;
		return _MoveLocalWaiter_c__Iterator;
	}

	public static IEnumerator Move(GameObject obj, Vector3 to, float duration, EffectControl.Callback0 fn)
	{
		EffectControl._Move_c__Iterator3 _Move_c__Iterator = new EffectControl._Move_c__Iterator3();
		_Move_c__Iterator.obj = obj;
		_Move_c__Iterator.duration = duration;
		_Move_c__Iterator.to = to;
		_Move_c__Iterator.fn = fn;
		return _Move_c__Iterator;
	}

	public static IEnumerator MoveAnchor(RectTransform rec, Vector2 to, float duration, EffectControl.Callback0 fn)
	{
		EffectControl._MoveAnchor_c__Iterator4 _MoveAnchor_c__Iterator = new EffectControl._MoveAnchor_c__Iterator4();
		_MoveAnchor_c__Iterator.rec = rec;
		_MoveAnchor_c__Iterator.duration = duration;
		_MoveAnchor_c__Iterator.to = to;
		_MoveAnchor_c__Iterator.fn = fn;
		return _MoveAnchor_c__Iterator;
	}

	public static void MoveAnchorNow(GameObject obj, Vector2 to)
	{
		RectTransform component = obj.GetComponent<RectTransform>();
		component.anchoredPosition = to;
	}

	public static IEnumerator Scale(Transform trans, Vector3 to, float duration, EffectControl.Callback0 fn = null)
	{
		EffectControl._Scale_c__Iterator5 _Scale_c__Iterator = new EffectControl._Scale_c__Iterator5();
		_Scale_c__Iterator.trans = trans;
		_Scale_c__Iterator.duration = duration;
		_Scale_c__Iterator.to = to;
		_Scale_c__Iterator.fn = fn;
		return _Scale_c__Iterator;
	}

	public static IEnumerator ScaleInOut(Transform trans, Vector3 to, Vector3 outScale, float duration)
	{
		EffectControl._ScaleInOut_c__Iterator6 _ScaleInOut_c__Iterator = new EffectControl._ScaleInOut_c__Iterator6();
		_ScaleInOut_c__Iterator.duration = duration;
		_ScaleInOut_c__Iterator.trans = trans;
		_ScaleInOut_c__Iterator.outScale = outScale;
		_ScaleInOut_c__Iterator.to = to;
		return _ScaleInOut_c__Iterator;
	}

	public static IEnumerator FadeIn(Transform obj, float duration)
	{
		EffectControl._FadeIn_c__Iterator7 _FadeIn_c__Iterator = new EffectControl._FadeIn_c__Iterator7();
		_FadeIn_c__Iterator.obj = obj;
		_FadeIn_c__Iterator.duration = duration;
		return _FadeIn_c__Iterator;
	}

	public static IEnumerator FadeOut(Transform obj, float duration)
	{
		EffectControl._FadeOut_c__Iterator8 _FadeOut_c__Iterator = new EffectControl._FadeOut_c__Iterator8();
		_FadeOut_c__Iterator.obj = obj;
		_FadeOut_c__Iterator.duration = duration;
		return _FadeOut_c__Iterator;
	}

	public static IEnumerator FadeOutSprite(GameObject obj, float duration)
	{
		EffectControl._FadeOutSprite_c__Iterator9 _FadeOutSprite_c__Iterator = new EffectControl._FadeOutSprite_c__Iterator9();
		_FadeOutSprite_c__Iterator.obj = obj;
		_FadeOutSprite_c__Iterator.duration = duration;
		return _FadeOutSprite_c__Iterator;
	}

	public static void FadeInSpriteNow(GameObject obj)
	{
		SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
		Color color = component.color;
		component.color = new Color(color.r, color.g, color.b, 1f);
	}

	public static void FadeOutNow(Transform obj)
	{
		Material material = obj.GetComponent<MeshRenderer>().material;
		material.SetFloat("_alpha", 0f);
	}

	public static void FadeInNow(Transform obj)
	{
		Material material = obj.GetComponent<MeshRenderer>().material;
		material.SetFloat("_alpha", 1f);
	}

	public static IEnumerator FadeOutCanvas(GameObject obj, float duration)
	{
		EffectControl._FadeOutCanvas_c__IteratorA _FadeOutCanvas_c__IteratorA = new EffectControl._FadeOutCanvas_c__IteratorA();
		_FadeOutCanvas_c__IteratorA.obj = obj;
		_FadeOutCanvas_c__IteratorA.duration = duration;
		return _FadeOutCanvas_c__IteratorA;
	}

	public static void FadeOutCanvasNow(GameObject obj)
	{
		Graphic component = obj.GetComponent<Graphic>();
		Color color = component.color;
		component.color = new Color(color.r, color.g, color.b, 0f);
	}

	public static IEnumerator FadeInCanvas(GameObject obj, float duration)
	{
		EffectControl._FadeInCanvas_c__IteratorB _FadeInCanvas_c__IteratorB = new EffectControl._FadeInCanvas_c__IteratorB();
		_FadeInCanvas_c__IteratorB.obj = obj;
		_FadeInCanvas_c__IteratorB.duration = duration;
		return _FadeInCanvas_c__IteratorB;
	}

	public static IEnumerator FadeInCanvasTo(GameObject obj, float duration, float a)
	{
		EffectControl._FadeInCanvasTo_c__IteratorC _FadeInCanvasTo_c__IteratorC = new EffectControl._FadeInCanvasTo_c__IteratorC();
		_FadeInCanvasTo_c__IteratorC.obj = obj;
		_FadeInCanvasTo_c__IteratorC.duration = duration;
		_FadeInCanvasTo_c__IteratorC.a = a;
		return _FadeInCanvasTo_c__IteratorC;
	}

	public static void FadeInCanvasNow(GameObject obj)
	{
		Graphic component = obj.GetComponent<Graphic>();
		Color color = component.color;
		component.color = new Color(color.r, color.g, color.b, 1f);
	}

	public static IEnumerator DeActive(GameObject obj, float duration, EffectControl.Callback0 fn)
	{
		EffectControl._DeActive_c__IteratorD _DeActive_c__IteratorD = new EffectControl._DeActive_c__IteratorD();
		_DeActive_c__IteratorD.duration = duration;
		_DeActive_c__IteratorD.obj = obj;
		_DeActive_c__IteratorD.fn = fn;
		return _DeActive_c__IteratorD;
	}

	public static IEnumerator StartShakeGO(GameObject obj, float shakeRange, float duration)
	{
		EffectControl._StartShakeGO_c__IteratorE _StartShakeGO_c__IteratorE = new EffectControl._StartShakeGO_c__IteratorE();
		_StartShakeGO_c__IteratorE.obj = obj;
		_StartShakeGO_c__IteratorE.duration = duration;
		_StartShakeGO_c__IteratorE.shakeRange = shakeRange;
		return _StartShakeGO_c__IteratorE;
	}

	private static float RandomShakeRange(float range)
	{
		return UnityEngine.Random.Range(-Mathf.Abs(range), Mathf.Abs(range));
	}

	public static float quartEaseInOut(float time)
	{
		time *= 2f;
		if (time < 1f)
		{
			return 0.5f * time * time * time * time;
		}
		time -= 2f;
		return -0.5f * (time * time * time * time - 2f);
	}

	public static IEnumerator MoveLocalSpecial(GameObject obj, Vector3 to, float duration)
	{
		EffectControl._MoveLocalSpecial_c__IteratorF _MoveLocalSpecial_c__IteratorF = new EffectControl._MoveLocalSpecial_c__IteratorF();
		_MoveLocalSpecial_c__IteratorF.obj = obj;
		_MoveLocalSpecial_c__IteratorF.duration = duration;
		_MoveLocalSpecial_c__IteratorF.to = to;
		return _MoveLocalSpecial_c__IteratorF;
	}

	public static IEnumerator RotateLocalZ(GameObject obj, Vector3 to, float duration)
	{
		EffectControl._RotateLocalZ_c__Iterator10 _RotateLocalZ_c__Iterator = new EffectControl._RotateLocalZ_c__Iterator10();
		_RotateLocalZ_c__Iterator.obj = obj;
		_RotateLocalZ_c__Iterator.duration = duration;
		_RotateLocalZ_c__Iterator.to = to;
		return _RotateLocalZ_c__Iterator;
	}
}
