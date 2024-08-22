// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaSlider : Puzzle, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        private RectTransform m_RectTransform;
        private float m_Progress = 0f;

        public RectTransform sliderBar;

        [System.Serializable]
        public class Debugging
        {
            public float progress;
        }

        [System.Serializable]
        public class Settings
        {
            public bool resetOnRelease = false;
            [HideInInspector] public bool lockOnSuccess = true;
        }

        public Settings settings = new Settings();
        public Debugging debugging = new Debugging();

        public void OnDrag(PointerEventData eventData)
        {
            if (captcha.Success && settings.lockOnSuccess) return;

            transform.Translate(new Vector2(eventData.delta.x, 0));
            Vector3 clamp = transform.localPosition;
            clamp.x = Mathf.Clamp(clamp.x, -(sliderBar.rect.width / 2) + m_RectTransform.rect.width / 2, (sliderBar.rect.width / 2) - m_RectTransform.rect.width / 2);
            transform.localPosition = clamp;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (settings.resetOnRelease && !captcha.Success)
            {
                Vector3 pos = transform.localPosition;
                pos.x = -(sliderBar.rect.width / 2) + m_RectTransform.rect.width / 2;
                transform.localPosition = pos;
            }
        }

        float GetProgress()
        {
            return ((((100 * (transform.localPosition.x)) / ((sliderBar.rect.width / 2) - m_RectTransform.rect.width / 2)) + 100) / 200) * 100;
        }

        private void Start()
        {
            m_RectTransform = GetComponent<RectTransform>();
            if (captcha.captchaEvents == null)
                captcha.captchaEvents = GetComponent<CaptchaEvents>();

            captcha.captchaEvents.onCaptchaSuccessEvent.AddListener(delegate ()
            {
                GetComponent<UnityEngine.UI.Image>().color = new Color32(0x00, 0xFF, 0x00, 0xFF);
            });
        }

        private void Update()
        {
            m_Progress = Mathf.Clamp(GetProgress(), 0, 100);
            if (m_Progress >= 100 && !captcha.Success)
            {
                isPrepared = true;
                captcha.Confirm();
            }

            debugging.progress = m_Progress;
        }
    }
}