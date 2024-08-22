// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaRotate : Puzzle
    {
        public RectTransform picture;

        [System.Serializable]
        public class RotationSettings
        {
            [RangeEx(0, 360, 5, 30)]
            public float degreeSum = 30;
            [RangeEx(0, 360, 5, 0)]
            public float answerRotation = 0f;
            [RangeEx(8, 100, 1, 12)]
            public float rotationSmoothSpeed = 12f;

            [HideInInspector] public float currentRotation = -180f;
        }

        [System.Serializable]
        public class Debugging
        {
            public float currentRotation;
        }

        public RotationSettings rotationSettings = new RotationSettings();
        public Debugging debugging = new Debugging();

        private float GetRandomRotation()
        {
            int max = (int)Mathf.Floor(360 / rotationSettings.degreeSum);
            int value = (int)Mathf.Floor(Random.Range(0, max) * rotationSettings.degreeSum);
            while (value == rotationSettings.answerRotation)
            {
                value = (int)Mathf.Floor(Random.Range(0, max) * rotationSettings.degreeSum);
            }
            return value;
        }

        public void RotateLeft()
        {
            if (rotationSettings.currentRotation + rotationSettings.degreeSum >= 359)
            {
                if (rotationSettings.currentRotation == 360)
                {
                    rotationSettings.currentRotation = 0 + rotationSettings.degreeSum;
                }
                else
                {
                    rotationSettings.currentRotation = 0;
                }
            }
            else
            {
                rotationSettings.currentRotation += rotationSettings.degreeSum;
            }
        }

        public void RotateRight()
        {
            if (rotationSettings.currentRotation - rotationSettings.degreeSum <= 0)
            {
                if (rotationSettings.currentRotation == 0)
                {
                    rotationSettings.currentRotation = 360 - rotationSettings.degreeSum;

                }
                else
                {
                    rotationSettings.currentRotation = 360;
                }
            }
            else
            {
                rotationSettings.currentRotation -= rotationSettings.degreeSum;
            }
        }

        public void Refresh()
        {
            rotationSettings.currentRotation = GetRandomRotation();
            picture.transform.rotation = Quaternion.Euler(0, 0, rotationSettings.currentRotation);
        }

        private void Start()
        {
            Refresh();
        }

        public void Check()
        {
            if (Mathf.Floor(rotationSettings.currentRotation) == Mathf.Floor(rotationSettings.answerRotation))
            {
                isPrepared = true;
            }
            else
            {
                isPrepared = false;
            }
        }


        private void Update()
        {
            if (rotationSettings.currentRotation == 360)
            {
                rotationSettings.currentRotation = 0;
            }

            picture.transform.rotation = Quaternion.Lerp(picture.transform.rotation, Quaternion.Euler(0, 0, rotationSettings.currentRotation), Time.deltaTime * rotationSettings.rotationSmoothSpeed);

            debugging.currentRotation = rotationSettings.currentRotation;

            Check();
        }
    }
}