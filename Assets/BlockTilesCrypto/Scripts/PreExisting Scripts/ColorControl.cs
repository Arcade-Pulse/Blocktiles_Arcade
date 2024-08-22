using System;
using System.IO;
using UnityEngine;

public class ColorControl : MonoBehaviour
{
    public Sprite[] preloadSprite;
    public Sprite[] preloadBlockColorsNormal;
    public Sprite[] preloadBlockColorsHighlight;

    public int preload;

    public Texture2D spriteTexs;

    public int width;

    public int height;

    public Color whiteAlpha;

    public int space;

    private bool isDragged;

    global::Types types;

    private void Awake()
    {
        this.Load();
    }

    private void Start()
    {
        NextViewer.OnCubeDragged += StopSettingColor_OnCubeDrag;
        // PlaneView.OnCubeDropped += StopSettingColor_OnCubeDropped;
    }

    private void OnDisable()
    {
        NextViewer.OnCubeDragged -= StopSettingColor_OnCubeDrag;
        // PlaneView.OnCubeDropped -= StopSettingColor_OnCubeDropped;
    }

    private void StopSettingColor_OnCubeDrag(object sender, NextViewer.OnSuccessfulDragEventArgs e)
    {
        isDragged = e.isDragged;
        types = e.type;
    }

    // private void StopSettingColor_OnCubeDropped(object sender, EventArgs e)
    // {
    //     UnityEngine.Debug.Log("Cube dropped");
    // }

    private void CreateAliasBlock()
    {
        Texture2D texture2D = new Texture2D(116 * this.preload + 116, 116);
        for (int i = 0; i < this.preload + 1; i++)
        {
            for (int j = 0; j < this.width; j++)
            {
                for (int k = 0; k < this.height; k++)
                {
                    Color pixel = this.spriteTexs.GetPixel(j, k);
                    float b = 0.2126f * pixel.r + 0.7152f * pixel.g + 0.0722f * pixel.b;
                    Color color = new Color(Mathf.Lerp(pixel.r, b, (float)i / (float)this.preload), Mathf.Lerp(pixel.g, b, (float)i / (float)this.preload), Mathf.Lerp(pixel.b, b, (float)i / (float)this.preload), pixel.a);
                    if (i == 0)
                    {
                        texture2D.SetPixel(j + i * this.width, k, pixel + new Color(0.12f, 0.12f, 0.12f, 0f));
                    }
                    else if (i == 1)
                    {
                        texture2D.SetPixel(j + i * this.width, k, pixel);
                    }
                    else
                    {
                        texture2D.SetPixel(j + i * this.width, k, color);
                    }
                }
            }
        }
        texture2D.Apply();
        this.SaveTextureToFile(texture2D, "BlockAlias");
    }

    private void SaveTextureToFile(Texture2D texture, string name)
    {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + name + ".png", bytes);
    }

    private void Load()
    {
        this.preloadSprite = Resources.LoadAll<Sprite>("BlockAlias");
        this.preloadBlockColorsNormal = Resources.LoadAll<Sprite>("Blocks_Normal");
        this.preloadBlockColorsHighlight = Resources.LoadAll<Sprite>("Blocks_Highlighted");
    }

    public Sprite GetSpriteGray(float t)
    {
        int a = Mathf.Min(Mathf.RoundToInt(t * (float)this.preload), this.preload - 1);
        return this.preloadSprite[Mathf.Max(a, 1)];
    }

    public Sprite GetSpriteData(int index)
    {
        return this.preloadSprite[index];
    }

    public Sprite GetSpriteBlockHighlightColor(int index)
    {
        return this.preloadBlockColorsHighlight[index];
    }

    public Sprite GetSpriteBlockColor(int index)
    {
        return this.preloadBlockColorsNormal[index];
    }

    public void SetCubeHighlight(CubeUnit cubeUnit)
    {
        if (!isDragged) return;
        // Debug.Log("Enter 3 " + types);
        if (types == global::Types.O0 || types == global::Types.O1 || types == global::Types.O2)
        {
            cubeUnit.SetSprite(GetSpriteBlockHighlightColor(0), 0);
        }
        else if (types == global::Types.L0 || types == global::Types.L1)
        {
            cubeUnit.SetSprite(GetSpriteBlockHighlightColor(1), 0);
        }
        else if (types == global::Types.I0 || types == global::Types.I1 || types == global::Types.I2 || types == global::Types.I3)
        {
            cubeUnit.SetSprite(GetSpriteBlockHighlightColor(2), 0);
        }
        else
        {
            cubeUnit.SetSprite(GetSpriteBlockHighlightColor(3), 0);
        }
    }

    public void SetCubeColor(CubeUnit cubeUnit)
    {
        // Debug.Log("Dragging: " + isDragged);
        if (cubeUnit.thisType == global::Types.O0 || cubeUnit.thisType == global::Types.O1 || cubeUnit.thisType == global::Types.O2)
        {
            cubeUnit.SetSprite(GetSpriteBlockColor(0), 1);
        }
        else if (cubeUnit.thisType == global::Types.L0 || cubeUnit.thisType == global::Types.L1)
        {
            cubeUnit.SetSprite(GetSpriteBlockColor(1), 2);
        }
        else if (cubeUnit.thisType == global::Types.I0 || cubeUnit.thisType == global::Types.I1 || cubeUnit.thisType == global::Types.I2 || cubeUnit.thisType == global::Types.I3)
        {
            cubeUnit.SetSprite(GetSpriteBlockColor(2), 3);
        }
        else
        {
            // Debug.Log("No Match" + cubeUnit.thisType);
            cubeUnit.SetSprite(GetSpriteBlockColor(3), 4);
        }
    }
}
