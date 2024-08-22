using System;
using UnityEngine;

public class gridOverlay : MonoBehaviour
{
	public bool show = true;

	public int gridSizeX;

	public int gridSizeY;

	public int gridSizeZ;

	public float step;

	public float startX;

	public float startY;

	public float startZ;

	private Material lineMaterial;

	public Color mainColor = new Color(0f, 1f, 0f, 1f);

	private void CreateLineMaterial()
	{
		if (!this.lineMaterial)
		{
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			this.lineMaterial = new Material(shader);
			this.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			this.lineMaterial.SetInt("_SrcBlend", 5);
			this.lineMaterial.SetInt("_DstBlend", 10);
			this.lineMaterial.SetInt("_Cull", 0);
			this.lineMaterial.SetInt("_ZWrite", 0);
		}
	}

	private void OnPostRender()
	{
		this.CreateLineMaterial();
		this.lineMaterial.SetPass(0);
		GL.Begin(1);
		if (this.step == 0f)
		{
			UnityEngine.Debug.Log("Step is 0, wont draw Grid. Step it to higher than 0.");
		}
		if (this.show)
		{
			GL.Color(this.mainColor);
			for (float num = 0f; num <= (float)this.gridSizeY; num += this.step)
			{
				for (float num2 = 0f; num2 <= (float)this.gridSizeZ; num2 += this.step)
				{
					GL.Vertex3(this.startX, this.startY + num, this.startZ + num2);
					GL.Vertex3(this.startX + (float)this.gridSizeX, this.startY + num, this.startZ + num2);
				}
				for (float num3 = 0f; num3 <= (float)this.gridSizeX; num3 += this.step)
				{
					GL.Vertex3(this.startX + num3, this.startY + num, this.startZ);
					GL.Vertex3(this.startX + num3, this.startY + num, this.startZ + (float)this.gridSizeZ);
				}
			}
			for (float num4 = 0f; num4 <= (float)this.gridSizeZ; num4 += this.step)
			{
				for (float num5 = 0f; num5 <= (float)this.gridSizeX; num5 += this.step)
				{
					GL.Vertex3(this.startX + num5, this.startY, this.startZ + num4);
					GL.Vertex3(this.startX + num5, this.startY + (float)this.gridSizeY, this.startZ + num4);
				}
			}
		}
		GL.End();
	}
}
