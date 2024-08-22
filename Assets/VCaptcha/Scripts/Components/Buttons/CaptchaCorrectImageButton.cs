// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaCorrectImageButton : MonoBehaviour
    {
        public Image icon;

        [HideInInspector]
        public CaptchaCorrectImage captchaCorrectImage;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(delegate ()
            {
                captchaCorrectImage.Confirm(icon.overrideSprite);
            });
        }
    }
}