using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ScoreCtr : MonoBehaviour
{
    public struct ScoreData
    {
        public GameObject scoreObj;

        public TextMesh scoreTxt;
    }

    private sealed class _DelayShowScore_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal ScoreCtr.ScoreData _text___0;

        internal Vector2 pos;

        internal int score;

        internal int line;

        internal ScoreCtr _this;

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

        public _DelayShowScore_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current = new WaitForSeconds(this._this.delay);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    this._text___0 = this._this.GetText();
                    this._text___0.scoreObj.transform.position = this.pos;
                    this._text___0.scoreTxt.text = this.score.ToString();
                   // Debug.Log($"DelayShowScore: Showing score {this.score} at position {this.pos}");
                    if (this.line >= 3)
                    {
                        SpriteRenderer image = this._this.GetImage(this.line);
                        Vector2 v = this.pos + Vector2.up * 0.7f;
                        if (this.line == 3)
                        {
                            v = new Vector2(Mathf.Clamp(v.x, this._this.minGood, this._this.maxGood), v.y);
                        }
                        else if (this.line == 4)
                        {
                            v = new Vector2(Mathf.Clamp(v.x, this._this.minExcelent, this._this.maxExcelent), v.y);
                        }
                        else if (this.line >= 5)
                        {
                            v = new Vector2(Mathf.Clamp(v.x, this._this.minPerfect, this._this.maxPerfect), v.y);
                        }
                        MainAudio.Main.PlaySound(TypeAudio.SoundGood);
                        image.gameObject.transform.position = v;
                        this._this.StartCoroutine(this._this.WaitDisable(image));
                    }
                    this._this.StartCoroutine(this._this.FadeOut(this._text___0));
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

    private sealed class _WaitDisable_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SpriteRenderer imageObj;

        internal ScoreCtr _this;

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

        public _WaitDisable_c__Iterator1()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current = new WaitForSeconds(2f);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    this._this.SetImage(this.imageObj);
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

    private sealed class _FadeOut_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal ScoreCtr.ScoreData textData;

        internal TextMesh _text___0;

        internal float _t___0;

        internal float _k___1;

        internal ScoreCtr _this;

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

        public _FadeOut_c__Iterator2()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._text___0 = this.textData.scoreTxt;
                    this._t___0 = 0f;
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (this._t___0 < this._this.duration)
            {
                this._t___0 += Time.deltaTime;
                this._k___1 = this._t___0 / this._this.duration;
                this._this.mat.SetColor("_Color", Color.Lerp(this._this.startColor, this._this.endColor, this._this.ac.Evaluate(this._k___1)));
                this._current = null;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            this._this.SetText(this.textData);
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

    public GameObject scorePrefab;

    public GameObject imagePrefab;

    public List<ScoreCtr.ScoreData> listText;

    public List<SpriteRenderer> listImage;

    public Material mat;

    public AnimationCurve ac;

    public float delay;

    public Color startColor;

    public Color endColor;

    public float duration;

    public Sprite good;

    public Sprite exellent;

    public Sprite perfect;

    public float minGood;

    public float maxGood;

    public float minExcelent;

    public float maxExcelent;

    public float minPerfect;

    public float maxPerfect;
    public List<int> lineBroken;

    public void Preload()
    {
        this.listText = new List<ScoreCtr.ScoreData>();
        this.listImage = new List<SpriteRenderer>();
        ScoreCtr.ScoreData text = this.GetText();
        ScoreCtr.ScoreData text2 = this.GetText();
        this.SetText(text);
        this.SetText(text2);
        SpriteRenderer image = this.GetImage(3);
        this.SetImage(image);
    }

    public ScoreCtr.ScoreData GetText()
    {
        ScoreCtr.ScoreData result;
        if (this.listText.Count == 0)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.scorePrefab);
            result = default(ScoreCtr.ScoreData);
            result.scoreObj = gameObject;
            result.scoreTxt = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        }
        else
        {
            result = this.listText[this.listText.Count - 1];
            result.scoreObj.SetActive(true);
            result.scoreTxt.color = this.startColor;
            this.listText.RemoveAt(this.listText.Count - 1);
        }
        return result;
    }

    public void SetText(ScoreCtr.ScoreData text)
    {
        text.scoreObj.SetActive(false);
        this.listText.Add(text);
    }

    public SpriteRenderer GetImage(int line)
    {
        SpriteRenderer spriteRenderer;
        if (this.listImage.Count == 0)
        {
            spriteRenderer = UnityEngine.Object.Instantiate<GameObject>(this.imagePrefab).GetComponent<SpriteRenderer>();
        }
        else
        {
            spriteRenderer = this.listImage[this.listImage.Count - 1];
            spriteRenderer.color = this.startColor;
            this.listImage.RemoveAt(this.listImage.Count - 1);
        }
        if (line == 3)
        {
            spriteRenderer.sprite = this.good;
        }
        else if (line == 4)
        {
            spriteRenderer.sprite = this.exellent;
        }
        else if (line >= 5)
        {
            spriteRenderer.sprite = this.perfect;
        }
        else
        {
            spriteRenderer.sprite = this.good;
        }
        spriteRenderer.gameObject.SetActive(true);
        return spriteRenderer;
    }

    public void SetImage(SpriteRenderer text)
    {
        text.gameObject.SetActive(false);
        this.listImage.Add(text);
    }

    public void ShowText(Vector2 pos, int score, int line)
    {
        // UnityEngine.Debug.Log(line);
        Debug.Log($"ShowText called with pos: {pos}, score: {score}, line: {line}");
        lineBroken.Add(line);
        base.StartCoroutine(this.DelayShowScore(pos, score, line));
    
    }

    private IEnumerator DelayShowScore(Vector2 pos, int score, int line)
    {
        ScoreCtr._DelayShowScore_c__Iterator0 _DelayShowScore_c__Iterator = new ScoreCtr._DelayShowScore_c__Iterator0
        {
            pos = pos,
            score = score,
            line = line,
            _this = this
        };
        return _DelayShowScore_c__Iterator;
    }

    private IEnumerator WaitDisable(SpriteRenderer imageObj)
    {
        ScoreCtr._WaitDisable_c__Iterator1 _WaitDisable_c__Iterator = new ScoreCtr._WaitDisable_c__Iterator1
        {
            imageObj = imageObj,
            _this = this
        };
        return _WaitDisable_c__Iterator;
    }

    private IEnumerator FadeOut(ScoreCtr.ScoreData textData)
    {
        ScoreCtr._FadeOut_c__Iterator2 _FadeOut_c__Iterator = new ScoreCtr._FadeOut_c__Iterator2
        {
            textData = textData,
            _this = this
        };
        return _FadeOut_c__Iterator;
    }
}
