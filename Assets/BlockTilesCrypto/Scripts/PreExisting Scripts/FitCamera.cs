using System;
using UnityEngine;

public class FitCamera : MonoBehaviour
{
	public bool setFitX;

	public bool setFitY;

	private void Awake()
	{
		Vector3 size = base.GetComponent<SpriteRenderer>().sprite.bounds.size;
		float x = size.x;
		float y = size.y;
		float num = Camera.main.orthographicSize * 2f;
		float num2 = num / (float)Screen.height * (float)Screen.width;
		float x2 = (!this.setFitX) ? base.transform.localScale.x : (num2 / x);
		float y2 = (!this.setFitY) ? base.transform.localScale.y : (num / y);
		base.transform.localScale = new Vector3(x2, y2, 1f);
	}
}
