using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NextViewer : MonoBehaviour
{
    public enum State
    {
        Null,
        Show,
        Hide
    }

    private sealed class _ScaleUpBlock_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float _timer___0;

        internal float _newScale___1;

        internal NextViewer _this;

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

        public _ScaleUpBlock_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._this.isScaling = true;
                    this._timer___0 = 0f;
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (this._timer___0 < this._this.duration)
            {
                this._timer___0 += Time.deltaTime;
                this._newScale___1 = Mathf.LerpUnclamped(0f, this._this.scale, this._this.ac.Evaluate(this._timer___0 / this._this.duration));
                this._this.ScaleBlock(this._newScale___1);
                this._this.SetBlockPos(this._newScale___1);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            this._this.SetBlockPos(this._this.scale);
            this._this.ScaleBlock(this._this.scale);
            this._this.isScaling = false;
            this._PC = -1;
            return false;
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

    private sealed class _StartSetLight_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float _timer___0;

        internal float fromAlpha;

        internal float toAlpha;

        internal NextViewer _this;

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

        public _StartSetLight_c__Iterator1()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._timer___0 = 0f;
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (this._timer___0 < this._this.durationLight)
            {
                this._timer___0 += Time.deltaTime;
                this._this.SetLight(Mathf.Lerp(this.fromAlpha, this.toAlpha, this._timer___0 / this._this.durationLight));
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            this._this.SetLight(this.toAlpha);
            this._PC = -1;
            return false;
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

    private sealed class _MoveHomeBlocks_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal List<CubeUnit> listFingerBlock;

        internal Vector2[] _listFingerBlockPos___0;

        internal float _durationGoHome___0;

        internal float _dragScale___0;

        internal float _timer___0;

        internal float _t___1;

        internal NextViewer _this;

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

        public _MoveHomeBlocks_c__Iterator2()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._this.isRunningHome = true;
                    this._listFingerBlockPos___0 = new Vector2[this.listFingerBlock.Count];
                    for (int i = 0; i < this._this.listBlock.Count; i++)
                    {
                        this._listFingerBlockPos___0[i] = this.listFingerBlock[i].transform.position;
                        this._this.listBlock[i].SetLayer(4);
                    }
                    this._durationGoHome___0 = Mathf.Min((this._listFingerBlockPos___0[0] - this._this.listBlockPos[0]).magnitude / this._this.speed, 0.37f);
                    this._dragScale___0 = this.listFingerBlock[0].transform.localScale.x;
                    this._timer___0 = 0f;
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (this._timer___0 < this._durationGoHome___0)
            {
                this._timer___0 += Time.deltaTime;
                this._t___1 = this._this.goHomeAc.Evaluate(this._timer___0 / this._durationGoHome___0);
                for (int j = 0; j < this._this.listBlock.Count; j++)
                {
                    this._this.listBlock[j].transform.position = Vector2.LerpUnclamped(this._listFingerBlockPos___0[j], this._this.listBlockPos[j], this._t___1);
                    this._this.listBlock[j].transform.localScale = Mathf.LerpUnclamped(this._dragScale___0, this._this.scale, this._t___1) * Vector3.one;
                }
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            for (int k = 0; k < this._this.listBlock.Count; k++)
            {
                this._this.listBlock[k].transform.position = this._this.listBlockPos[k];
            }
            this._this.ScaleBlock(this._this.scale);
            this._this.isRunningHome = false;
            this._PC = -1;
            return false;
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

    public static event EventHandler<OnSuccessfulDragEventArgs> OnCubeDragged;

    public class OnSuccessfulDragEventArgs : EventArgs
    {
        public bool isDragged;
        public global::Types type;
    }

    public int index;

    public float scale;

    public float durationLight;

    public float duration;

    public float durationIn;

    public float outScale;

    public global::Types myType;

    public List<CubeUnit> listBlock;

    private Vector2[] listBlockLocalPos;

    private List<Vector2> listBlockPos;

    private bool isScaling;

    private IEnumerator ScaleUpAnim;

    public NextViewer.State state;

    public int rotateTime;

    public AnimationCurve ac;

    public float speed;

    public AnimationCurve goHomeAc;

    private IEnumerator gohme;

    private bool isRunningHome;

    public void SetPattem(List<CubeUnit> listUnit, global::Types type, int numberRotate)
    {
        this.myType = type;
        this.listBlock = listUnit;
        this.RotatePattem(numberRotate);
        this.FixCenterPos();
        this.listBlockPos = new List<Vector2>();
        this.listBlockLocalPos = new Vector2[listUnit.Count];
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            this.listBlockPos.Add(this.listBlock[i].transform.position);
            this.listBlockLocalPos[i] = this.listBlockPos[i] - (Vector2)base.transform.position;
        }
        this.ScalePattem();
    }

    private void ScalePattem()
    {
        if (this.ScaleUpAnim != null && this.isScaling)
        {
            base.StopCoroutine(this.ScaleUpAnim);
            this.SetBlockPos(this.scale);
            this.ScaleBlock(this.scale);
        }
        this.ScaleUpAnim = this.ScaleUpBlock();
        base.StartCoroutine(this.ScaleUpAnim);
    }

    private IEnumerator ScaleUpBlock()
    {
        NextViewer._ScaleUpBlock_c__Iterator0 _ScaleUpBlock_c__Iterator = new NextViewer._ScaleUpBlock_c__Iterator0();
        _ScaleUpBlock_c__Iterator._this = this;
        return _ScaleUpBlock_c__Iterator;
    }

    private void SetBlockPos(float newScale)
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            float x = this.listBlockLocalPos[i].x * newScale / this.scale;
            float y = this.listBlockLocalPos[i].y * newScale / this.scale;
            this.listBlock[i].transform.position = new Vector2(x, y) + (Vector2)base.transform.position;
        }
    }

    private void ScaleBlock(float newScale)
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            this.listBlock[i].transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void RotatePattem(int numberRotate)
    {
        if (this.myType == global::Types.I3 || this.myType == global::Types.O1 || this.myType == global::Types.O0)
        {
            this.rotateTime = 0;
            return;
        }
        this.rotateTime = numberRotate;
        for (int i = 0; i < this.rotateTime; i++)
        {
            for (int j = 0; j < this.listBlock.Count; j++)
            {
                this.listBlock[j].RotateUnit();
            }
        }
    }

    public void RotatePattemHardTime(int newRotateTime)
    {
        this.rotateTime = newRotateTime;
        for (int i = 0; i < this.rotateTime; i++)
        {
            this.RotatePattemOne();
        }
    }

    public void RotatePattemOne()
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            this.listBlock[i].RotateUnit();
            this.listBlockPos[i] = this.listBlock[i].transform.position;
            this.listBlockLocalPos[i] = this.listBlockPos[i] - (Vector2)base.transform.position;
        }
    }

    public void FixCenterPos()
    {
        float num = 0f;
        float num2 = 0f;
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            num += (float)this.listBlock[i].indexCol;
            num2 += (float)this.listBlock[i].indexRow;
        }
        num /= (float)this.listBlock.Count;
        num2 /= (float)this.listBlock.Count;
        Vector2 center = new Vector2(num, num2);
        for (int j = 0; j < this.listBlock.Count; j++)
        {
            this.listBlock[j].MoveCenterPos(base.transform.position, center);
        }
    }

    private void OnMouseDown()
    {

        if (this.state == NextViewer.State.Show && MainState.GetState == MainState.State.Ingame)
        {
            if (this.ScaleUpAnim != null && this.isScaling)
            {
                base.StopCoroutine(this.ScaleUpAnim);
                this.SetBlockPos(this.scale);
                this.ScaleBlock(this.scale);
            }
            if (this.gohme != null && this.isRunningHome)
            {
                base.StopCoroutine(this.gohme);
                for (int i = 0; i < this.listBlock.Count; i++)
                {
                    this.listBlock[i].transform.position = this.listBlockPos[i];
                }
                this.ScaleBlock(this.scale);
            }
            OnCubeDragged?.Invoke(this, new OnSuccessfulDragEventArgs { isDragged = true, type = this.listBlock[0].thisType});
            // UnityEngine.Debug.Log("On Drag" + this.listBlock[0].thisType);
            MainObjControl.Instant.planeViewCrt.SetPattem(this.listBlock, base.transform.position, this.index, this.scale);
            this.HideAllBlock();
            if (MainState.typePlay == MainState.TypePlay.Tutorial)
            {
                MainObjControl.Instant.tutorial.StopFinger();
            }
        }
    }

    public void CheckImpossible()
    {
        if (this.state != NextViewer.State.Null)
        {
            if (!MainObjControl.Instant.grid.InvalidPlacePattem(this.listBlock))
            {
                if (this.state == NextViewer.State.Hide)
                {
                    base.StartCoroutine(this.StartSetLight(GameDefine.pattemDarkAlpha, GameDefine.pattemLightAlpha));
                    this.state = NextViewer.State.Show;
                }
            }
            else if (this.state == NextViewer.State.Show)
            {
                base.StartCoroutine(this.StartSetLight(GameDefine.pattemLightAlpha, GameDefine.pattemDarkAlpha));
                this.state = NextViewer.State.Hide;
            }
        }
    }

    public bool ValidMoreThanOne()
    {
        return MainObjControl.Instant.grid.ValidPlaceMoreThanOne(this.listBlock);
    }

    public Vector2 PosValid()
    {
        return MainObjControl.Instant.grid.PosValidPattem(this.listBlock);
    }

    private IEnumerator StartSetLight(float fromAlpha, float toAlpha)
    {
        NextViewer._StartSetLight_c__Iterator1 _StartSetLight_c__Iterator = new NextViewer._StartSetLight_c__Iterator1();
        _StartSetLight_c__Iterator.fromAlpha = fromAlpha;
        _StartSetLight_c__Iterator.toAlpha = toAlpha;
        _StartSetLight_c__Iterator._this = this;
        return _StartSetLight_c__Iterator;
    }

    private void SetLight(float alpha)
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            this.listBlock[i].SetAlpha(alpha);
        }
    }

    public void HideAllBlock()
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            this.listBlock[i].transform.position = new Vector2(0f, -100f);
        }
    }

    public void ShowAllBlock(List<CubeUnit> listFingerBlock)
    {
        this.gohme = this.MoveHomeBlocks(listFingerBlock);
        base.StartCoroutine(this.gohme);
    }

    private IEnumerator MoveHomeBlocks(List<CubeUnit> listFingerBlock)
    {
        NextViewer._MoveHomeBlocks_c__Iterator2 _MoveHomeBlocks_c__Iterator = new NextViewer._MoveHomeBlocks_c__Iterator2();
        _MoveHomeBlocks_c__Iterator.listFingerBlock = listFingerBlock;
        _MoveHomeBlocks_c__Iterator._this = this;
        return _MoveHomeBlocks_c__Iterator;
    }

    public void SetAllBlock()
    {
        for (int i = 0; i < this.listBlock.Count; i++)
        {
            MainObjControl.Instant.pattemCreater.SetCube(this.listBlock[i]);
        }
        this.listBlock = new List<CubeUnit>();
    }
}
