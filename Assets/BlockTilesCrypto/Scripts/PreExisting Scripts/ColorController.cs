using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ColorController : MonoBehaviour
{
	private sealed class _StartGrayOn_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal ColorController _this;

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

		public _StartGrayOn_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				Shader.EnableKeyword("GRAYSCALE_ON");
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationGray)
			{
				this._timer___0 += Time.deltaTime;
				Shader.SetGlobalFloat("_GrayScaleFactor", this._timer___0 / this._this.durationGray * this._this.maxGray);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			Shader.SetGlobalFloat("_GrayScaleFactor", this._this.maxGray);
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

	private sealed class _StartGrayOff_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal ColorController _this;

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

		public _StartGrayOff_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				Shader.EnableKeyword("GRAYSCALE_ON");
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationGray)
			{
				this._timer___0 += Time.deltaTime;
				Shader.SetGlobalFloat("_GrayScaleFactor", this._this.maxGray * (1f - this._timer___0 / this._this.durationGray));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			Shader.SetGlobalFloat("_GrayScaleFactor", 0f);
			Shader.DisableKeyword("GRAYSCALE_ON");
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

	public float durationGray;

	public float maxGray;

	private int index;

	[SerializeField]
	public UnitListColor[] listColor;

	public void GrayOn()
	{
		base.StartCoroutine(this.StartGrayOn());
	}

	private IEnumerator StartGrayOn()
	{
		ColorController._StartGrayOn_c__Iterator0 _StartGrayOn_c__Iterator = new ColorController._StartGrayOn_c__Iterator0();
		_StartGrayOn_c__Iterator._this = this;
		return _StartGrayOn_c__Iterator;
	}

	public void GrayOffAnim()
	{
		base.StartCoroutine(this.StartGrayOff());
	}

	private IEnumerator StartGrayOff()
	{
		ColorController._StartGrayOff_c__Iterator1 _StartGrayOff_c__Iterator = new ColorController._StartGrayOff_c__Iterator1();
		_StartGrayOff_c__Iterator._this = this;
		return _StartGrayOff_c__Iterator;
	}

	public void GrayOff()
	{
		Shader.DisableKeyword("GRAYSCALE_ON");
		Shader.SetGlobalFloat("_GrayScaleFactor", 0f);
	}
}
