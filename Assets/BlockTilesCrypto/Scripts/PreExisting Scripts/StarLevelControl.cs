using System;
using UnityEngine;

public class StarLevelControl : MonoBehaviour
{
	public GameObject starObj;

	public void SetActive(bool isActive)
	{
		this.starObj.SetActive(isActive);
		if (isActive && !base.gameObject.activeSelf)
		{
			this.SetActiveObj(true);
		}
	}

	public void SetActiveObj(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}
}
