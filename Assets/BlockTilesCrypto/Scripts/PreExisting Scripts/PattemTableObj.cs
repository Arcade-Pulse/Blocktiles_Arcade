using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PattemData", menuName = "Create Pattem Data")]
public class PattemTableObj : ScriptableObject
{
	public PattemInfor[] listPattemsInfor;

	public LevelData[] levelData;

	public global::Types GetFixedRandomType(int score)
	{
		for (int i = 0; i < this.levelData.Length; i++)
		{
			if (score < this.levelData[i].Score)
			{
				return this.listPattemsInfor[this.RandomWeight(this.levelData[i].weight)].type;
			}
		}
		return this.listPattemsInfor[this.RandomWeight(this.levelData[0].weight)].type;
	}

	private int RandomWeight(int[] list)
	{
		int num = 0;
		for (int i = 0; i < list.Length; i++)
		{
			num += list[i];
		}
		int num2 = UnityEngine.Random.Range(0, num);
		int num3 = 0;
		for (int j = 0; j < list.Length; j++)
		{
			if (list[j] + num3 >= num2)
			{
				return j;
			}
			num3 += list[j];
		}
		return 0;
	}
}
