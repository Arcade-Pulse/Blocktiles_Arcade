// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace VinforlabTeam.VCaptcha
{
    public class RandomSprite : MonoBehaviour
    {
        public Sprite[] sprites;

        private Image m_Image;

        public bool randomizeRotation = false;

        public void Refresh()
        {
            Sprite random = sprites[Random.Range(0, sprites.Length)];
            m_Image.overrideSprite = random;
        }

        public Sprite RefreshAndGetSprite()
        {
            Sprite random = sprites[Random.Range(0, sprites.Length)];
            m_Image.overrideSprite = random;
            return random;
        }

        void Start()
        {
            m_Image = GetComponent<Image>();
            Refresh();
            if (randomizeRotation)
            {
                transform.Rotate(new Vector3(0,0, Random.Range(-360, 360)));
            }
        }


        void Update()
        {

        }
    }
}
