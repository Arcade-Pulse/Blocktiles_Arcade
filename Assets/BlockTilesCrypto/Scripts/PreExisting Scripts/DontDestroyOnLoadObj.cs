using System;
using UnityEngine;

public class DontDestroyOnLoadObj : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}
}
