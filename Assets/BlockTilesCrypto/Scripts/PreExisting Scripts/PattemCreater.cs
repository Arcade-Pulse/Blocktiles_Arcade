using System;
using System.Collections.Generic;
using UnityEngine;

public class PattemCreater : MonoBehaviour
{
	public ColorControl colorCtr;

	public GameObject cubePrefab;

	public List<CubeUnit> listCube;

	public PattemTableObj pattemDataSave;

	private PattemInfor[] listPattemsInfor;

	private Dictionary<global::Types, Vec2[]> dataInfor = new Dictionary<global::Types, Vec2[]>();

	private void Awake()
	{
		this.listPattemsInfor = this.pattemDataSave.listPattemsInfor;
		for (int i = 0; i < this.listPattemsInfor.Length; i++)
		{
			bool[] grid = this.listPattemsInfor[i].grid;
			List<Vec2> list = new List<Vec2>();
			for (int j = 0; j < grid.Length; j++)
			{
				if (grid[j])
				{
					list.Add(new Vec2(j % 5, j / 5));
				}
			}
			this.dataInfor.Add(this.listPattemsInfor[i].type, list.ToArray());
		}
	}

	public List<CubeUnit> CreatePattem(global::Types type, Vector2 pos, float scale)
	{
		return this.Create(type, pos, scale);
	}

	public CubeUnit CreateABlock(global::Types type, Vector2 pos, float scale)
	{
		CubeUnit cube = this.GetCube(type);
		cube.transform.localScale = Vector3.one * scale;
		cube.transform.position = pos;
		return cube;
	}

    public CubeUnit CreateABlock(Vector2 pos, float scale)
    {
        CubeUnit cube = this.GetCube();
        cube.transform.localScale = Vector3.one * scale;
        cube.transform.position = pos;
        return cube;
    }

    private List<CubeUnit> Create(global::Types thisType, Vector2 pos, float scale)
	{
		Vec2[] array = this.dataInfor[thisType];
		List<CubeUnit> list = new();
		for (int i = 0; i < array.Length; i++)
		{
			CubeUnit cube = this.GetCube(thisType);
			list.Add(cube);
			cube.thisType = thisType;
            // Debug.Log("enter 1 " + cube.thisType);
			this.colorCtr.SetCubeColor(cube);
            // cube.SetSprite(this.colorCtr.GetSpriteBlockColor(0), 1);
            cube.transform.localScale = new Vector3(scale, scale, scale);
			float x = pos.x + (float)array[i].R * scale;
			float y = pos.y + (float)array[i].C * scale;
			cube.transform.position = new Vector2(x, y);
			cube.scale = scale;
			cube.indexRow = array[i].C;
			cube.indexCol = array[i].R;
		}
		return list;
	}

	private CubeUnit GetCube(global::Types thisType)
	{
		CubeUnit cubeUnit;
		if (this.listCube.Count == 0)
		{
			cubeUnit = UnityEngine.Object.Instantiate<GameObject>(this.cubePrefab).GetComponent<CubeUnit>();
		}
		else
		{
			cubeUnit = this.listCube[^1];
			this.listCube.RemoveAt(this.listCube.Count - 1);
			cubeUnit.SetAlpha(GameDefine.pattemLightAlpha);
		}
        cubeUnit.thisType = thisType;
        // Debug.Log("enter 2 " + cubeUnit.thisType);
		this.colorCtr.SetCubeColor(cubeUnit);
        // cubeUnit.SetSprite(this.colorCtr.GetSpriteBlockColor(0), 1);
        cubeUnit.transform.eulerAngles = Vector3.zero;
		cubeUnit.SetLayer(0);
		return cubeUnit;
	}

    private CubeUnit GetCube()
    {
        CubeUnit cubeUnit;
        if (this.listCube.Count == 0)
        {
            cubeUnit = UnityEngine.Object.Instantiate<GameObject>(this.cubePrefab).GetComponent<CubeUnit>();
        }
        else
        {
            cubeUnit = this.listCube[this.listCube.Count - 1];
            this.listCube.RemoveAt(this.listCube.Count - 1);
            cubeUnit.SetAlpha(GameDefine.pattemLightAlpha);
        }
        this.colorCtr.SetCubeColor(cubeUnit);
        // cubeUnit.SetSprite(this.colorCtr.GetSpriteBlockColor(0), 1);
        cubeUnit.transform.eulerAngles = Vector3.zero;
        cubeUnit.SetLayer(0);
        return cubeUnit;
    }

    public void SetCube(CubeUnit cube)
	{
		this.listCube.Add(cube);
		cube.transform.position = new Vector2(0f, -100f);
	}

	public global::Types GetTypeFromString(string text)
	{
		for (int i = 0; i < this.listPattemsInfor.Length; i++)
		{
			if (this.listPattemsInfor[i].type.ToString().Equals(text))
			{
				return this.listPattemsInfor[i].type;
			}
		}
		return global::Types.O0;
	}
}
