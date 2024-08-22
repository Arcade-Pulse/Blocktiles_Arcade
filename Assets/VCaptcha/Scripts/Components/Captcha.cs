// (c) Copyright VinforLab Team. All rights reserved.

using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabPersonal.Managers;
using UnityEngine;

namespace VinforlabTeam.VCaptcha
{
    public class Captcha : MonoBehaviour
    {
        public string captchaTag;

        private bool success = false;

        [System.Serializable]
        public class EventsSettings
        {
            public int maxFails = 3;
            public int maxCaptchaResets = 2;
            public float timeoutSeconds = 30f;
        }

        [System.Serializable]
        public class Debugging
        {
            public int failResetCounter = 0;
        }

        public CaptchaEvents captchaEvents = new CaptchaEvents();
        public EventsSettings eventsSettings = new EventsSettings();
        public Debugging debugging = new Debugging();

        private int currentFails = 0, failResetCounter = 0;
        private float counter = 0;

        [HideInInspector] public List<Puzzle> puzzles = new List<Puzzle>();

        public float CurrentFailResetCounter
        {
            get
            {
                return failResetCounter;
            }
        }

        public bool Success
        {
            get
            {
                return success;
            }
            set
            {
                success = value;
            }
        }

        public void RegisterPuzzle(Puzzle puzzle)
        {
            puzzles.Add(puzzle);
        }

        public void Confirm()
        {
            bool isAnyFalse = false;

            foreach (Puzzle p in puzzles)
            {
                if (!p.isPrepared)
                    isAnyFalse = true;
            }

            if (!isAnyFalse)
            {
                captchaEvents.onCaptchaSuccessEvent.Invoke();
                Success = true;
                SendCaptchaResultToServer(true);
                
            }
            else
            {
                Fail();
            }
        }

        public void Fail()
        {
            currentFails++;
            SendCaptchaResultToServer(false);
            if (currentFails > eventsSettings.maxFails)
            {
                captchaEvents.onTooManyFailsEvent.Invoke();
                currentFails = 0;
                failResetCounter++;
            }

            if(failResetCounter >= eventsSettings.maxCaptchaResets)
            {
                captchaEvents.onTooManyResetsEvent.Invoke();
            }
        }

        private void Update()
        {
            counter += Time.deltaTime;

            if (counter >= eventsSettings.timeoutSeconds)
            {
                captchaEvents.onTimeoutEvent.Invoke();
                counter = 0;
            }

            debugging.failResetCounter = failResetCounter;
        }
        public void SendCaptchaResultToServer(bool captchaPassed2) {
            ObscuredString funcName = "captchaResult1A";

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                var request = new ExecuteCloudScriptRequest
                {
                    FunctionName = funcName,
                    GeneratePlayStreamEvent = true,
                    RevisionSelection = PlayfabDataManager.Instance.GetCloudRevision(),
                    FunctionParameter = new { captchaPassed = captchaPassed2 }
                };
                // PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnTransactionDateGet, OnTransactionDateGetError);
                PlayFabClientAPI.ExecuteCloudScript(request, result => {
                    if (result.Error != null) {
                        if (captchaPassed2)
                        {
                            this.gameObject.SetActive(false);
                        }
                        return;
                    }

                    if (captchaPassed2)
                    {
                        this.gameObject.SetActive(false);
                    }

                    //Debug.Log("Server responded: " + result.FunctionResult.ToString());
                }, error => {
                    this.gameObject.SetActive(false);
                    //                   Debug.LogError("Error calling cloud script: " + error.GenerateErrorReport());
                });
            }
        }
        


    }

}