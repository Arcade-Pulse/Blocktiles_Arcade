using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCtr : MonoBehaviour
{
	private sealed class _Step_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _i___1;

		internal int _i___2;

		internal ScreenCtr _this;

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

		public _Step_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.gridMoving = true;
				this._i___1 = 0;
				break;
			case 1u:
				this._i___1++;
				break;
			case 2u:
				this._i___2++;
				goto IL_13C;
			default:
				return false;
			}
			if (this._i___1 < 8)
			{
				for (int i = 0; i <= this._i___1; i++)
				{
					this._this.StartCoroutine(this._this.ScaleUpUnit(this._i___1 + 7 * i));
				}
				this._current = new WaitForSeconds(this._this.waitStep);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._i___2 = 0;
			IL_13C:
			if (this._i___2 < 7)
			{
				for (int j = 6 - this._i___2; j >= 0; j--)
				{
					this._this.StartCoroutine(this._this.ScaleUpUnit(15 + this._i___2 * 8 + 7 * j));
				}
				this._current = new WaitForSeconds(this._this.waitStep);
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			}
			this._this.gridMoving = false;
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

	private sealed class _ScaleUpUnit_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int index;

		internal GameObject _unit___0;

		internal float _timer___0;

		internal ScreenCtr _this;

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

		public _ScaleUpUnit_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._unit___0 = this._this.grid[this.index];
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationScaleUp)
			{
				this._timer___0 += Time.deltaTime;
				this._unit___0.transform.localScale = new Vector3(0.63f, 0.63f, 0.63f) * Mathf.LerpUnclamped(0f, 1f, this._this.acScale.Evaluate(this._timer___0 / this._this.durationScaleUp));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._unit___0.transform.localScale = new Vector3(0.63f,0.63f,0.63f);
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

	private sealed class _StartFade_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal GameObject obj;

		internal SpriteRenderer _render___0;

		internal Color _startColor___0;

		internal float _timer___0;

		internal float _t___1;

		internal Vector2 startPos;

		internal Vector2 targetPos;

		internal ScreenCtr _this;

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

		public _StartFade_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this.obj.SetActive(true);
				this._render___0 = this.obj.GetComponent<SpriteRenderer>();
				this._startColor___0 = Color.white;
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationFadeIn)
			{
				this._timer___0 += Time.deltaTime;
				this._t___1 = this._timer___0 / this._this.durationFadeIn;
				this.obj.transform.position = Vector2.Lerp(this.startPos, this.targetPos, this._t___1);
				this._render___0.color = new Color(this._startColor___0.r, this._startColor___0.g, this._startColor___0.b, Mathf.Lerp(this._this.startAlpha, 1f, this._t___1));
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._render___0.color = new Color(this._startColor___0.r, this._startColor___0.g, this._startColor___0.b, 1f);
			this.obj.transform.position = this.targetPos;
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

	public GameObject BgUnit;

	public RectTransform topRec;

	public Transform bottomTrans;

	public float defaultPattemY;

	public float defaultPattemUIY;

	public float orthographicSizeMin;

	public Camera cam;

	public float distanceEdge;

	public float durationFadeIn;

	public float distanceMoveFade;

	public float startAlpha;

	public float F;

	public float FUI;

	public GameObject BG;

	private int myR;

	private int myC;

	public GameObject[] grid;

	private float defaultScreen;

	private float currentScreen;

	public CanvasScaler cs;

	public Transform parentCell;

	public float waitStep;

	public bool gridMoving;

	public AnimationCurve acScale;

	public float durationScaleUp;

	public void Fix(int row, int col)
	{
		this.myR = row;
		this.myC = col;
		float num = (float)col + this.distanceEdge;
		this.FixMultiScreen();
		this.ScaleToFitBg();
	}

	private void FixMultiScreen()
	{
		this.defaultScreen = 1.77777779f;
		float num = 1.33333337f;
		float num2 = 2.33333325f;
		this.currentScreen = (float)Screen.height / (float)Screen.width;
		float t = (this.currentScreen - this.defaultScreen) / (num2 - this.defaultScreen);
		float t2 = (this.currentScreen - this.defaultScreen) / (num - this.defaultScreen);
		float a = 7.76f;
		float a2 = -0.6f;
		float a3 = 2.2f;
		if (this.currentScreen > this.defaultScreen + 0.05f)
		{
			float num3 = (float)this.myC + this.distanceEdge;
			this.cam.orthographicSize = Mathf.Max(this.orthographicSizeMin, num3 / (2f * this.cam.aspect));
			float b = 1.9f;
			this.cam.transform.position = new Vector3(3.5f, Mathf.LerpUnclamped(a3, b, t), -20f);
			this.BG.transform.localPosition = new Vector2(0f, this.cam.transform.position.y);
			float b2 = -1.17f;
			this.bottomTrans.position = new Vector2(0f, Mathf.LerpUnclamped(a2, b2, t));
		}
		else if (this.currentScreen < this.defaultScreen - 0.05f)
		{
			this.cs.matchWidthOrHeight = 1f;
			float b3 = 7.28f;
			this.cam.orthographicSize = Mathf.LerpUnclamped(a, b3, t2);
			float b4 = -0.24f;
			this.bottomTrans.position = new Vector2(0f, Mathf.LerpUnclamped(a2, b4, t2));
		}
	}

	private void CreateBG(int row, int col)
	{
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.BgUnit);
				gameObject.transform.position = new Vector2((float)j, (float)i);
				gameObject.transform.SetParent(this.parentCell);
				this.grid[i + j * row] = gameObject;
			}
		}
	}

	public void StartNoAnim()
	{
	}

	public void StartAnim()
	{
		for (int i = 0; i < 64; i++)
		{
			this.grid[i].transform.localScale = Vector3.zero;
		}
		MainAudio.Main.PlaySound(TypeAudio.SounReady);
		base.StartCoroutine(this.Step());
	}

	private IEnumerator Step()
	{
		ScreenCtr._Step_c__Iterator0 _Step_c__Iterator = new ScreenCtr._Step_c__Iterator0();
		_Step_c__Iterator._this = this;
		return _Step_c__Iterator;
	}

	private IEnumerator ScaleUpUnit(int index)
	{
		ScreenCtr._ScaleUpUnit_c__Iterator1 _ScaleUpUnit_c__Iterator = new ScreenCtr._ScaleUpUnit_c__Iterator1();
		_ScaleUpUnit_c__Iterator.index = index;
		_ScaleUpUnit_c__Iterator._this = this;
		return _ScaleUpUnit_c__Iterator;
	}

	private void ScaleToFitBg()
	{
		Vector3 size = this.BG.GetComponent<SpriteRenderer>().sprite.bounds.size;
		float x = size.x;
		float y = size.y;
		float num = this.cam.orthographicSize * 2f;
		float num2 = num / (float)Screen.height * (float)Screen.width;
		this.BG.transform.localScale = new Vector3(num2 / x, num / y, 1f);
	}

	private IEnumerator StartFade(GameObject obj, Vector2 startPos, Vector2 targetPos, bool enableGrid)
	{
		ScreenCtr._StartFade_c__Iterator2 _StartFade_c__Iterator = new ScreenCtr._StartFade_c__Iterator2();
		_StartFade_c__Iterator.obj = obj;
		_StartFade_c__Iterator.startPos = startPos;
		_StartFade_c__Iterator.targetPos = targetPos;
		_StartFade_c__Iterator._this = this;
		return _StartFade_c__Iterator;
	}
}
