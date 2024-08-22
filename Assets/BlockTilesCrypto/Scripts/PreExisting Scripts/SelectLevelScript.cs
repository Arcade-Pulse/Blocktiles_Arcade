using System;
using UnityEngine;

public class SelectLevelScript : MonoBehaviour
{
	public AddSelectItem addScript;

	public ItemUnit[] listUnit;

	public bool isSelected;

	public void Preload()
	{
		this.addScript.CreateTable();
		this.listUnit = this.addScript.listUnit;
	}

	public void ReSet()
	{
		this.isSelected = false;
		this.ResetContentTable();
		this.SetActive(false);
	}

	private void ResetContentTable()
	{
		int getMaxLevel = Settings.GetMaxLevel;
		for (int i = 0; i < this.listUnit.Length; i++)
		{
			if (i <= getMaxLevel - 1)
			{
				this.listUnit[i].SetOpen(getMaxLevel);
				this.listUnit[i].enabled = true;
			}
			else
			{
				this.listUnit[i].SetLock();
				this.listUnit[i].enabled = false;
			}
		}
	}

	public void SetActive(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}
}
