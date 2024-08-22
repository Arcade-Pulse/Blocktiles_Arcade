// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;

namespace VinforlabTeam.VCaptcha
{
    public class Puzzle : MonoBehaviour
    {
        public Captcha captcha;
        [HideInInspector] public bool isPrepared = false;

        private void Awake()
        {
            captcha.RegisterPuzzle(this);
        }
    }
}