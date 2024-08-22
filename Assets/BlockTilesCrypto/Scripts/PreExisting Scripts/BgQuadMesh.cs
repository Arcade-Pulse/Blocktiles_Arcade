using System;
using UnityEngine;

public class BgQuadMesh : MonoBehaviour
{
	public Material bgMat;

	public Color color0;

	public Color color1;

	public float f = 0.2f;

	private void Start()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(0.5f, this.f, 0f),
			new Vector3(0.5f, -this.f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(-0.5f, -this.f, 0f),
			new Vector3(-0.5f, this.f, 0f)
		};
		mesh.vertices = vertices;
		mesh.triangles = new int[]
		{
			0,
			1,
			7,
			7,
			1,
			2,
			7,
			2,
			6,
			6,
			2,
			3,
			6,
			3,
			5,
			5,
			3,
			4
		};
		Color[] colors = new Color[]
		{
			this.color0,
			this.color0,
			this.color0,
			this.color1,
			this.color1,
			this.color1,
			this.color1,
			this.color0
		};
		mesh.colors = colors;
		base.GetComponent<MeshRenderer>().material = this.bgMat;
		base.GetComponent<MeshFilter>().mesh = mesh;
	}
}
