using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private sealed class _StartLight_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float _timer___0;

        internal Loading _this;

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

        public _StartLight_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._this.async = SceneManager.LoadSceneAsync("AuthenticationScene");
                    this._this.async.allowSceneActivation = false;
                    this._current = new WaitForSeconds(this._this.delayStart);
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                case 1u:
                    this._timer___0 = 0f;
                    break;
                case 2u:
                    break;
                case 3u:
                    this._this.async.allowSceneActivation = true;
                    this._PC = -1;
                    return false;
                default:
                    return false;
            }
            if (this._timer___0 >= this._this.duration)
            {
                this._current = new WaitForSeconds(this._this.delay);
                if (!this._disposing)
                {
                    this._PC = 3;
                }
                return true;
            }
            this._timer___0 += Time.deltaTime;
            this._this.image.fillAmount = this._timer___0 / this._this.duration;
            this._current = null;
            if (!this._disposing)
            {
                this._PC = 2;
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

    public float delayStart;

    public float delay;

    public float duration;

    public Image image;

    private AsyncOperation async;

    private void Awake()
    {
        //GoogleMobileAdsController.OnAdReadyToServe += LoadGame_OnAdReadyToServe;
    }

    private void LoadGame_OnAdReadyToServe(object sender, EventArgs e)
    {
        base.StartCoroutine(this.StartLight());
    }

    private IEnumerator StartLight()
    {
        Loading._StartLight_c__Iterator0 _StartLight_c__Iterator = new()
        {
            _this = this
        };
        return _StartLight_c__Iterator;
    }

    private void OnDisable()
    {
       // GoogleMobileAdsController.OnAdReadyToServe -= LoadGame_OnAdReadyToServe;
    }
}
