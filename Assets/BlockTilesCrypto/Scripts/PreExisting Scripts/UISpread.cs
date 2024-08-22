using System;
using UnityEngine;
using UnityEngine.UI;

public class UISpread : MonoBehaviour
{
	public float Duration = 0.6f;

	private float t;

	public float delayStart;

	public float fromScale;

	public float toScale;

	public float fromOpacity;

	public float toOpacity;

	public bool repeating;

	private Graphic image;

	private bool enableAnimating = true;

	private Vector3 frSc;

	private Vector3 toSc;

	private bool starting;

	public bool DoneAnimate
	{
		get
		{
			return this.t >= this.Duration;
		}
	}

	public bool EnableAnimating
	{
		get
		{
			return this.enableAnimating;
		}
		set
		{
			this.enableAnimating = value;
		}
	}

	public void StopAction()
	{
		this.enableAnimating = false;
	}

	public void Reset()
	{
		this.enableAnimating = true;
		this.Start();
	}

	private void Start()
	{
		if (this.enableAnimating)
		{
			this.t = 0f;
			this.starting = false;
			this.image = base.GetComponent<Graphic>();
			this.SetOpacity(this.fromOpacity);
			this.frSc = Vector3.one * this.fromScale;
			this.toSc = Vector3.one * this.toScale;
			base.transform.localScale = this.frSc;
		}
	}

	private void Update()
	{
		if (!this.enableAnimating)
		{
			return;
		}
		if (!this.starting)
		{
			this.t += Time.deltaTime;
			if (this.t >= this.delayStart)
			{
				this.t = 0f;
				this.starting = true;
			}
		}
		else if (this.starting && this.t < this.Duration)
		{
			this.t += Time.deltaTime;
			base.transform.localScale = Vector3.Lerp(this.frSc, this.toSc, this.t / this.Duration);
			this.SetOpacity(Mathf.Lerp(this.fromOpacity, this.toOpacity, this.t / this.Duration));
			if (this.t >= this.Duration && this.repeating)
			{
				this.t = 0f;
				Vector3 vector = this.frSc;
				this.frSc = this.toSc;
				this.toSc = vector;
				float num = this.fromOpacity;
				this.fromOpacity = this.toOpacity;
				this.toOpacity = num;
			}
		}
	}

	private void SetOpacity(float opacity)
	{
		Color color = this.image.color;
		color.a = opacity;
		this.image.color = color;
	}
}
