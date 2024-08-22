// (c) Copyright VinforLab Team. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaCorrectImage : Puzzle
    {
        public Transform container;
        public Text info;
        public string infoText = "Select {name} to solve the puzzle";
        public SpriteElement[] sprites;

        [HideInInspector]
        public SpriteElement choosenSprite;

        [System.Serializable]
        public class SpriteElement
        {
            public Sprite sprite;
            public string name;
        }

        public void Refresh()
        {
            CaptchaCorrectImageButton[] puzzles = container.GetComponentsInChildren<CaptchaCorrectImageButton>();
            List<SpriteElement> available = new List<SpriteElement>(sprites);
            List<SpriteElement> choosen = new List<SpriteElement>();

            foreach (CaptchaCorrectImageButton p in puzzles)
            {
                SpriteElement s = available[Random.Range(0, available.Count)];
                p.icon.overrideSprite = s.sprite;
                available.Remove(s);
                choosen.Add(s);
                p.captchaCorrectImage = this;
            }

            choosenSprite = choosen[Random.Range(0, choosen.Count)];
            info.text = infoText.Replace("{name}", choosenSprite.name);
        }

        public void Confirm(Sprite yourAnswer)
        {
            if(yourAnswer == choosenSprite.sprite)
            {
                isPrepared = true;
                captcha.Confirm();
            }
            else
            {
                Fail();
            }
        }

        public void Fail()
        {
            captcha.Fail();
        }

        private void Start()
        {
            Refresh();
        }
    }
}