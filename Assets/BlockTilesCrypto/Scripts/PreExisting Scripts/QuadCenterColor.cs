using System;
using UnityEngine;

public class QuadCenterColor : MonoBehaviour
{
	public Material bgMat;

	private Color color0;

	private Color color1;

	private Color color23;

	private Color color67;

	private Color color4;

	private Color color5;

	public float f = 0.2f;

	private Mesh mesh;

	public void SetColor()
	{
		this.color0 = this.bgMat.GetColor("_Color0");
		this.color1 = this.bgMat.GetColor("_Color1");
		this.color23 = this.bgMat.GetColor("_Color23");
		this.color67 = this.bgMat.GetColor("_Color67");
		this.color4 = this.color1;
		this.color5 = this.color0;
		Color[] colors = new Color[]
		{
			this.color0,
			this.color1,
			this.color23,
			this.color23,
			this.color4,
			this.color5,
			this.color67,
			this.color67
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
			new Vector3(-0.5f, this.f, 0f)
		};
		this.mesh.vertices = vertices;
		this.mesh.triangles = new int[]
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
		base.GetComponent<MeshRenderer>().material = this.bgMat;
		base.GetComponent<MeshFilter>().mesh = this.mesh;
	}
}
