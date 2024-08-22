using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public enum State
    {
        Step0,
        Step1,
        Step2,
        Step3
    }

    private sealed class _WaitOpenMap2_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Tutorial _this;

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

        public _WaitOpenMap2_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current = new WaitForSeconds(1.4f);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    this._this.OpenMap2();
                    this._PC = -1;
                    break;
            }
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

    private sealed class _WaitOpenMap3_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Tutorial _this;

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

        public _WaitOpenMap3_c__Iterator1()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current = new WaitForSeconds(1.4f);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    this._this.OpenMap3();
                    this._PC = -1;
                    break;
            }
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

    private sealed class _StartMoveFinger_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector2 startPos;

        internal Vector2 _preStart___0;

        internal float _timer___1;

        internal Vector2 targetPos;

        internal Tutorial _this;

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

        public _StartMoveFinger_c__Iterator2()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._preStart___0 = this.startPos + new Vector2(0.5f, -0.5f);
                    break;
                case 1u:
                    //IL_DB:
                    if (this._timer___1 >= this._this.durationMoveShort)
                    {
                        this._this.finger.transform.position = this.startPos;
                        this._current = new WaitForSeconds(0.1f);
                        if (!this._disposing)
                        {
                            this._PC = 2;
                        }
                        return true;
                    }
                    this._timer___1 += Time.deltaTime;
                    this._this.finger.transform.position = Vector2.Lerp(this._preStart___0, this.startPos, this._timer___1 / this._this.durationMoveShort);
                    this._current = null;
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 2u:
                    this._timer___1 = 0f;
                    goto IL_1C3;
                case 3u:
                    goto IL_1C3;
                case 4u:
                    this._timer___1 = 0f;
                    goto IL_297;
                case 5u:
                    goto IL_297;
                case 6u:
                    goto IL_361;
                case 7u:
                    this._timer___1 = 0f;
                    goto IL_41B;
                case 8u:
                    goto IL_41B;
                case 9u:
                    break;
                default:
                    return false;
            }
            this._timer___1 = 0f;
            goto IL_DB;
        IL_1C3:
            if (this._timer___1 >= this._this.durationScale)
            {
                this._this.finger.transform.localScale = this._this.smallScale;
                this._current = new WaitForSeconds(0.1f);
                if (!this._disposing)
                {
                    this._PC = 4;
                }
                return true;
            }
            this._timer___1 += Time.deltaTime;
            this._this.finger.transform.localScale = Vector2.Lerp(this._this.normalScale, this._this.smallScale, this._timer___1 / this._this.durationScale);
            this._current = null;
            if (!this._disposing)
            {
                this._PC = 3;
            }
            return true;
        IL_297:
            if (this._timer___1 < this._this.durationMoveUp)
            {
                this._timer___1 += Time.deltaTime;
                this._this.finger.transform.position = Vector2.Lerp(this.startPos, this.targetPos, this._timer___1 / this._this.durationMoveUp);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 5;
                }
                return true;
            }
            this._this.finger.transform.position = this.targetPos;
            this._timer___1 = 0f;
        IL_361:
            if (this._timer___1 >= this._this.durationScale)
            {
                this._current = new WaitForSeconds(0.1f);
                if (!this._disposing)
                {
                    this._PC = 7;
                }
                return true;
            }
            this._timer___1 += Time.deltaTime;
            this._this.finger.transform.localScale = Vector2.Lerp(this._this.normalScale, this._this.smallScale, 1f - this._timer___1 / this._this.durationScale);
            this._current = null;
            if (!this._disposing)
            {
                this._PC = 6;
            }
            return true;
        IL_41B:
            if (this._timer___1 >= this._this.durationMoveDown)
            {
                this._this.finger.transform.position = this._preStart___0;
                this._current = new WaitForSeconds(0.3f);
                if (!this._disposing)
                {
                    this._PC = 9;
                }
            }
            else
            {
                this._timer___1 += Time.deltaTime;
                this._this.finger.transform.position = Vector2.Lerp(this._preStart___0, this.targetPos, 1f - this._timer___1 / this._this.durationMoveDown);
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 8;
                }
            }




        IL_DB:
            if (this._timer___1 >= this._this.durationMoveShort)
            {
                this._this.finger.transform.position = this.startPos;
                this._current = new WaitForSeconds(0.1f);
                if (!this._disposing)
                {
                    this._PC = 2;
                }
                return true;
            }
            this._timer___1 += Time.deltaTime;
            this._this.finger.transform.position = Vector2.Lerp(this._preStart___0, this.startPos, this._timer___1 / this._this.durationMoveShort);
            this._current = null;
            if (!this._disposing)
            {
                this._PC = 1;
            }

            return true;


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

    public GameObject finger;

    public GameObject circle;

    public float durationMoveUp;

    public float durationMoveDown;

    public float durationScale;

    public float durationMoveShort;

    public Tutorial.State state;

    private Grid grid;

    private IEnumerator FingerMove;

    private Vector3 normalScale;

    private Vector3 smallScale;

    public void Preload()
    {
        if (MainState.typePlay == MainState.TypePlay.Tutorial)
        {
            this.grid = MainObjControl.Instant.grid;
            this.finger.SetActive(false);
            this.normalScale = new Vector3(0.8f, 0.8f, 1f);
            this.smallScale = new Vector3(0.66f, 0.66f, 1f);
        }
    }

    public void Show()
    {
        this.state = Tutorial.State.Step0;
        this.finger.SetActive(false);
        this.Next();
    }

    public void Next()
    {
        Tutorial.State state = this.state;
        if (state != Tutorial.State.Step0)
        {
            if (state != Tutorial.State.Step1)
            {
                if (state == Tutorial.State.Step2)
                {
                    base.StartCoroutine(this.WaitOpenMap3());
                    this.state = Tutorial.State.Step3;
                    MainState.typePlay = MainState.TypePlay.Normal;
                }
            }
            else
            {
                base.StartCoroutine(this.WaitOpenMap2());
                this.state = Tutorial.State.Step2;
            }
        }
        else
        {
            this.OpenMap1();
            this.state = Tutorial.State.Step1;
        }
    }

    private IEnumerator WaitOpenMap2()
    {
        Tutorial._WaitOpenMap2_c__Iterator0 _WaitOpenMap2_c__Iterator = new Tutorial._WaitOpenMap2_c__Iterator0();
        _WaitOpenMap2_c__Iterator._this = this;
        return _WaitOpenMap2_c__Iterator;
    }

    private IEnumerator WaitOpenMap3()
    {
        Tutorial._WaitOpenMap3_c__Iterator1 _WaitOpenMap3_c__Iterator = new Tutorial._WaitOpenMap3_c__Iterator1();
        _WaitOpenMap3_c__Iterator._this = this;
        return _WaitOpenMap3_c__Iterator;
    }

    public void StopFinger()
    {
        if (this.FingerMove != null)
        {
            base.StopCoroutine(this.FingerMove);
            this.finger.SetActive(false);
        }
    }

    public void StartFinger()
    {
        if (this.FingerMove != null)
        {
            this.finger.SetActive(true);
            base.StartCoroutine(this.FingerMove);
        }
    }

    private void OpenMap1()
    {
        this.LoadMap1();
        NextViewer nextViewer = MainObjControl.Instant.nextViewerCtr.listView[1];
        global::Types type = global::Types.I1;
        List<CubeUnit> listUnit = MainObjControl.Instant.pattemCreater.CreatePattem(type, nextViewer.transform.position, nextViewer.scale);
        nextViewer.SetPattem(listUnit, type, 0);
        nextViewer.state = NextViewer.State.Show;
        Vector2 startPos = nextViewer.transform.position;
        Vector2 targetPos = new Vector2(3f, 3f);
        this.FingerMove = this.StartMoveFinger(startPos, targetPos);
        this.StartFinger();
    }

    private void LoadMap1()
    {
        for (int i = 0; i < this.grid.numberRow; i++)
        {
            for (int j = 0; j < this.grid.numberCol; j++)
            {
                if (j >= 2 && j <= 4 && i != 3)
                {
                    this.grid.grid[i, j] = MainObjControl.Instant.pattemCreater.CreateABlock(new Vector2((float)j, (float)i), 1f);
                    if (i != 6)
                    {
                        if (i != 7 && i != 2 && i != 4)
                        {
                            if (j == 3 && i < 4)
                            {
                            }
                        }
                    }
                    this.grid.grid[i, j].row = i;
                    this.grid.grid[i, j].col = j;
                }
            }
        }
    }

    private void OpenMap2()
    {
        this.LoadMap2();
        NextViewer nextViewer = MainObjControl.Instant.nextViewerCtr.listView[1];
        global::Types type = global::Types.I1;
        List<CubeUnit> listUnit = MainObjControl.Instant.pattemCreater.CreatePattem(type, nextViewer.transform.position, nextViewer.scale);
        nextViewer.SetPattem(listUnit, type, 1);
        nextViewer.rotateTime = 1;
        nextViewer.state = NextViewer.State.Show;
        Vector2 startPos = nextViewer.transform.position;
        Vector2 targetPos = new Vector2(2f, 4f);
        this.FingerMove = this.StartMoveFinger(startPos, targetPos);
        this.finger.SetActive(true);
        base.StartCoroutine(this.FingerMove);
    }

    private void LoadMap2()
    {
        for (int i = 0; i < this.grid.numberRow; i++)
        {
            for (int j = 0; j < this.grid.numberCol; j++)
            {
                if (i >= 3 && i <= 5 && j != 2)
                {
                    this.grid.grid[i, j] = MainObjControl.Instant.pattemCreater.CreateABlock(new Vector2((float)j, (float)i), 1f);
                    if (j >= 5)
                    {
                        if (i != 6)
                        {
                            if (i != 7 && i != 7 && i != 4)
                            {
                                if (j == 1 && i < 2)
                                {
                                }
                            }
                        }
                    }
                    this.grid.grid[i, j].row = i;
                    this.grid.grid[i, j].col = j;
                }
            }
        }
    }

    private void OpenMap3()
    {
        this.LoadMap3();
        MainCanvas.Main.inGameScript.ResetScore();
        MainCanvas.Main.inGameScript.ResetBest();
        MainCanvas.Main.inGameScript.lastBest = 0;
        NextViewer nextViewer = MainObjControl.Instant.nextViewerCtr.listView[1];
        global::Types type = global::Types.O1;
        List<CubeUnit> listUnit = MainObjControl.Instant.pattemCreater.CreatePattem(type, nextViewer.transform.position, nextViewer.scale);
        nextViewer.SetPattem(listUnit, type, 0);
        nextViewer.state = NextViewer.State.Show;
    }

    private void LoadMap3()
    {
        for (int i = 0; i < this.grid.numberRow; i++)
        {
            for (int j = 0; j < this.grid.numberCol; j++)
            {
                if ((i == 3 || i == 4 || j == 3 || j == 4) && (i != 3 || j != 3) && (i != 4 || j != 4) && (i != 4 || j != 3) && (i != 3 || j != 4))
                {
                    this.grid.grid[i, j] = MainObjControl.Instant.pattemCreater.CreateABlock(new Vector2((float)j, (float)i), 1f);
                    if (i != 5)
                    {
                        if (j != 5 || i >= 2)
                        {
                            if (i != 1 && i != 5 && i != 2)
                            {
                                if (j == 5 && i < 6)
                                {
                                }
                            }
                        }
                    }
                    this.grid.grid[i, j].row = i;
                    this.grid.grid[i, j].col = j;
                }
            }
        }
    }

    private IEnumerator StartMoveFinger(Vector2 startPos, Vector2 targetPos)
    {
        Tutorial._StartMoveFinger_c__Iterator2 _StartMoveFinger_c__Iterator = new Tutorial._StartMoveFinger_c__Iterator2();
        _StartMoveFinger_c__Iterator.startPos = startPos;
        _StartMoveFinger_c__Iterator.targetPos = targetPos;
        _StartMoveFinger_c__Iterator._this = this;
        return _StartMoveFinger_c__Iterator;
    }
}
