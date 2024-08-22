// (c) Copyright VinforLab Team. All rights reserved.

using System;
using UnityEngine;

namespace VinforlabTeam.VCaptcha
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class RangeExAttribute : PropertyAttribute
    {
        public readonly int min;
        public readonly int max;
        public readonly int step;
        public readonly int def;

        public RangeExAttribute(int min, int max, int step, int def)
        {
            this.min = min;
            this.max = max;
            this.step = step;
            this.def = def;
        }
    }
}