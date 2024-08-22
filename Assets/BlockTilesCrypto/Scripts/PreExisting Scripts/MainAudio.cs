using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainAudio : MonoBehaviour
{
	private sealed class _PlayLoopBgSound_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal MainAudio _this;

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

		public _PlayLoopBgSound_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(3f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				break;
			case 2u:
				break;
			default:
				return false;
			}
			this._this.PlaySound(TypeAudio.SoundBG);
			this._current = new WaitForSeconds((float)UnityEngine.Random.Range(14, 20));
			if (!this._disposing)
			{
				this._PC = 2;
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

	private static MainAudio main;

	public InputAudio[] listInputAudio;

	private Dictionary<TypeAudio, AudioSource> audioDict = new Dictionary<TypeAudio, AudioSource>();

	private Dictionary<TypeAudio, float> vollumeData = new Dictionary<TypeAudio, float>();

	public bool isSoundOn;

	public bool isSoundBgOn;

	private int bgIndex;

	public static MainAudio Main
	{
		get
		{
			return MainAudio.main;
		}
	}

	private void Awake()
	{
		this.AddComponienAudioSources();
		this.SetBGMusic();
		MainAudio.main = this;
	}

	private void SetBGMusic()
	{
		this.audioDict[TypeAudio.SoundBG].loop = true;
		this.audioDict[TypeAudio.SoundScore].loop = true;
		this.PlaySound(TypeAudio.SoundBG);
	}

	private IEnumerator PlayLoopBgSound()
	{
		MainAudio._PlayLoopBgSound_c__Iterator0 _PlayLoopBgSound_c__Iterator = new MainAudio._PlayLoopBgSound_c__Iterator0();
		_PlayLoopBgSound_c__Iterator._this = this;
		return _PlayLoopBgSound_c__Iterator;
	}

	private void AddComponienAudioSources()
	{
		for (int i = 0; i < this.listInputAudio.Length; i++)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.clip = this.listInputAudio[i].audioClip;
			audioSource.volume = this.listInputAudio[i].vollume;
			this.vollumeData.Add(this.listInputAudio[i].type, this.listInputAudio[i].vollume);
			this.audioDict.Add(this.listInputAudio[i].type, audioSource);
		}
	}

	public void PlaySound(TypeAudio type)
	{
		if (type == TypeAudio.SoundBG)
		{
			if (this.isSoundBgOn)
			{
				switch (UnityEngine.Random.Range(0, 6))
				{
				case 0:
					this.audioDict[type].time = 0f;
					break;
				case 1:
					this.audioDict[type].time = 5f;
					break;
				case 2:
					this.audioDict[type].time = 13f;
					break;
				case 3:
					this.audioDict[type].time = 19f;
					break;
				case 4:
					this.audioDict[type].time = 22f;
					break;
				case 5:
					this.audioDict[type].time = 28f;
					break;
				}
				this.audioDict[type].Play();
			}
		}
		else if (this.isSoundOn)
		{
			this.audioDict[type].Play();
		}
	}

	public void StopSound(TypeAudio type)
	{
		this.audioDict[type].Stop();
	}

	public void SoundOn()
	{
		this.isSoundOn = true;
		for (int i = 0; i < this.listInputAudio.Length; i++)
		{
			TypeAudio type = this.listInputAudio[i].type;
			if (type != TypeAudio.SoundBG)
			{
				this.audioDict[type].volume = this.vollumeData[type];
			}
		}
	}

	public void SoundOff()
	{
		this.isSoundOn = false;
		for (int i = 0; i < this.listInputAudio.Length; i++)
		{
			TypeAudio type = this.listInputAudio[i].type;
			if (type != TypeAudio.SoundBG)
			{
				this.audioDict[type].volume = 0f;
			}
		}
	}

	public void SoundBgOn()
	{
		this.isSoundBgOn = true;
		this.audioDict[TypeAudio.SoundBG].volume = this.vollumeData[TypeAudio.SoundBG];
	}

	public void SoundBgOff()
	{
		this.isSoundBgOn = false;
		this.audioDict[TypeAudio.SoundBG].volume = 0f;
	}
}
