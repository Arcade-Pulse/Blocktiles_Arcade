using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FaderControl : MonoBehaviour
{
	public delegate void Callback0();

	private sealed class _StartFade_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal Color _color___0;

		internal FaderControl.Callback0 fn;

		internal FaderControl _this;

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

		public _StartFade_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.SetActive(true);
				this._timer___0 = 0f;
				this._color___0 = this._this.image.color;
				this._this.image.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 0f);
				break;
			case 1u:
				break;
			case 2u:
				goto IL_211;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationFade)
			{
				this._timer___0 += Time.unscaledDeltaTime;
				this._this.image.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, Mathf.Lerp(0f, 1f, this._timer___0 / this._this.durationFade));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.image.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 1f);
			if (this.fn != null)
			{
				this.fn();
			}
			this._timer___0 = 0f;
			IL_211:
			if (this._timer___0 <= this._this.durationFade)
			{
				this._timer___0 += Time.unscaledDeltaTime;
				this._this.image.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, Mathf.Lerp(1f, 0f, this._timer___0 / this._this.durationFade));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.image.color = new Color(this._color___0.r, this._color___0.g, this._color___0.b, 0f);
			this._this.SetActive(false);
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

	private sealed class _StartSwichPanel_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group1;

		internal CanvasGroup group2;

		internal FaderControl.Callback0 fn;

		internal float _t___1;

		internal FaderControl _this;

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

		public _StartSwichPanel_c__Iterator1()
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
				this.group1.alpha = 1f;
				this.group2.alpha = 0f;
				this.group2.gameObject.SetActive(true);
				if (this.fn != null)
				{
					this.fn();
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group1.alpha = 1f - this._t___1;
				this.group2.alpha = this._t___1;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.group1.gameObject.SetActive(false);
			this.group1.alpha = 1f;
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

	private sealed class _StartSwichPanelFix_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group1;

		internal CanvasGroup group2;

		internal float _t___1;

		internal FaderControl.Callback0 fn;

		internal FaderControl _this;

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

		public _StartSwichPanelFix_c__Iterator2()
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
				this.group1.alpha = 1f;
				this.group2.alpha = 0f;
				this.group2.gameObject.SetActive(true);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group1.alpha = 1f - this._t___1;
				this.group2.alpha = this._t___1;
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
			this.group1.gameObject.SetActive(false);
			this.group1.alpha = 1f;
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

	private sealed class _StartFadeInPanel_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group;

		internal FaderControl.Callback0 fn;

		internal float _t___1;

		internal FaderControl _this;

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

		public _StartFadeInPanel_c__Iterator3()
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
				this.group.alpha = 0f;
				this.group.gameObject.SetActive(true);
				if (this.fn != null)
				{
					this.fn();
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group.alpha = this._t___1;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.group.alpha = 1f;
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

	private sealed class _StartFadeInPanelFix_c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group;

		internal float _t___1;

		internal FaderControl.Callback0 fn;

		internal FaderControl _this;

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

		public _StartFadeInPanelFix_c__Iterator4()
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
				this.group.alpha = 0f;
				this.group.gameObject.SetActive(true);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group.alpha = this._t___1;
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
			this.group.alpha = 1f;
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

	private sealed class _StartFadeOutPanel_c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group;

		internal FaderControl.Callback0 fn;

		internal float _t___1;

		internal FaderControl _this;

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

		public _StartFadeOutPanel_c__Iterator5()
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
				this.group.alpha = 1f;
				if (this.fn != null)
				{
					this.fn();
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group.alpha = 1f - this._t___1;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.group.gameObject.SetActive(false);
			this.group.alpha = 1f;
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

	private sealed class _StartFadeOutPanelFix_c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal CanvasGroup group;

		internal float _t___1;

		internal FaderControl.Callback0 fn;

		internal FaderControl _this;

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

		public _StartFadeOutPanelFix_c__Iterator6()
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
				this.group.alpha = 1f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 <= this._this.durationSwich)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationSwich;
				this.group.alpha = 1f - this._t___1;
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
			this.group.gameObject.SetActive(false);
			this.group.alpha = 1f;
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

	public Image image;

	public float durationFade;

	public float durationFadeOut;

	public float durationSwich;

	public void Reset()
	{
		this.SetActive(false);
	}

	public void Fade(FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartFade(fn));
	}

	private void SetActive(bool isActive)
	{
		this.image.gameObject.SetActive(isActive);
	}

	private IEnumerator StartFade(FaderControl.Callback0 fn)
	{
		FaderControl._StartFade_c__Iterator0 _StartFade_c__Iterator = new FaderControl._StartFade_c__Iterator0();
		_StartFade_c__Iterator.fn = fn;
		_StartFade_c__Iterator._this = this;
		return _StartFade_c__Iterator;
	}

	public void SwichPanel(CanvasGroup group1, CanvasGroup group2, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartSwichPanel(group1, group2, fn));
	}

	private IEnumerator StartSwichPanel(CanvasGroup group1, CanvasGroup group2, FaderControl.Callback0 fn)
	{
		FaderControl._StartSwichPanel_c__Iterator1 _StartSwichPanel_c__Iterator = new FaderControl._StartSwichPanel_c__Iterator1();
		_StartSwichPanel_c__Iterator.group1 = group1;
		_StartSwichPanel_c__Iterator.group2 = group2;
		_StartSwichPanel_c__Iterator.fn = fn;
		_StartSwichPanel_c__Iterator._this = this;
		return _StartSwichPanel_c__Iterator;
	}

	public void SwichPanelFix(CanvasGroup group1, CanvasGroup group2, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartSwichPanelFix(group1, group2, fn));
	}

	private IEnumerator StartSwichPanelFix(CanvasGroup group1, CanvasGroup group2, FaderControl.Callback0 fn)
	{
		FaderControl._StartSwichPanelFix_c__Iterator2 _StartSwichPanelFix_c__Iterator = new FaderControl._StartSwichPanelFix_c__Iterator2();
		_StartSwichPanelFix_c__Iterator.group1 = group1;
		_StartSwichPanelFix_c__Iterator.group2 = group2;
		_StartSwichPanelFix_c__Iterator.fn = fn;
		_StartSwichPanelFix_c__Iterator._this = this;
		return _StartSwichPanelFix_c__Iterator;
	}

	public void FadeInPanel(CanvasGroup group, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartFadeInPanel(group, fn));
	}

	private IEnumerator StartFadeInPanel(CanvasGroup group, FaderControl.Callback0 fn)
	{
		FaderControl._StartFadeInPanel_c__Iterator3 _StartFadeInPanel_c__Iterator = new FaderControl._StartFadeInPanel_c__Iterator3();
		_StartFadeInPanel_c__Iterator.group = group;
		_StartFadeInPanel_c__Iterator.fn = fn;
		_StartFadeInPanel_c__Iterator._this = this;
		return _StartFadeInPanel_c__Iterator;
	}

	public void FadeInPanelFix(CanvasGroup group, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartFadeInPanelFix(group, fn));
	}

	private IEnumerator StartFadeInPanelFix(CanvasGroup group, FaderControl.Callback0 fn)
	{
		FaderControl._StartFadeInPanelFix_c__Iterator4 _StartFadeInPanelFix_c__Iterator = new FaderControl._StartFadeInPanelFix_c__Iterator4();
		_StartFadeInPanelFix_c__Iterator.group = group;
		_StartFadeInPanelFix_c__Iterator.fn = fn;
		_StartFadeInPanelFix_c__Iterator._this = this;
		return _StartFadeInPanelFix_c__Iterator;
	}

	public void FadeOutPanel(CanvasGroup group, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartFadeOutPanel(group, fn));
	}

	private IEnumerator StartFadeOutPanel(CanvasGroup group, FaderControl.Callback0 fn)
	{
		FaderControl._StartFadeOutPanel_c__Iterator5 _StartFadeOutPanel_c__Iterator = new FaderControl._StartFadeOutPanel_c__Iterator5();
		_StartFadeOutPanel_c__Iterator.group = group;
		_StartFadeOutPanel_c__Iterator.fn = fn;
		_StartFadeOutPanel_c__Iterator._this = this;
		return _StartFadeOutPanel_c__Iterator;
	}

	public void FadeOutPanelFix(CanvasGroup group, FaderControl.Callback0 fn)
	{
		base.StartCoroutine(this.StartFadeOutPanelFix(group, fn));
	}

	private IEnumerator StartFadeOutPanelFix(CanvasGroup group, FaderControl.Callback0 fn)
	{
		FaderControl._StartFadeOutPanelFix_c__Iterator6 _StartFadeOutPanelFix_c__Iterator = new FaderControl._StartFadeOutPanelFix_c__Iterator6();
		_StartFadeOutPanelFix_c__Iterator.group = group;
		_StartFadeOutPanelFix_c__Iterator.fn = fn;
		_StartFadeOutPanelFix_c__Iterator._this = this;
		return _StartFadeOutPanelFix_c__Iterator;
	}
}
