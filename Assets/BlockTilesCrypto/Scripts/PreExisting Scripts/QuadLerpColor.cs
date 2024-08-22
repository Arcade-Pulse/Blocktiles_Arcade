using System;
using UnityEngine;

public class QuadLerpColor : MonoBehaviour
{
	public Material bgMat;

	private Color color0;

	private Color color1;

	private Color color23;

	private Color color67;

	private Color color4;

	private Color color5;

	private Color color8;

	private Color color9;

	private Color color10;

	private Color color11;

	public float f = 0.25f;

	public float f2 = 0.25f;

	private Mesh mesh;

	public void SetColor()
	{
		this.color0 = this.bgMat.GetColor("_Color0");
		this.color1 = this.bgMat.GetColor("_Color1");
		this.color23 = this.bgMat.GetColor("_Color23");
		this.color67 = this.bgMat.GetColor("_Color67");
		this.color4 = this.color1;
		this.color5 = this.color0;
		this.color8 = this.color0;
		this.color9 = this.color67;
		this.color10 = this.color67;
		this.color11 = this.color5;
		Color[] colors = new Color[]
		{
			this.color0,
			this.color1,
			this.color23,
			this.color23,
			this.color4,
			this.color5,
			this.color67,
			this.color67,
			this.color8,
			this.color9,
			this.color10,
			this.color11
		};
		this.mesh.colors = colors;
	}

	private void Awake()
	{
		base.gameObject.AddComponent<MeshFilter>();
		base.gameObject.AddComponent<MeshRenderer>();
		this.mesh = new Mesh();
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f),
			new Vector3(0.5f, this.f, 0f),
			new Vector3(0.5f, -this.f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(-0.5f, -this.f, 0f),
			new Vector3(-0.5f, this.f, 0f),
			new Vector3(0f, 0.5f, 0f),
			new Vector3(0f, this.f, 0f),
			new Vector3(0f, -this.f, 0f),
			new Vector3(0f, -0.5f, 0f)
		};
		this.mesh.vertices = vertices;
		this.mesh.triangles = new int[]
		{
			0,
			8,
			7,
			7,
			8,
			9,
			7,
			9,
			6,
			6,
			9,
			10,
			6,
			10,
			5,
			5,
			10,
			11,
			8,
			1,
			9,
			9,
			1,
			2,
			9,
			2,
			10,
			10,
			2,
			3,
			10,
			3,
			11,
			11,
			3,
			4
		};
		base.GetComponent<MeshRenderer>().material = this.bgMat;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
	}
}
