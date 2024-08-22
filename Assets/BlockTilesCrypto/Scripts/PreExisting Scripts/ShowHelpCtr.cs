using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowHelpCtr : MonoBehaviour
{
	public GameObject groundPrefab;

	public List<GameObject> listBlockDisable;

	public List<GameObject> listBlockActive;

	public void ShowHelp(List<CubeUnit> listBlock, Vector2 posValid)
	{
		int indexRow = listBlock[0].indexRow;
		int indexCol = listBlock[0].indexCol;
		for (int i = 0; i < listBlock.Count; i++)
		{
			GameObject block = this.GetBlock();
			Vector2 v = posValid + new Vector2((float)(listBlock[i].indexCol - indexCol), (float)(listBlock[i].indexRow - indexRow));
			block.transform.position = v;
			this.listBlockActive.Add(block);
		}
	}

	public GameObject GetBlock()
	{
		GameObject gameObject;
		if (this.listBlockDisable.Count == 0)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.groundPrefab);
		}
		else
		{
			gameObject = this.listBlockDisable[this.listBlockDisable.Count - 1];
			gameObject.SetActive(true);
			this.listBlockDisable.RemoveAt(this.listBlockDisable.Count - 1);
		}
		return gameObject;
	}

	public void SetBlock(GameObject block)
	{
		block.SetActive(false);
		this.listBlockDisable.Add(block);
	}

	public void HideAllBlock()
	{
		if (this.listBlockActive.Count > 0)
		{
			for (int i = 0; i < this.listBlockActive.Count; i++)
			{
				this.SetBlock(this.listBlockActive[i]);
			}
			this.listBlockActive.Clear();
		}
	}
}
