using System;
using UnityEngine;

public class RotateZ : MonoBehaviour
{
	public float speed;

	public bool randomDirect;

	private int rotateRight = 1;

	private void OnEnable()
	{
		if (this.randomDirect)
		{
			if (UnityEngine.Random.value > 0.5f)
			{
				this.rotateRight = -1;
			}
			else
			{
				this.rotateRight = 1;
			}
		}
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, -this.speed * (float)this.rotateRight * Time.deltaTime), Space.Self);
	}
}
