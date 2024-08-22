using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectCtr : MonoBehaviour
{
	private sealed class _WaitDisable_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ParticleSystem particle;

		internal EffectCtr _this;

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

		public _WaitDisable_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this._this.durationDeactive);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.SetParticle(this.particle);
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

	public GameObject particlePrefab;

	public List<ParticleSystem> listParticlesDisable;

	public float durationDeactive;

	public int preload;

	public void Preload()
	{
		this.CreateFirst();
	}

	private void CreateFirst()
	{
		ParticleSystem[] array = new ParticleSystem[this.preload];
		for (int i = 0; i < this.preload; i++)
		{
			array[i] = this.GetParticle();
		}
		for (int j = 0; j < this.preload; j++)
		{
			this.SetParticle(array[j]);
		}
	}

	public void Effect(Vector2 pos)
	{
		ParticleSystem particle = this.GetParticle();
		particle.transform.position = pos;
		particle.Play();
		base.StartCoroutine(this.WaitDisable(particle));
	}

	private IEnumerator WaitDisable(ParticleSystem particle)
	{
		EffectCtr._WaitDisable_c__Iterator0 _WaitDisable_c__Iterator = new EffectCtr._WaitDisable_c__Iterator0();
		_WaitDisable_c__Iterator.particle = particle;
		_WaitDisable_c__Iterator._this = this;
		return _WaitDisable_c__Iterator;
	}

	public ParticleSystem GetParticle()
	{
		ParticleSystem particleSystem;
		if (this.listParticlesDisable.Count == 0)
		{
			particleSystem = UnityEngine.Object.Instantiate<GameObject>(this.particlePrefab).GetComponent<ParticleSystem>();
		}
		else
		{
			particleSystem = this.listParticlesDisable[this.listParticlesDisable.Count - 1];
			this.listParticlesDisable.RemoveAt(this.listParticlesDisable.Count - 1);
			particleSystem.gameObject.SetActive(true);
		}
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
		return particleSystem;
	}

	public void SetParticle(ParticleSystem particle)
	{
		this.listParticlesDisable.Add(particle);
		particle.gameObject.SetActive(false);
	}
}
