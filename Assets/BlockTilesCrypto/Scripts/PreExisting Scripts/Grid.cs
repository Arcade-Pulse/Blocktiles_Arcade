using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public struct fillData
	{
		public int cubeRow;

		public int cubeCol;
	}

	private sealed class _StartGrayScale_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal List<Vec2> _vecs___0;

		internal int _i___1;

		internal int _rand___2;

		internal bool isGrayIn;

		internal Grid _this;

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

		public _StartGrayScale_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._vecs___0 = new List<Vec2>();
				for (int i = 0; i < this._this.numberRow; i++)
				{
					for (int j = 0; j < this._this.numberCol; j++)
					{
						this._vecs___0.Add(new Vec2(i, j));
					}
				}
				goto IL_2A1;
			case 1u:
				//IL_261:
				this._vecs___0.RemoveAt(this._rand___2);
				if (this._vecs___0.Count == 0)
				{
					goto IL_2A1;
				}
				this._i___1++;
				break;
			default:
				return false;
			}
			IL_295:
			if (this._i___1 < 3)
			{
				this._rand___2 = UnityEngine.Random.Range(0, this._vecs___0.Count);
				if (this._this.grid[this._vecs___0[this._rand___2].R, this._vecs___0[this._rand___2].C] != null)
				{
					if (this.isGrayIn)
					{
						float randGray = this._this.GetRandGray();
						this._this.grid[this._vecs___0[this._rand___2].R, this._vecs___0[this._rand___2].C].targetGray = randGray;
						this._this.StartCoroutine(this._this.GrayBlock(this._this.grid[this._vecs___0[this._rand___2].R, this._vecs___0[this._rand___2].C], randGray, this.isGrayIn));
					}
					else
					{
						this._this.StartCoroutine(this._this.GrayBlock(this._this.grid[this._vecs___0[this._rand___2].R, this._vecs___0[this._rand___2].C], this._this.grid[this._vecs___0[this._rand___2].R, this._vecs___0[this._rand___2].C].targetGray, this.isGrayIn));
					}
					this._current = new WaitForSeconds(this._this.waitUnit);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				goto IL_261;
			}
			IL_2A1:
			if (this._vecs___0.Count > 0)
			{
				this._i___1 = 0;
				goto IL_295;
			}
			this._PC = -1;

            IL_261:
            this._vecs___0.RemoveAt(this._rand___2);
            if (this._vecs___0.Count == 0)
            {
                goto IL_2A1;
            }
            this._i___1++;

            return false;
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

	private sealed class _GrayBlock_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal bool isGrayIn;

		internal float targetGray;

		internal Sprite _sprite___1;

		internal CubeUnit unit;

		internal Grid _this;

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

		public _GrayBlock_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this._this.durationGray)
			{
				this._timer___0 += Time.deltaTime;
				if (this.isGrayIn)
				{
					this._sprite___1 = this._this.colorCtr.GetSpriteGray(this._timer___0 * this.targetGray / this._this.durationGray);
				}
				else
				{
					this._sprite___1 = this._this.colorCtr.GetSpriteGray(this.targetGray * Mathf.Max(1f - this._timer___0 / this._this.durationGray, 0f));
				}
				this.unit.SetSprite(this._sprite___1, 10);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			if (!this.isGrayIn)
			{
                UnityEngine.Debug.Log(this.unit.thisType);
                this._this.colorCtr.SetCubeColor(this.unit);
				// this.unit.SetSprite(this._this.colorCtr.GetSpriteBlockColor(0), 1);
			}
			this._PC = -1;
			return false;
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

	private sealed class _DeleteList_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float timeWait;

		internal List<CubeUnit> newCubeUnit;

		internal Grid _this;

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

		public _DeleteList_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this._this.waitDelete + this.timeWait);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				for (int i = 0; i < this.newCubeUnit.Count; i++)
				{
					this.newCubeUnit[i].Effect();
				}
				this._PC = -1;
				break;
			}
			return false;
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

	private sealed class _DeleteListEffect_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _i___1;

		internal List<CubeUnit> listBlockDelete;

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

		public _DeleteListEffect_c__Iterator3()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._i___1 = 0;
				break;
			case 1u:
				this._i___1++;
				break;
			default:
				return false;
			}
			if (this._i___1 < this.listBlockDelete.Count)
			{
				this.listBlockDelete[this._i___1].Effect();
				this._current = new WaitForSeconds(0.03f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._PC = -1;
			return false;
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

	public int numberCol;

	public int numberRow;

	public CubeUnit[,] grid;

	public Vector2[,] gridPos;

	public float waitDelete;

	private PattemCreater pattemCreater;

	public float durationGray;

	public float nonGray;

	public float halfGray;

	public float fullGray;

	public float waitUnit;

	public List<Grid.fillData> listFill;

	private ColorControl colorCtr;

	public void Preload()
	{
		this.grid = new CubeUnit[this.numberRow, this.numberCol];
		this.listFill = new List<Grid.fillData>();
		this.pattemCreater = MainObjControl.Instant.pattemCreater;
		this.colorCtr = MainObjControl.Instant.colorControl;
	}

	public void GrayScaleMap(bool isGrayIn)
	{
		base.StartCoroutine(this.StartGrayScale(isGrayIn));
	}

	private IEnumerator StartGrayScale(bool isGrayIn)
	{
		Grid._StartGrayScale_c__Iterator0 _StartGrayScale_c__Iterator = new Grid._StartGrayScale_c__Iterator0();
		_StartGrayScale_c__Iterator.isGrayIn = isGrayIn;
		_StartGrayScale_c__Iterator._this = this;
		return _StartGrayScale_c__Iterator;
	}

	private IEnumerator GrayBlock(CubeUnit unit, float targetGray, bool isGrayIn)
	{
		Grid._GrayBlock_c__Iterator1 _GrayBlock_c__Iterator = new Grid._GrayBlock_c__Iterator1();
		_GrayBlock_c__Iterator.isGrayIn = isGrayIn;
		_GrayBlock_c__Iterator.targetGray = targetGray;
		_GrayBlock_c__Iterator.unit = unit;
		_GrayBlock_c__Iterator._this = this;
		return _GrayBlock_c__Iterator;
	}

	private float GetRandGray()
	{
		float value = UnityEngine.Random.value;
		if (value > 0.3f)
		{
			return this.fullGray;
		}
		if (value > 0.1f)
		{
			return this.halfGray;
		}
		return this.nonGray;
	}

	public bool InvalidPlacePattem(List<CubeUnit> listUnit)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (!this.InvalidPoint(listUnit, i, j))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool ValidPlaceMoreThanOne(List<CubeUnit> listUnit)
	{
		int num = 0;
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (!this.InvalidPoint(listUnit, i, j))
				{
					num++;
					if (num >= 2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public Vector2 PosValidPattem(List<CubeUnit> listUnit)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (!this.InvalidPoint(listUnit, i, j))
				{
					return new Vector2((float)j, (float)i);
				}
			}
		}
		return Vector2.zero;
	}

	public bool InvalidPoint(List<CubeUnit> listUnit, int row, int col)
	{
		int indexRow = listUnit[0].indexRow;
		int indexCol = listUnit[0].indexCol;
		for (int i = 0; i < listUnit.Count; i++)
		{
			int num = row - (indexRow - listUnit[i].indexRow);
			int num2 = col - (indexCol - listUnit[i].indexCol);
			if (num < 0 || num >= this.numberRow || num2 < 0 || num2 >= this.numberCol)
			{
				return true;
			}
			if (this.grid[num, num2] != null)
			{
				return true;
			}
		}
		return false;
	}

	public void CheckGridFillTest(List<CubeUnit> listUnit, int mainRow, int mainCol)
	{
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.isColFullTest(i, listUnit, mainRow, mainCol))
			{
				this.ChangeSpriteFillCol(i);
				for (int j = 0; j < listUnit.Count; j++)
				{
					int num = mainCol - (listUnit[0].indexCol - listUnit[j].indexCol);
					if (num == i)
					{
                        // UnityEngine.Debug.Log("Enter 5 " + listUnit[j].thisType);
                        this.colorCtr.SetCubeHighlight(listUnit[j]);
                        // listUnit[j].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
					}
				}
			}
		}
		for (int k = 0; k < this.numberRow; k++)
		{
			if (this.isRowFullTest(k, listUnit, mainRow, mainCol))
			{
				this.ChangeSpriteFillRow(k);
				for (int l = 0; l < listUnit.Count; l++)
				{
					int num2 = mainRow - (listUnit[0].indexRow - listUnit[l].indexRow);
					if (num2 == k)
					{
                        // UnityEngine.Debug.Log("Enter 4 " + listUnit[l].thisType);
                        this.colorCtr.SetCubeHighlight(listUnit[l]);
                        // listUnit[l].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
					}
				}
			}
		}
	}

	public bool isRowFullTest(int row, List<CubeUnit> listUnit, int mainRow, int mainCol)
	{
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.grid[row, i] == null && !this.IsInListTest(row, i, listUnit, mainRow, mainCol))
			{
				return false;
			}
		}
		return true;
	}

	public bool isColFullTest(int col, List<CubeUnit> listUnit, int mainRow, int mainCol)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			if (this.grid[i, col] == null && !this.IsInListTest(i, col, listUnit, mainRow, mainCol))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsRowFillWith(int r, int c)
	{
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.grid[r, i] == null && i != c)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsColFillWith(int r, int c)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			if (this.grid[i, c] == null && i != r)
			{
				return false;
			}
		}
		return true;
	}

	public void ChangeSpriteFillCol(int col)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			if (this.grid[i, col] != null)
			{
				Grid.fillData item = default(Grid.fillData);
				item.cubeRow = this.grid[i, col].row;
				item.cubeCol = this.grid[i, col].col;
				this.listFill.Add(item);
                // UnityEngine.Debug.Log("Enter 6 " + this.grid[i, col].thisType);
                this.colorCtr.SetCubeHighlight(this.grid[i, col]);
				// this.grid[i, col].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
			}
		}
	}

	public void ChangeSpriteFillRow(int row)
	{
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.grid[row, i] != null)
			{
				Grid.fillData item = default(Grid.fillData);
				item.cubeRow = this.grid[row, i].row;
				item.cubeCol = this.grid[row, i].col;
				this.listFill.Add(item);
                // UnityEngine.Debug.Log("Enter 7 " + this.grid[row, i].thisType);
                this.colorCtr.SetCubeHighlight(this.grid[row, i]);
				// this.grid[row, i].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
			}
		}
	}

	public void CheckGrid(List<CubeUnit> newCubeUnit, float timeWait, ref bool isCollect)
	{
		int num = 0;
		List<Vector2> list = new List<Vector2>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.isColFull(i, newCubeUnit))
			{
				for (int j = 0; j < newCubeUnit.Count; j++)
				{
					if (newCubeUnit[j].col == i)
					{
						list.Add(newCubeUnit[j].GetCorrectPos());
					}
				}
				num++;
				for (int k = 0; k < newCubeUnit.Count; k++)
				{
					if (newCubeUnit[k].col == i)
					{
                        // UnityEngine.Debug.Log("Enter 8 " + newCubeUnit[k].thisType);
                        this.colorCtr.SetCubeHighlight(newCubeUnit[k]);
						// newCubeUnit[k].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
					}
				}
				this.deleteCol(i, timeWait);
				list2.Add(i);
			}
		}
		for (int l = 0; l < this.numberRow; l++)
		{
			if (this.isRowFull(l, newCubeUnit, list2))
			{
				for (int m = 0; m < newCubeUnit.Count; m++)
				{
					if (newCubeUnit[m].row == l)
					{
						list.Add(newCubeUnit[m].GetCorrectPos());
					}
				}
				num++;
				for (int n = 0; n < newCubeUnit.Count; n++)
				{
					if (newCubeUnit[n].row == l)
					{
                        // UnityEngine.Debug.Log("Enter 9 " + newCubeUnit[n].thisType);
                        this.colorCtr.SetCubeHighlight(newCubeUnit[n]);
						// newCubeUnit[n].SetSprite(this.colorCtr.GetSpriteBlockHighlightColor(0), 0);
					}
				}
				this.deleteRow(l, timeWait);
			}
		}

		int score = GameDefine.GetScore(num);
		if (score > 0)
		{
			isCollect = true;
			this.ClearListFill();
			MainObjControl.Instant.scoreCtr.ShowText(this.CenterOfList(list), score, num);
		}
		MainCanvas.Main.inGameScript.SetNewScore(score, newCubeUnit.Count);
	}

	private Vector2 CenterOfList(List<Vector2> listFill)
	{
		Vector2 a = Vector2.zero;
		for (int i = 0; i < listFill.Count; i++)
		{
			a += listFill[i];
		}
		return a / (float)listFill.Count;
	}

	public bool isRowFull(int row, List<CubeUnit> newCubeUnit, List<int> listColDeleted)
	{
		if (listColDeleted.Count == 0)
		{
			for (int i = 0; i < this.numberCol; i++)
			{
				if (this.grid[row, i] == null && !this.IsInList(row, i, newCubeUnit))
				{
					return false;
				}
			}
		}
		else
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (this.grid[row, j] == null && !this.IsInList(row, j, newCubeUnit) && !this.IsInListInt(j, listColDeleted))
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool isColFull(int col, List<CubeUnit> newCubeUnit)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			if (this.grid[i, col] == null && !this.IsInList(i, col, newCubeUnit))
			{
				return false;
			}
		}
		return true;
	}

	public void TurnOffAllFillLine()
	{
		if (this.listFill.Count > 0)
		{
			for (int i = 0; i < this.listFill.Count; i++)
			{
                var cube = this.grid[this.listFill[i].cubeRow, this.listFill[i].cubeCol];
                // UnityEngine.Debug.Log(cube.thisType);
                this.colorCtr.SetCubeColor(cube);
				// this.grid[this.listFill[i].cubeRow, this.listFill[i].cubeCol].SetSprite(this.colorCtr.GetSpriteBlockColor(0), 1);
			}
			this.ClearListFill();
		}
	}

	public void ClearListFill()
	{
		this.listFill.Clear();
	}

	private bool IsInListTest(int r, int c, List<CubeUnit> listUnit, int mainRow, int mainCol)
	{
		for (int i = 0; i < listUnit.Count; i++)
		{
			if (mainRow - (listUnit[0].indexRow - listUnit[i].indexRow) == r && mainCol - (listUnit[0].indexCol - listUnit[i].indexCol) == c)
			{
				return true;
			}
		}
		return false;
	}

	private void deleteRow(int r, float timeWait)
	{
		List<CubeUnit> list = new List<CubeUnit>();
		for (int i = 0; i < this.numberCol; i++)
		{
			if (this.grid[r, i] != null)
			{
				list.Add(this.grid[r, i]);
				this.grid[r, i] = null;
			}
		}
		base.StartCoroutine(this.DeleteList(list, timeWait));
	}

	private void deleteCol(int c, float timeWait)
	{
		List<CubeUnit> list = new List<CubeUnit>();
		for (int i = 0; i < this.numberRow; i++)
		{
			if (this.grid[i, c] != null)
			{
				list.Add(this.grid[i, c]);
				this.grid[i, c] = null;
			}
		}
		base.StartCoroutine(this.DeleteList(list, timeWait));
	}

	private IEnumerator DeleteList(List<CubeUnit> newCubeUnit, float timeWait)
	{
		Grid._DeleteList_c__Iterator2 _DeleteList_c__Iterator = new Grid._DeleteList_c__Iterator2();
		_DeleteList_c__Iterator.timeWait = timeWait;
		_DeleteList_c__Iterator.newCubeUnit = newCubeUnit;
		_DeleteList_c__Iterator._this = this;
		return _DeleteList_c__Iterator;
	}

	private bool IsInList(int r, int c, List<CubeUnit> newCubeUnit)
	{
		for (int i = 0; i < newCubeUnit.Count; i++)
		{
			if (newCubeUnit[i].row == r && newCubeUnit[i].col == c)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInListInt(int value, List<int> listInt)
	{
		for (int i = 0; i < listInt.Count; i++)
		{
			if (value == listInt[i])
			{
				return true;
			}
		}
		return false;
	}

	private CubeUnit GetOneInCol(int c, List<CubeUnit> newCubeUnit)
	{
		for (int i = 0; i < newCubeUnit.Count; i++)
		{
			if (newCubeUnit[i].col == c)
			{
				return newCubeUnit[i];
			}
		}
		return newCubeUnit[0];
	}

	private CubeUnit GetOneInRow(int r, List<CubeUnit> newCubeUnit)
	{
		for (int i = 0; i < newCubeUnit.Count; i++)
		{
			if (newCubeUnit[i].row == r)
			{
				return newCubeUnit[i];
			}
		}
		return newCubeUnit[0];
	}

	public int insideBorder(CubeUnit cubeUnit)
	{
		if (cubeUnit.col < 0)
		{
			return 1;
		}
		if (cubeUnit.col >= this.numberCol)
		{
			return 2;
		}
		if (cubeUnit.row < 0)
		{
			return 3;
		}
		return 0;
	}

	public void DeleteSomeBlock()
	{
		List<CubeUnit> list = new List<CubeUnit>();
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (this.grid[i, j] != null)
				{
					list.Add(this.grid[i, j]);
				}
			}
		}
		int num = Mathf.FloorToInt((float)list.Count * 0.7f);
		List<CubeUnit> list2 = new List<CubeUnit>();
		for (int k = 0; k < num; k++)
		{
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				list2.Add(list[index]);
				this.grid[list[index].row, list[index].col] = null;
				list.RemoveAt(index);
			}
		}
		base.StartCoroutine(this.DeleteListEffect(list2));
	}

	private IEnumerator DeleteListEffect(List<CubeUnit> listBlockDelete)
	{
		Grid._DeleteListEffect_c__Iterator3 _DeleteListEffect_c__Iterator = new Grid._DeleteListEffect_c__Iterator3();
		_DeleteListEffect_c__Iterator.listBlockDelete = listBlockDelete;
		return _DeleteListEffect_c__Iterator;
	}

	public void SetAllBlock()
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (this.grid[i, j] != null)
				{
					this.pattemCreater.SetCube(this.grid[i, j]);
					this.grid[i, j] = null;
				}
			}
		}
	}

	public Vec2 GetPlacePattem(List<CubeUnit> listUnit)
	{
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				if (!this.InvalidPoint(listUnit, i, j))
				{
					return new Vec2(i, j);
				}
			}
		}
		return new Vec2(0, 0);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && (MainState.state == MainState.State.Ingame || MainState.state == MainState.State.Pause))
		{
			string text = string.Empty;
			for (int i = 0; i < this.numberRow; i++)
			{
				for (int j = 0; j < this.numberCol; j++)
				{
					if (this.grid[i, j] != null)
					{
						text += "1";
					}
					else
					{
						text += "0";
					}
					text += "-";
				}
			}
			text += "+";
			text += MainCanvas.Main.inGameScript.scoreInt.ToString();
			if (MainObjControl.Instant.nextViewerCtr.listView[0].state == NextViewer.State.Null && MainObjControl.Instant.nextViewerCtr.listView[1].state == NextViewer.State.Null && MainObjControl.Instant.nextViewerCtr.listView[2].state == NextViewer.State.Null)
			{
				UnityEngine.Debug.Log("dont save");
				return;
			}
			for (int k = 0; k < 3; k++)
			{
				text += "+";
				if (MainObjControl.Instant.nextViewerCtr.listView[k].state != NextViewer.State.Null)
				{
					text += MainObjControl.Instant.nextViewerCtr.listView[k].myType.ToString();
					text += "-";
					text += MainObjControl.Instant.nextViewerCtr.listView[k].rotateTime.ToString();
				}
			}
			Settings.SetContinueData(text);
			Settings.SetContinue(1);
		}
	}

	public void LoadDataSave()
	{
		string getContinueData = Settings.GetContinueData;
		Settings.SetContinue(0);
		if (getContinueData == string.Empty)
		{
			return;
		}
		Settings.SetContinueData(string.Empty);
		string[] array = getContinueData.Split(new char[]
		{
			'+'
		});
		string[] array2 = array[0].Split(new char[]
		{
			'-'
		});
		int num = 0;
		for (int i = 0; i < this.numberRow; i++)
		{
			for (int j = 0; j < this.numberCol; j++)
			{
				int num2 = int.Parse(array2[num]);
				if (num2 == 1)
				{
					this.grid[i, j] = this.pattemCreater.CreateABlock(new Vector2((float)j, (float)i), 1f);
//                    UnityEngine.Debug.Log(this.grid[i, j].thisType);
                    this.colorCtr.SetCubeColor(this.grid[i, j]);
					// this.grid[i, j].SetSprite(this.colorCtr.GetSpriteBlockColor(0), 1);
					this.grid[i, j].row = i;
					this.grid[i, j].col = j;
				}
				num++;
			}
		}
		MainCanvas.Main.inGameScript.SetScoreContinue(int.Parse(array[1]));
		for (int k = 0; k < 3; k++)
		{
			if (array[2 + k] != string.Empty)
			{
				string[] array3 = array[2 + k].Split(new char[]
				{
					'-'
				});
				NextViewer nextViewer = MainObjControl.Instant.nextViewerCtr.listView[k];
				global::Types typeFromString = this.pattemCreater.GetTypeFromString(array3[0]);
				List<CubeUnit> listUnit = MainObjControl.Instant.pattemCreater.CreatePattem(typeFromString, nextViewer.transform.position, nextViewer.scale);
				nextViewer.state = NextViewer.State.Show;
				nextViewer.SetPattem(listUnit, typeFromString, int.Parse(array3[1]));
				nextViewer.FixCenterPos();
				nextViewer.CheckImpossible();
			}
		}
	}
}
