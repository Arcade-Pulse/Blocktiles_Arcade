using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
	private sealed class _StartAuto_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal NextViewer _next___1;

		internal Vector2[] _listBlockLocalPos___2;

		internal Vector2 _facePos___2;

		internal Vec2 _place___2;

		internal float _timer___2;

		internal Vector2 _startPos___2;

		internal Vector2 _correctPos___2;

		internal Vector2 _targetPos___2;

		internal float _duration___2;

		internal AutoPlay _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _StartAuto_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				if (this._this.nextViewerCtr.listView[0].state == NextViewer.State.Show && !this._this.grid.InvalidPlacePattem(this._this.nextViewerCtr.listView[0].listBlock))
				{
					this._next___1 = this._this.nextViewerCtr.listView[0];
					this._this.planeView.selected = 0;
					Vector2 vector = this._this.nextViewerCtr.listView[0].transform.position;
					this._this.planeView.faceMousePos = new Vector2(vector.x, vector.y + this._this.planeView.distanceTouch);
				}
				else if (this._this.nextViewerCtr.listView[1].state == NextViewer.State.Show && !this._this.grid.InvalidPlacePattem(this._this.nextViewerCtr.listView[1].listBlock))
				{
					this._next___1 = this._this.nextViewerCtr.listView[1];
					this._this.planeView.selected = 1;
					Vector2 vector2 = this._this.nextViewerCtr.listView[1].transform.position;
					this._this.planeView.faceMousePos = new Vector2(vector2.x, vector2.y + this._this.planeView.distanceTouch);
				}
				else
				{
					this._next___1 = this._this.nextViewerCtr.listView[2];
					this._this.planeView.selected = 2;
					Vector2 vector3 = this._this.nextViewerCtr.listView[2].transform.position;
					this._this.planeView.faceMousePos = new Vector2(vector3.x, vector3.y + this._this.planeView.distanceTouch);
				}
				MainObjControl.Instant.planeViewCrt.SetPattem(this._next___1.listBlock, this._next___1.gameObject.transform.position, this._next___1.index, this._next___1.scale);
				this._listBlockLocalPos___2 = new Vector2[this._next___1.listBlock.Count];
				for (int i = 0; i < this._next___1.listBlock.Count; i++)
				{
					this._listBlockLocalPos___2[i] = (this._next___1.listBlock[i].transform.position - this._next___1.transform.position) / this._next___1.scale;
				}
				this._next___1.HideAllBlock();
				this._facePos___2 = this._this.nextViewerCtr.listView[this._this.planeView.selected].transform.position;
				this._this.planeView.faceMousePos = new Vector2(this._facePos___2.x, this._facePos___2.y + this._this.planeView.distanceTouch);
				this._place___2 = this._this.grid.GetPlacePattem(this._next___1.listBlock);
				this._current = new WaitForSeconds(0.4f);
				if (!this._disposing)
				{
					this._PC = 2;
				}
				return true;
			case 2u:
				this._timer___2 = 0f;
				this._startPos___2 = this._this.planeView.faceMousePos;
				this._correctPos___2 = this._listBlockLocalPos___2[0];
				this._targetPos___2 = new Vector2((float)this._place___2.C - this._correctPos___2.x, (float)this._place___2.R - this._correctPos___2.y);
				this._duration___2 = 0.7f;
				goto IL_4F9;
			case 3u:
				goto IL_4F9;
			case 4u:
				if (MainState.state == MainState.State.GameOver)
				{
					UnityEngine.Debug.Log("Wait game over");
					this._current = new WaitForSeconds(8.5f);
					if (!this._disposing)
					{
						this._PC = 5;
					}
					return true;
				}
				break;
			case 5u:
				MainCanvas.Main.lostScript.TryAgainButton();
				this._current = new WaitForSeconds(2f);
				if (!this._disposing)
				{
					this._PC = 6;
				}
				return true;
			case 6u:
				break;
			default:
				return false;
			}
			IL_35:
			this._current = new WaitForSeconds(1f);
			if (!this._disposing)
			{
				this._PC = 1;
			}
			return true;
			IL_4F9:
			if (this._timer___2 >= this._duration___2)
			{
				this._this.planeView.faceMousePos = this._targetPos___2;
				this._this.planeView.PlacePattemGround();
				this._current = new WaitForSeconds(2.2f);
				if (!this._disposing)
				{
					this._PC = 4;
				}
				return true;
			}
			this._timer___2 += Time.deltaTime;
			this._this.planeView.faceMousePos = Vector2.Lerp(this._startPos___2, this._targetPos___2, this._timer___2 / this._duration___2);
			this._current = null;
			if (!this._disposing)
			{
				this._PC = 3;
			}
			return true;
			goto IL_35; // wont work because it came after return
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private Grid grid;

	private NextViewerControl nextViewerCtr;

	private PlaneView planeView;

	public int timeScale;

	private void Start()
	{
		Time.timeScale = (float)this.timeScale;
		this.grid = MainObjControl.Instant.grid;
		this.nextViewerCtr = MainObjControl.Instant.nextViewerCtr;
		this.planeView = MainObjControl.Instant.planeViewCrt;
		this.planeView.isAuto = true;
		MainCanvas.Main.lostScript.isAuto = true;
		base.StartCoroutine(this.StartAuto());
	}

	private IEnumerator StartAuto()
	{
		AutoPlay._StartAuto_c__Iterator0 _StartAuto_c__Iterator = new AutoPlay._StartAuto_c__Iterator0();
		_StartAuto_c__Iterator._this = this;
		return _StartAuto_c__Iterator;
	}
}
