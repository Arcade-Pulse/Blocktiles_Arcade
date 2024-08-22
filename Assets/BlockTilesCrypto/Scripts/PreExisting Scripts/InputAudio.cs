using System;
using UnityEngine;

[Serializable]
public struct InputAudio
{
	[SerializeField]
	public TypeAudio type;

	[SerializeField]
	public AudioClip audioClip;

	[SerializeField]
	public float vollume;
}
