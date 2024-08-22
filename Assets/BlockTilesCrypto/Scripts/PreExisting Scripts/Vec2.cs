using System;
using UnityEngine;

public class Vec2
{
	public int R;

	public int C;

	private static int r;

	private static int c;

	public Vec2()
	{
		this.R = 0;
		this.C = 0;
	}

	public Vec2(int r, int c)
	{
		this.R = r;
		this.C = c;
	}

	public Vec2(Vector2 vec)
	{
		this.R = (int)vec.y;
		this.C = (int)vec.x;
	}

	public Vec2(Vec2 vec)
	{
		this.R = vec.R;
		this.C = vec.C;
	}

	public static int FastDistance(Vec2 v1, Vec2 v2)
	{
		Vec2.r = Mathf.Abs(v1.R - v2.R);
		Vec2.c = Mathf.Abs(v1.C - v2.C);
		if (Vec2.r > Vec2.c)
		{
			return Vec2.r;
		}
		return Vec2.c;
	}

	public string Print()
	{
		return string.Concat(new object[]
		{
			"(",
			this.R,
			",",
			this.C,
			")"
		});
	}
}
