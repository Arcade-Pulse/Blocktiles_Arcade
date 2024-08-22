using System;
using UnityEngine;

public class MainObjControl : MonoBehaviour
{
	private static MainObjControl main;

	public ColorControl colorControl;

	public PattemCreater pattemCreater;

	public NextViewerControl nextViewerCtr;

	public Grid grid;

	public PlaneView planeViewCrt;

	public GroundView groundView;

	public CameraScript camScript;

	public ScoreCtr scoreCtr;

	public ShowHelpCtr helpCtr;

	public ScreenCtr screenCtr;

	public Tutorial tutorial;

	public CameraScript cam;

	public static MainObjControl Instant
	{
		get
		{
			return MainObjControl.main;
		}
	}

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Time.timeScale = 1f;
		this.screenCtr.Fix(this.grid.numberRow, this.grid.numberCol);
		MainObjControl.main = this;
	}

	public void Preload()
	{
		this.grid.Preload();
		this.nextViewerCtr.Preload();
		this.tutorial.Preload();
		this.scoreCtr.Preload();
	}
}
