using System;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
	public void Reset(bool isActive)
	{
		this.SetActive(isActive);
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}
}
