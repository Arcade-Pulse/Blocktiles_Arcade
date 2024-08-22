using System;
using System.Collections.Generic;
using UnityEngine;

public class NextViewerControl : MonoBehaviour
{
	public PattemTableObj pattemTableObj;

	public NextViewer[] listView;

	private global::Types type0;

	private global::Types type1;

	private global::Types type2;

	public List<CubeUnit> listUnit0;

	public List<CubeUnit> listUnit1;

	public List<CubeUnit> listUnit2;

	private bool isFirst = true;

	public void Preload()
	{
		this.listUnit0 = new List<CubeUnit>();
		this.listUnit1 = new List<CubeUnit>();
		this.listUnit2 = new List<CubeUnit>();
	}

	public void CheckUpdateNewPattem()
	{
		this.UpdatePattems();
		this.CheckImpossiblePattem();
	}

	private void UpdatePattems()
	{
		if (this.IsEmptyAll())
		{
			this.GetNewPattems();
		}
		else
		{
			this.CheckGameOver();
		}
	}

	private void CheckImpossiblePattem()
	{
		this.listView[0].CheckImpossible();
		this.listView[1].CheckImpossible();
		this.listView[2].CheckImpossible();
	}

	public void CheckHelp()
	{
		if (this.listView[0].state == NextViewer.State.Show && !this.listView[0].ValidMoreThanOne())
		{
			if ((this.listView[1].state == NextViewer.State.Hide || this.listView[1].state == NextViewer.State.Null) && (this.listView[2].state == NextViewer.State.Hide || this.listView[2].state == NextViewer.State.Null))
			{
				Vector2 posValid = this.listView[0].PosValid();
				MainObjControl.Instant.helpCtr.ShowHelp(this.listView[0].listBlock, posValid);
			}
		}
		else if (this.listView[1].state == NextViewer.State.Show && !this.listView[1].ValidMoreThanOne())
		{
			if ((this.listView[0].state == NextViewer.State.Hide || this.listView[0].state == NextViewer.State.Null) && (this.listView[2].state == NextViewer.State.Hide || this.listView[2].state == NextViewer.State.Null))
			{
				Vector2 posValid2 = this.listView[1].PosValid();
				MainObjControl.Instant.helpCtr.ShowHelp(this.listView[1].listBlock, posValid2);
			}
		}
		else if (this.listView[2].state == NextViewer.State.Show && !this.listView[2].ValidMoreThanOne())
		{
			if ((this.listView[1].state == NextViewer.State.Hide || this.listView[1].state == NextViewer.State.Null) && (this.listView[0].state == NextViewer.State.Hide || this.listView[0].state == NextViewer.State.Null))
			{
				Vector2 posValid3 = this.listView[2].PosValid();
				MainObjControl.Instant.helpCtr.ShowHelp(this.listView[2].listBlock, posValid3);
			}
		}
		else
		{
			MainObjControl.Instant.helpCtr.HideAllBlock();
		}
	}

	public void CheckGameOver()
	{
		if (this.IsGameOver())
		{
			MainCanvas.Main.lostScript.GameOver(true);
		}
	}

	private bool IsGameOver()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.listView[i].state != NextViewer.State.Null)
			{
				List<CubeUnit> listBlock = MainObjControl.Instant.nextViewerCtr.listView[i].listBlock;
				if (!MainObjControl.Instant.grid.InvalidPlacePattem(listBlock))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void GetNewPattems()
	{
		do
		{
			int scoreInt = MainCanvas.Main.inGameScript.scoreInt;
			this.SetAllBlockFromList(this.listUnit0);
			this.SetAllBlockFromList(this.listUnit1);
			this.SetAllBlockFromList(this.listUnit2);
			this.type0 = this.pattemTableObj.GetFixedRandomType(scoreInt);
			this.type1 = this.pattemTableObj.GetFixedRandomType(scoreInt);
			this.type2 = this.pattemTableObj.GetFixedRandomType(scoreInt);
			this.listUnit0 = MainObjControl.Instant.pattemCreater.CreatePattem(this.type0, this.listView[0].transform.position, this.listView[0].scale);
			this.listUnit1 = MainObjControl.Instant.pattemCreater.CreatePattem(this.type1, this.listView[1].transform.position, this.listView[1].scale);
			this.listUnit2 = MainObjControl.Instant.pattemCreater.CreatePattem(this.type2, this.listView[2].transform.position, this.listView[2].scale);
			this.listView[0].SetPattem(this.listUnit0, this.type0, UnityEngine.Random.Range(0, 4));
			this.listView[1].SetPattem(this.listUnit1, this.type1, UnityEngine.Random.Range(0, 4));
			this.listView[2].SetPattem(this.listUnit2, this.type2, UnityEngine.Random.Range(0, 4));
		}
		while (this.InvalidAllPattem() || this.NumberPattemO2(this.type0, this.type1, this.type2) > 2);
		this.listUnit0 = new List<CubeUnit>();
		this.listUnit1 = new List<CubeUnit>();
		this.listUnit2 = new List<CubeUnit>();
		this.listView[0].state = NextViewer.State.Show;
		this.listView[1].state = NextViewer.State.Show;
		this.listView[2].state = NextViewer.State.Show;
		this.CheckImpossiblePattem();
	}

	public void ReSetNewPattems()
	{
		this.SetAllBlock();
		this.GetNewPattems();
	}

	public void RotatePattem(List<CubeUnit> listUnitRotate)
	{
		int num = UnityEngine.Random.Range(0, 3);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < listUnitRotate.Count; j++)
			{
				listUnitRotate[j].RotateUnit();
			}
		}
	}

	private bool InvalidAllPattem()
	{
		bool flag = MainObjControl.Instant.grid.InvalidPlacePattem(this.listView[0].listBlock);
		bool flag2 = MainObjControl.Instant.grid.InvalidPlacePattem(this.listView[1].listBlock);
		bool flag3 = MainObjControl.Instant.grid.InvalidPlacePattem(this.listView[2].listBlock);
		return flag && flag2 && flag3;
	}

	private int NumberPattemO2(global::Types t0, global::Types t1, global::Types t2)
	{
		int num = 0;
		if (t0 == global::Types.O2)
		{
			num++;
		}
		if (t1 == global::Types.O2)
		{
			num++;
		}
		if (t2 == global::Types.O2)
		{
			num++;
		}
		return num;
	}

	public void SetAllBlockFromList(List<CubeUnit> listBlock)
	{
		for (int i = 0; i < listBlock.Count; i++)
		{
			MainObjControl.Instant.pattemCreater.SetCube(listBlock[i]);
		}
		listBlock = new List<CubeUnit>();
	}

	private bool IsEmptyAll()
	{
		for (int i = 0; i < this.listView.Length; i++)
		{
			if (this.listView[i].state != NextViewer.State.Null)
			{
				return false;
			}
		}
		return true;
	}

	public void SetAllBlock()
	{
		this.listView[0].SetAllBlock();
		this.listView[1].SetAllBlock();
		this.listView[2].SetAllBlock();
		this.listView[0].state = NextViewer.State.Null;
		this.listView[1].state = NextViewer.State.Null;
		this.listView[2].state = NextViewer.State.Null;
	}
}
