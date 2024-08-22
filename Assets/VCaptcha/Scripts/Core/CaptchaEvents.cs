// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine.Events;

namespace VinforlabTeam.VCaptcha
{
    [System.Serializable]
    public class CaptchaEvents
    {
        public UnityEvent onCaptchaSuccessEvent, onTooManyFailsEvent, onTimeoutEvent, onTooManyResetsEvent;
    }
}