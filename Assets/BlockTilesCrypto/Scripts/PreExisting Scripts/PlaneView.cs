using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class PlaneView : MonoBehaviour
{
    public enum State
    {
        Free,
        Drag
    }

    public new Camera camera;
    public float y;
    public float scale;
    public float scaleSmall;
    public float distanceTouch;
    public float duration;
    public Transform cam;
    private global::Types myType;
    public List<CubeUnit> listBlock;
    private Vector2[] listBlockLocalPos;
    private Vector3 touchPos;
    private Vector3 startPos;
    public int selected;
    private int row;
    private int col;
    private GroundView groundView;
    private Grid grid;
    private NextViewerControl nextViewerCtr;
    private PattemCreater pattemCreater;
    private IEnumerator ScaleUpAnim;
    private bool isScaling;
    private bool groundAcepted;
    private Vec2 cellAcepted = new Vec2();
    private Vec2 lastResetFillCel;
    public Vector2 faceMousePos;
    public bool isAuto;
    public float speedMoveDrop;
    public PlaneView.State state;
    private ColorControl colorCtr;

    public static event EventHandler OnCubeDropped;

    private void Start()
    {
        listBlock = new List<CubeUnit>();
        row = MainObjControl.Instant.grid.numberRow;
        col = MainObjControl.Instant.grid.numberCol;
        groundView = MainObjControl.Instant.groundView;
        grid = MainObjControl.Instant.grid;
        nextViewerCtr = MainObjControl.Instant.nextViewerCtr;
        pattemCreater = MainObjControl.Instant.pattemCreater;
        colorCtr = MainObjControl.Instant.colorControl;
    }

    public void SetPattem(List<CubeUnit> listBlock0, Vector2 pos, int select, float viewScale)
    {
        state = PlaneView.State.Drag;
        selected = select;
        startPos = pos;
        groundAcepted = false;
        SetAllBlock();
        listBlockLocalPos = new Vector2[listBlock0.Count];

        for (int i = 0; i < listBlock0.Count; i++)
        {
            colorCtr.SetCubeColor(listBlock0[i]);
            int indexRow = listBlock0[i].indexRow;
            int indexCol = listBlock0[i].indexCol;
            Vector2 vector = listBlock0[i].transform.position;
            listBlock.Add(pattemCreater.CreateABlock(listBlock0[i].thisType, vector, scale));
            listBlock[i].thisType = listBlock0[i].thisType;
            listBlock[i].indexRow = indexRow;
            listBlock[i].indexCol = indexCol;
            listBlock[i].SetLayer(GameDefine.selectingLayer);
            listBlockLocalPos[i] = (vector - pos) / viewScale;
        }
        ScaleBlock(scaleSmall);
        SetBlockPos(GetFixedMousePos(), scaleSmall);
        if (ScaleUpAnim != null && isScaling)
        {
            StopCoroutine(ScaleUpAnim);
        }
        ScaleUpAnim = ScaleUpBlock();
        StartCoroutine(ScaleUpAnim);
        MainObjControl.Instant.helpCtr.HideAllBlock();
    }

    private IEnumerator ScaleUpBlock()
    {
        float timer = 0f;
        isScaling = true;

        while (timer < duration && listBlock.Count > 0)
        {
            timer += Time.deltaTime;
            float newScale = Mathf.Lerp(scaleSmall, scale, timer / duration);
            ScaleBlock(newScale);
            Vector2 currentMousePos = GetFixedMousePos();
            SetBlockPos(currentMousePos, newScale);
            yield return null;
        }

        Vector2 finalMousePos = GetFixedMousePos();
        SetBlockPos(finalMousePos, scale);
        isScaling = false;
    }

    private void Update()
    {
        Drag();
        if (isAuto)
        {
            return;
        }
        CheckPlace();
    }

    private void Drag()
    {
        if (state == PlaneView.State.Drag && listBlock.Count > 0 && !isScaling)
        {
            SetBlockPos(GetFixedMousePos(), scale);
            CheckGround();
        }
    }

    private void CheckPlace()
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        if (Mouse.current.leftButton.wasReleasedThisFrame && listBlock.Count > 0)
#else
        if (Input.GetMouseButtonUp(0) && listBlock.Count > 0)
#endif
        {
            OnCubeDropped?.Invoke(this, EventArgs.Empty);
            PlacePattemGround();
        }
    }

    public void PlacePattemGround()
    {
        CheckSelectedBlock();
        SetAllBlock();
        groundView.HideFromBlock(0);
        state = PlaneView.State.Free;
    }

    private void CheckSelectedBlock()
    {
        if (groundAcepted && !isScaling && CheckPlaceInTuto())
        {
            int r = cellAcepted.R;
            int c = cellAcepted.C;
            Vector2 b = new Vector2(c, r);
            Vector2 a = listBlock[0].transform.position;
            float num = (a - b).magnitude / speedMoveDrop;
            num = Mathf.Min(num, 0.09f);
            List<CubeUnit> list = new List<CubeUnit>();
            for (int i = 0; i < listBlock.Count; i++)
            {
                int num2 = cellAcepted.R - (listBlock[0].indexRow - listBlock[i].indexRow);
                int num3 = cellAcepted.C - (listBlock[0].indexCol - listBlock[i].indexCol);
                Vector2 targetPos = new Vector2(num3, num2);
                Vector2 pos = listBlock[i].transform.position;
                CubeUnit cubeUnit = pattemCreater.CreateABlock(listBlock[i].thisType, pos, scale);
                grid.grid[num2, num3] = cubeUnit;
                cubeUnit.col = num3;
                cubeUnit.row = num2;
                cubeUnit.thisType = listBlock[i].thisType;
                list.Add(cubeUnit);
                cubeUnit.DropDown(pos, targetPos, num);
            }
            bool flag = false;
            grid.CheckGrid(list, num, ref flag);
            nextViewerCtr.listView[selected].SetAllBlock();
            nextViewerCtr.listView[selected].state = NextViewer.State.Null;
            if (MainState.typePlay == MainState.TypePlay.Tutorial)
            {
                MainObjControl.Instant.tutorial.Next();
            }
            else
            {
                MainObjControl.Instant.nextViewerCtr.CheckUpdateNewPattem();
            }
            if (flag)
            {
                MainAudio.Main.PlaySound(TypeAudio.SounCollect1);
            }
            else
            {
                MainAudio.Main.PlaySound(TypeAudio.SoundStop);
            }
        }
        else
        {
            nextViewerCtr.listView[selected].ShowAllBlock(listBlock);
            if (MainState.typePlay == MainState.TypePlay.Tutorial)
            {
                MainObjControl.Instant.tutorial.StartFinger();
                grid.TurnOffAllFillLine();
            }
        }
    }

    private bool CheckPlaceInTuto()
    {
        if (MainState.typePlay == MainState.TypePlay.Tutorial)
        {
            int indexRow = listBlock[0].indexRow;
            int indexCol = listBlock[0].indexCol;
            for (int i = 0; i < listBlock.Count; i++)
            {
                int r = cellAcepted.R - (indexRow - listBlock[i].indexRow);
                int c = cellAcepted.C - (indexCol - listBlock[i].indexCol);
                if (!grid.IsRowFillWith(r, c) && !grid.IsColFillWith(r, c))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CheckGround()
    {
        if (IsInvalidGrid())
        {
            groundAcepted = true;
            groundView.SetPattem(listBlock, cellAcepted.R, cellAcepted.C);
            if (lastResetFillCel == null || lastResetFillCel.R != cellAcepted.R || lastResetFillCel.C != cellAcepted.C)
            {
                for (int i = 0; i < listBlock.Count; i++)
                {
                    colorCtr.SetCubeColor(listBlock[i]);
                }
                grid.TurnOffAllFillLine();
                grid.CheckGridFillTest(listBlock, cellAcepted.R, cellAcepted.C);
                lastResetFillCel = new Vec2(cellAcepted.R, cellAcepted.C);
            }
        }
        else
        {
            groundAcepted = false;
            grid.TurnOffAllFillLine();
            groundView.HideFromBlock(0);
            lastResetFillCel = null;
            for (int j = 0; j < listBlock.Count; j++)
            {
                if (listBlock[j].ID != 1 && listBlock[j].ID != 2 && listBlock[j].ID != 3 && listBlock[j].ID != 4)
                {
                    colorCtr.SetCubeColor(listBlock[j]);
                }
            }
        }
    }

    private bool IsInvalidGrid()
    {
        Vector2 vector = listBlock[0].transform.position;
        cellAcepted.C = Mathf.RoundToInt(vector.x);
        cellAcepted.R = Mathf.RoundToInt(vector.y);
        return !grid.InvalidPoint(listBlock, cellAcepted.R, cellAcepted.C);
    }

    private Vector2 GetFixedMousePos()
    {
        if (!isAuto)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            Vector2 result = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
#else
            Vector2 result = camera.ScreenToWorldPoint(Input.mousePosition);
#endif
            result = new Vector2(result.x, result.y + distanceTouch);
            return result;
        }
        return faceMousePos;
    }

    private void SetBlockPos(Vector3 pos, float newScale)
    {
        for (int i = 0; i < listBlock.Count; i++)
        {
            float x = pos.x + listBlockLocalPos[i].x * Mathf.Min(newScale, scale) / scale;
            float num = pos.y + listBlockLocalPos[i].y * Mathf.Min(newScale, scale) / scale;
            listBlock[i].transform.position = new Vector2(x, num);
        }
    }

    private void ScaleBlock(float newScale)
    {
        for (int i = 0; i < listBlock.Count; i++)
        {
            listBlock[i].transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void SetAllBlock()
    {
        for (int i = 0; i < listBlock.Count; i++)
        {
            listBlock[i].SetLayer(GameDefine.freeLayer);
            MainObjControl.Instant.pattemCreater.SetCube(listBlock[i]);
        }
        listBlock.Clear();
    }
}
