using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
	public LineRenderer line;

	private List<Vector3> listPoint;

	public float minX;

	public float maxX;

	public float minY;

	public float maxY;

	private void Start()
	{
		this.listPoint = new List<Vector3>();
		float num = this.minY + 1f;
		for (int i = 0; i < (int)this.maxY; i++)
		{
			Vector3 newPos = new Vector3(this.minX, num, 0f);
			Vector3 newPos2 = new Vector3(this.maxX, num, 0f);
			if (i % 2 == 0)
			{
				this.AddPoint(newPos);
				this.AddPoint(newPos2);
			}
			else
			{
				this.AddPoint(newPos2);
				this.AddPoint(newPos);
			}
			num += 1f;
		}
		this.Apply();
	}

	private void AddPoint(Vector3 newPos)
	{
		this.listPoint.Add(newPos);
	}

	private void Apply()
	{
		this.line.SetVertexCount(this.listPoint.Count);
		this.line.SetPositions(this.solution(this.listPoint.ToArray()));
	}

	private Vector3[] solution(Vector3[] original)
	{
		Vector3[] array = new Vector3[original.Length * 3 - 2];
		for (int i = 0; i < array.Length; i++)
		{
			if (i % 3 == 0)
			{
				array[i] = original[i / 3];
			}
			else if (i % 3 == 1)
			{
				array[i] = Vector3.Lerp(original[(i - 1) / 3], original[(i + 2) / 3], 0.0001f);
			}
			else if (i % 3 == 2)
			{
				array[i] = Vector3.Lerp(original[(i + 1) / 3], original[(i - 2) / 3], 0.0001f);
			}
		}
		return array;
	}
}
