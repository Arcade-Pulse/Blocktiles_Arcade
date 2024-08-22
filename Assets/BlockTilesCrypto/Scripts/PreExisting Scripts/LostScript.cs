using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using Global;
using PlayFabPersonal.Economy;
using PlayFabPersonal.Managers;
using TMPro;
using UnityEditor.Minesweeper.Scripts.UnityAds;
using UnityEngine;

public class LostScript : MonoBehaviour
{
    public int timesLost;
    public void UpdatePlayerStatus()
    {
        try
        {
            if (PlayfabDataManager.Instance != null)
            {
                PlayfabDataManager.Instance.LoadPlayerInventory2();
            }
            else
            {
                Debug.Log("PlayfabDataManager.Instance is null.");
            }
            
        }
        catch (NullReferenceException ex)
        {
           // Debug.Log("NullReferenceException in MenuPanel.OnEnable: " + ex.Message);
        }
        catch (Exception ex)
        {
          //  Debug.Log("Unexpected error in MenuPanel.OnEnable: " + ex.Message);
        }
    }
        
    private void OnEnable()
    {
        UpdatePlayerStatus();
    }

    private sealed class _WaitSwitch_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal bool isGray;

        internal LostScript _this;

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

        public _WaitSwitch_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    if (this.isGray)
                    {
                        this._current = new WaitForSeconds(1f);
                        if (!this._disposing)
                        {
                            this._PC = 1;
                        }
                        return true;
                    }
                    this._current = new WaitForSeconds(0.3f);
                    if (!this._disposing)
                    {
                        this._PC = 3;
                    }
                    return true;
                case 1u:
                    MainAudio.Main.PlaySound(TypeAudio.SoundLose);
                    MainObjControl.Instant.grid.GrayScaleMap(true);
                    this._current = new WaitForSeconds(2.8f);
                    if (!this._disposing)
                    {
                        this._PC = 2;
                    }
                    return true;
                case 2u:
                    break;
                case 3u:
                    break;
                default:
                    return false;
            }
            // AdsControl.Instance.showAds();
            // this._this.SetBestScore();
            this._this.StartCoroutine(this._this.WaitState());
            MainCanvas.Main.faderScript.SwichPanelFix(MainCanvas.Main.inGameScript.group, this._this.group, null);
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

    private sealed class _ScoreUpAnim_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int _current___0;

        internal int _unit___0;

        internal LostScript _this;

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

        public _ScoreUpAnim_c__Iterator1()
        {
        }

        public bool MoveNext()
        {
            
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current___0 = 0;
                    this._unit___0 = Mathf.Max(3, this._this.score / 30);
                    if (this._unit___0 == 5)
                    {
                        this._unit___0 = 4;
                    }
                    else if (this._unit___0 == 10)
                    {
                        this._unit___0 = 9;
                    }
                    else if (this._unit___0 == 15)
                    {
                        this._unit___0 = 14;
                    }
                    MainAudio.Main.PlaySound(TypeAudio.SoundScore);
                    break;
                case 1u:
                    this._current___0 = Mathf.Min(this._this.score, this._current___0 + this._unit___0);
                    this._this.SetCoin(this._current___0);
                    break;
                case 2u:
                    MainAudio.Main.PlaySound(TypeAudio.SoundOverBest);
                    // this._this.newObj.SetActive(true);
                    goto IL_181;
                default:
                    return false;
            }
            if (this._current___0 >= this._this.score)
            {
                MainAudio.Main.StopSound(TypeAudio.SoundScore);
                this._this.SetCoin(this._this.score);
                int previousScore = this._this.coinManager.GetPreviousScore();
                int scoreDifference = Mathf.Abs(previousScore - this._this.scoreOnly);
              //  Debug.Log("adding coins "+CoinManager.Instance.earnedCoinsInTheRound);

                VirtualCurrency.Instance.AddGameSocCurrency(CoinManager.Instance.earnedCoinsInTheRound);
                CoinManager.Instance.earnedCoinsInTheRound = 0;
              //  Debug.Log("earned points 3"+CoinManager.Instance.earnedCoinsInTheRound);

                this._this.coinManager.SetPreviousScore(0);
                GameSceneManager.Instance.SetButtonInteractable(true);
                if (!MainCanvas.Main.inGameScript.isOverBest)
                {
                    goto IL_181;
                }
                this._current = new WaitForSeconds(0.15f);
                if (!this._disposing)
                {
                    this._PC = 2;
                }
            }
            else
            {
                this._current = new WaitForSeconds(0.05f);
                if (!this._disposing)
                {
                    this._PC = 1;
                }
            }
            return true;
        IL_181:
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

    private sealed class _WaitState_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal LostScript _this;

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

        public _WaitState_c__Iterator2()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._current = new WaitForSeconds(0.25f);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    // this._this.StartCoroutine(EffectControl.MoveAnchor(this._this.topRect, Vector2.up * -200f, 0.25f, null));
                    this._current = new WaitForSeconds(0.1f);
                    if (!this._disposing)
                    {
                        this._PC = 2;
                    }
                    return true;
                case 2u:
                    this._this.StartCoroutine(EffectControl.Scale(this._this.scorePane.transform, Vector3.one * 1f, 0.45f, null));
                    this._current = new WaitForSeconds(0.1f);
                    if (!this._disposing)
                    {
                        this._PC = 3;
                    }
                    return true;
                case 3u:
                    this._this.AnimScore();
                    this._current = new WaitForSeconds(0.2f);
                    if (!this._disposing)
                    {
                        this._PC = 4;
                    }
                    return true;
                case 4u:
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.bestPanel.transform, Vector3.one * 0.8f, 0.3f, null));
                    // this._this.StartCoroutine(EffectControl.ScaleInOut(this._this.cup.transform, Vector3.one, Vector3.one * 1.7f, 0.5f));
                    this._current = new WaitForSeconds(0.2f);
                    if (!this._disposing)
                    {
                        this._PC = 5;
                    }
                    return true;
                case 5u:
                    this._this.playButton.SetActive(true);
                    this._this.StartCoroutine(EffectControl.ScaleInOut(this._this.playButton.transform, Vector3.one, Vector3.one * 1.3f, 0.3f));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 6;
                    }
                    return true;
                // case 6u:
                // 	// this._this.moreButton.SetActive(true);
                // 	this._this.StartCoroutine(EffectControl.ScaleInOut(this._this.moreButton.transform, Vector3.one, Vector3.one * 1.3f, 0.3f));
                // 	this._current = new WaitForSeconds(0.05f);
                // 	if (!this._disposing)
                // 	{
                // 		this._PC = 7;
                // 	}
                // 	return true;
                // case 7u:
                // 	// this._this.rateButton.SetActive(true);
                // 	this._this.StartCoroutine(EffectControl.ScaleInOut(this._this.rateButton.transform, Vector3.one, Vector3.one * 1.3f, 0.3f));
                // 	this._current = new WaitForSeconds(0.05f);
                // 	if (!this._disposing)
                // 	{
                // 		this._PC = 8;
                // 	}
                // 	return true;
                // case 8u:
                // 	// this._this.soundButton.SetActive(true);
                // 	this._this.StartCoroutine(EffectControl.ScaleInOut(this._this.soundButton.transform, Vector3.one, Vector3.one * 1.3f, 0.3f));
                // 	this._current = new WaitForSeconds(0.05f);
                // 	if (!this._disposing)
                // 	{
                // 		this._PC = 9;
                // 	}
                // 	return true;
                case 9u:
                    MainState.state = MainState.State.GameOver;
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

    private sealed class _AnimOut_c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal LostScript _this;

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

        public _AnimOut_c__Iterator3()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    MainObjControl.Instant.grid.SetAllBlock();
                    MainObjControl.Instant.nextViewerCtr.SetAllBlock();
                    MainObjControl.Instant.planeViewCrt.SetAllBlock();
                    MainObjControl.Instant.helpCtr.HideAllBlock();
                    // this._this.soundButton.SetActive(true);
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.soundButton.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    // this._this.rateButton.SetActive(true);
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.rateButton.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 2;
                    }
                    return true;
                case 2u:
                    // this._this.moreButton.SetActive(true);
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.moreButton.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 3;
                    }
                    return true;
                case 3u:
                    this._this.playButton.SetActive(true);
                    this._this.StartCoroutine(EffectControl.Scale(this._this.playButton.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.1f);
                    if (!this._disposing)
                    {
                        this._PC = 4;
                    }
                    return true;
                case 4u:
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.cup.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 5;
                    }
                    return true;
                case 5u:
                    // this._this.StartCoroutine(EffectControl.Scale(this._this.bestPanel.transform, Vector3.zero, 0.3f, null));
                    this._current = new WaitForSeconds(0.05f);
                    if (!this._disposing)
                    {
                        this._PC = 6;
                    }
                    return true;
                case 6u:
                    this._this.StartCoroutine(EffectControl.Scale(this._this.scorePane.transform, Vector3.zero, 0.3f, null));
                    MainAudio.Main.StopSound(TypeAudio.SoundScore);
                    this._current = new WaitForSeconds(0.4f);
                    if (!this._disposing)
                    {
                        this._PC = 7;
                    }
                    return true;
                case 7u:
                    // this._this.StartCoroutine(EffectControl.MoveAnchor(this._this.topRect, Vector2.up * 200f, 0.15f, null));
                    // this._this.StartCoroutine(EffectControl.FadeOutCanvas(this._this.centerBG, 0.2f));
                    this._current = new WaitForSeconds(0.3f);
                    if (!this._disposing)
                    {
                        this._PC = 8;
                    }
                    return true;
                case 8u:
                    MainCanvas.Main.faderScript.FadeOutPanelFix(this._this.group, new FaderControl.Callback0(this._this.Next));
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

    public TMP_Text gameSocCoinText;
    public TMP_Text scoreText;
    public TMP_Text gameSocCashoutText;

    // public Text bestText;

    public CanvasGroup group;

    public bool isAuto;

    public GameObject scorePane;

    // public GameObject bestPanel;

    // public GameObject cup;

    public GameObject playButton;

    // public GameObject moreButton;

    // public GameObject rateButton;

    // public GameObject soundButton;

    // public GameObject newObj;

    // public RectTransform topRect;

    // public GameObject centerBG;

    private int score;
    private int scoreOnly;

    private bool pressed;

    private Vector3 startScale;

    // public RectTransform cupRect;

    private CoinManager coinManager;

    private void Awake()
    {
        coinManager = FindAnyObjectByType<CoinManager>();
    }

    public void Preload()
    {
        this.SetActive(false);
        this.startScale = new Vector3(0.1f, 1f, 1f);
    }

    public void Reset()
    {
        this.gameSocCoinText.text = "0";
        this.SetActive(false);
        this.pressed = false;
        this.playButton.transform.localScale = this.startScale;
        // this.moreButton.transform.localScale = this.startScale;
        // this.rateButton.transform.localScale = this.startScale;
        // this.soundButton.transform.localScale = this.startScale;
        this.playButton.SetActive(false);
        // this.moreButton.SetActive(false);
        // this.rateButton.SetActive(false);
        // this.soundButton.SetActive(false);
        this.scorePane.transform.localScale = Vector3.zero;
        // this.bestPanel.transform.localScale = Vector3.zero;
        // this.cup.transform.localScale = Vector3.zero;
        // this.topRect.anchoredPosition = Vector2.up * 200f;
        // this.newObj.SetActive(false);
        // EffectControl.FadeInCanvasNow(this.centerBG);
    }

    public void GameOver(bool isGray)
    {
       // Debug.Log("Game is over");
        Settings.SetContinue(0);
        this.SetActive(true);
        this.group.alpha = 0f;
        if (this.isAuto)
        {
            MainState.SetState(MainState.State.GameOver);
        }
        else
        {
            MainState.SetState(MainState.State.Waiting);
        }
       // VirtualCurrency.Instance.AddGameSocCurrency(CoinManager.Instance.earnedCoinsInTheRound);
      //  CoinManager.Instance.earnedCoinsInTheRound = 0;
        this.score = MainCanvas.Main.inGameScript.scoreInt;
        this.scoreOnly = MainCanvas.Main.inGameScript.scoreOnly;
        scoreText.text = scoreOnly.ToString();
        base.StartCoroutine(this.WaitSwitch(isGray));

        // timesLost++;
        // if (timesLost>2)
        // {
        InterstitialAds.Instance.ShowAd();
        //}
    }

    private IEnumerator WaitSwitch(bool isGray)
    {
        LostScript._WaitSwitch_c__Iterator0 _WaitSwitch_c__Iterator = new LostScript._WaitSwitch_c__Iterator0
        {
            isGray = isGray,
            _this = this
        };
        return _WaitSwitch_c__Iterator;
    }

    private void AnimScore()
    {
        base.StartCoroutine(this.ScoreUpAnim());
    }

    private IEnumerator ScoreUpAnim()
    {
        LostScript._ScoreUpAnim_c__Iterator1 _ScoreUpAnim_c__Iterator = new LostScript._ScoreUpAnim_c__Iterator1();
        _ScoreUpAnim_c__Iterator._this = this;
        return _ScoreUpAnim_c__Iterator;
    }

    private IEnumerator WaitState()
    {
        LostScript._WaitState_c__Iterator2 _WaitState_c__Iterator = new LostScript._WaitState_c__Iterator2();
        _WaitState_c__Iterator._this = this;
        return _WaitState_c__Iterator;
    }

    public void TryAgainButton()
    {
        if (MainState.state == MainState.State.GameOver && !this.pressed)
        {
            this.pressed = true;
            base.StartCoroutine(this.AnimOut());
        }
    }

    private IEnumerator AnimOut()
    {
        LostScript._AnimOut_c__Iterator3 _AnimOut_c__Iterator = new LostScript._AnimOut_c__Iterator3();
        _AnimOut_c__Iterator._this = this;
        return _AnimOut_c__Iterator;
    }

    private void Next()
    {
        MainCanvas.Main.Reset(true);
    }

    public void SetActive(bool isActive)
    {
        base.gameObject.SetActive(isActive);
    }

    public void SetCoin(int coin)
    {
        this.gameSocCoinText.text = coin.ToString();
        this.gameSocCashoutText.text = coin.ToString();
    }

    // public void SetBestScore()
    // {
    // 	int num = Mathf.Max(MainCanvas.Main.inGameScript.bestInt, this.score);
    // 	// this.bestText.text = num.ToString();
    // 	float num2;
    // 	switch (this.bestText.text.Length)
    // 	{
    // 	case 0:
    // 	case 1:
    // 	case 2:
    // 		num2 = -200f;
    // 		break;
    // 	case 3:
    // 		num2 = -240f;
    // 		break;
    // 	case 4:
    // 		num2 = -290f;
    // 		break;
    // 	case 5:
    // 		num2 = -330f;
    // 		break;
    // 	case 6:
    // 		num2 = -370f;
    // 		break;
    // 	case 7:
    // 		num2 = -410f;
    // 		break;
    // 	default:
    // 		num2 = -450f;
    // 		break;
    // 	}
    // 	if (!MainCanvas.Main.inGameScript.isOverBest)
    // 	{
    // 		num2 += 20f;
    // 	}
    // 	this.cupRect.anchoredPosition = new Vector2(num2, this.cupRect.anchoredPosition.y);
    // }
}
