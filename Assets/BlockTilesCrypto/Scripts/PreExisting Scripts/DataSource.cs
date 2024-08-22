using System;
using UnityEngine;

public class DataSource : ScriptableObject
{
	[SerializeField]
	public UnitListColor[] listUnitColor;

	private const string InstancePath = "Data/Data";

	private static DataSource instance;

	public static DataSource Instance
	{
		get
		{
			if (DataSource.instance == null)
			{
				DataSource.instance = Resources.Load<DataSource>("Data/Data");
			}
			return DataSource.instance;
		}
	}
}
