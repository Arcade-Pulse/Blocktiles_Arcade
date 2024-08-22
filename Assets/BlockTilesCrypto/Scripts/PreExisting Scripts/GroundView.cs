using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundView : MonoBehaviour
{
	public GameObject groundPrefab;

	public List<GameObject> listBlock;

	public ColorControl colorCtr;

	private List<bool> check;

	private Vector3 vecDown;

	private void Awake()
	{
		this.listBlock = new List<GameObject>();
		this.check = new List<bool>();
		this.vecDown = new Vector2(0f, -100f);
		for (int i = 0; i < 9; i++)
		{
			this.GetCube();
		}
		this.HideFromBlock(0);
	}

	public void SetPattem(List<CubeUnit> listUnit, int row, int col)
	{
		if (this.listBlock.Count < listUnit.Count)
		{
			do
			{
				this.GetCube();
			}
			while (this.listBlock.Count < listUnit.Count);
		}
		this.HideFromBlock(listUnit.Count);
		for (int i = 0; i < listUnit.Count; i++)
		{
			int num = row - (listUnit[0].indexRow - listUnit[i].indexRow);
			int num2 = col - (listUnit[0].indexCol - listUnit[i].indexCol);
			this.listBlock[i].transform.position = new Vector2((float)num2, (float)num);
			this.check[i] = true;
		}
	}

	private GameObject GetCube()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.groundPrefab);
		gameObject.GetComponent<SpriteRenderer>().sprite = this.colorCtr.GetSpriteBlockColor(0);
		this.listBlock.Add(gameObject);
		this.check.Add(true);
		return gameObject;
	}

	public void HideFromBlock(int fromIndex)
	{
		if (fromIndex < this.listBlock.Count)
		{
			for (int i = fromIndex; i < this.listBlock.Count; i++)
			{
				if (this.check[i])
				{
					this.listBlock[i].transform.position = this.vecDown;
					this.check[i] = false;
				}
			}
		}
	}
}
