// (c) Copyright VinforLab Team. All rights reserved.

using System.Collections;
using UnityEngine;

namespace VinforlabTeam.VCaptcha
{
    public class DialogAnimation : MonoBehaviour
    {
        private float delay = 0f;

        Vector3 realDefaultScale;

        Vector3 defaultScale;
        Vector3 initialScale;

        public float speed = 6f;
        public float initialOffset = 0.6f;

        private float mOffset = 0.15f;

        bool start = false;

        private void Awake()
        {
            realDefaultScale = transform.localScale;
        }

        void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            transform.localScale = defaultScale;
            StopAllCoroutines();
            start = false;

        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            start = true;
        }

        void Init()
        {
            defaultScale = transform.localScale;
            defaultScale.x += mOffset;
            defaultScale.y += mOffset;
            defaultScale.z += mOffset;

            initialScale = defaultScale;
            initialScale.x -= initialOffset;
            initialScale.y -= initialOffset;
            initialScale.z -= initialOffset;

            transform.localScale = initialScale;

            StartCoroutine(Delay());
        }

        void FixedUpdate()
        {
            if (start)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, defaultScale, speed * Time.deltaTime);

                if (transform.localScale == defaultScale)
                {
                    defaultScale = realDefaultScale;
                }
            }
        }
    }
}