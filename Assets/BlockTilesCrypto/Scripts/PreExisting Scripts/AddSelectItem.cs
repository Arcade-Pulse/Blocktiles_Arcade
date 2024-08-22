using System;
using UnityEngine;
using UnityEngine.UI;

public class AddSelectItem : MonoBehaviour
{
	public GameObject item;

	public int row;

	public int col;

	public float height;

	public GridLayoutGroup grid;

	public RectTransform rec;

	public ItemUnit[] listUnit;

	private int count;

	public void CreateTable()
	{
		int num = this.row * this.col;
		this.listUnit = new ItemUnit[num];
		for (int i = 0; i < this.col; i++)
		{
			for (int j = 0; j < this.row; j++)
			{
				this.AddItem();
			}
		}
	}

	private void AddItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.item);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = Vector3.one;
		this.listUnit[this.count] = gameObject.GetComponent<ItemUnit>();
		this.listUnit[this.count].index = this.count + 1;
		this.count++;
	}
}
