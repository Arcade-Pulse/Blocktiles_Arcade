// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEngine.UI;

namespace VinforlabTeam.VCaptcha
{
    public class CaptchaMath : Puzzle
    {
        public InputField inputAnswer;
        public Text questionLabel;
        public RectTransform bars;
        public int[] numberSetAddition = {
            1,2,3,4,5,6,7,8,9
    };
        public int[] numberSetSubtraction = {
            1,2,3,4,5,6,7,8,9
    };
        public int[] numberSetMultiplication = {
            2, 3, 4, 5, 9
    };
        public int[] numberSetDivision = {
            1,2,4,8,16
    };

        public ExpressionType expressionType;

        private int n1, n2, answer;

        public enum ExpressionType
        {
            ADDITION,
            SUBTRACTION,
            DIVISION,
            MULTIPLICATION,
            RANDOM
        }

        public void Refresh()
        {
            int[] numbers;

            ExpressionType e = expressionType;

            if (expressionType == ExpressionType.RANDOM)
            {
                ExpressionType[] types = {
                 ExpressionType.ADDITION,
                 ExpressionType.SUBTRACTION,
                 ExpressionType.MULTIPLICATION,
                 ExpressionType.DIVISION
            };

                e = types[Random.Range(0, types.Length)];
            }

            if (e == ExpressionType.ADDITION)
            {
                numbers = numberSetAddition;

                n1 = numbers[Random.Range(0, numbers.Length)];
                n2 = numbers[Random.Range(0, numbers.Length)];

                questionLabel.text = n1 + " + " + n2;
                answer = n1 + n2;
            }

            if (e == ExpressionType.SUBTRACTION)
            {
                numbers = numberSetSubtraction;

                n1 = numbers[Random.Range(0, numbers.Length)];
                n2 = numbers[Random.Range(0, numbers.Length)];

                questionLabel.text = n1 + " - " + n2;
                answer = n1 - n2;
            }

            if (e == ExpressionType.DIVISION)
            {
                numbers = numberSetDivision;

                n1 = numbers[Random.Range(0, numbers.Length)];
                n2 = numbers[Random.Range(0, numbers.Length)];

                int n1_old = n1;

                if (n1 < n2)
                {
                    n1 = n2;
                    n2 = n1_old;
                }

                questionLabel.text = n1 + " / " + n2;
                answer = n1 / n2;
            }

            if (e == ExpressionType.MULTIPLICATION)
            {
                numbers = numberSetMultiplication;

                n1 = numbers[Random.Range(0, numbers.Length)];
                n2 = numbers[Random.Range(0, numbers.Length)];

                questionLabel.text = n1 + " x " + n2;

                answer = n1 * n2;
            }

            bars.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        public void Check()
        {
            if (string.IsNullOrEmpty(inputAnswer.text.Trim()))
            {
                isPrepared = false;
                return;
            }

            if (int.Parse(inputAnswer.text) == answer)
            {
                isPrepared = true;
            }
            else
            {
                isPrepared = false;
            }
        }

        private void Start()
        {
            inputAnswer.onValueChanged.AddListener(delegate (string val)
            {
                Check();
            });

            Refresh();
        }
    }
}