using System;
using UnityEngine;

public class BgScale : MonoBehaviour
{
	private void Start()
	{
		Camera main = Camera.main;
		Vector3 size = base.GetComponent<SpriteRenderer>().sprite.bounds.size;
		float num = 2f * main.orthographicSize;
		float x = num * main.aspect;
		base.transform.localScale = new Vector3(x, num * size.x / size.y, 1f);
	}
}
