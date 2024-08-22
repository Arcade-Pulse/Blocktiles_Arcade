using System;
using PlayFabPersonal.Managers;
using UnityEngine;

public class GameDefine
{
    public static Vector2 posRight = new Vector2(2000f, 0f);

    public static Vector2 posLeft = new Vector2(-2000f, 0f);

    public static Vector2 posTop = new Vector2(0f, 2000f);

    public static Vector2 posCenter = new Vector2(0f, 0f);

    public static float pikachuNormalScale = 0.48f;

    public static float pikachuYScale = 0.15f;

    public static string pageID = "1384190174944235";

    public static string pageName = "Connect Animal Game";

    public static int startRow = 16;

    public static int startCol = 4;

    public static int selectingLayer = 5;

    public static int freeLayer = 0;

    public static float pattemDarkAlpha = 0.38f;

    public static float pattemLightAlpha = 1f;

    public static int GetScore(int numberLine)
    {
        ScoreData scoreData = PlayfabDataManager.Instance.GetScoreData();
        return numberLine switch
        {
            0 => scoreData._0,
            1 => scoreData._1,
            2 => scoreData._2,
            3 => scoreData._3,
            4 => scoreData._4,
            5 => scoreData._5,
            6 => scoreData._6,
            7 => scoreData._7,
            _ => scoreData._,
        };
    }
}
