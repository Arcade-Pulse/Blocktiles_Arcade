// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaSimpleCode : Puzzle
    {
        public InputField inputField;
        public Text codeText;

        private string code;

        [System.Serializable]
        public class Settings
        {
            public int codeLength = 6;
            public string textTemplate = "Enter captcha code: {code}";
        }

        public Settings settings = new Settings();

        private string GenerateCode()
        {
            string code = "";
            for (int i = 0; i < settings.codeLength; i++)
            {
                code += Random.Range(0, 9);
            }
            return code;
        }

        public void Check()
        {
            if (code == inputField.text)
            {
                isPrepared = true;
            }
            else
            {
                isPrepared = false;
            }
        }

        public void Refresh()
        {
            code = GenerateCode();
            codeText.text = settings.textTemplate.Replace("{code}", "<i>" + code + "</i>");
        }

        void Start()
        {
            inputField.onValueChanged.AddListener(delegate (string val)
            {
                Check();
            });

            Refresh();
        }

    }
}