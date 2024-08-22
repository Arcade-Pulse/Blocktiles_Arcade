// (c) Copyright VinforLab Team. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace VinforlabTeam.VCaptcha
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RangeExAttribute))]
    internal sealed class RangeExDrawer : PropertyDrawer
    {
        private int value;

        bool isFirst = true;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rangeAttribute = (RangeExAttribute)base.attribute;

            if (isFirst)
            {
                value = rangeAttribute.def;
                isFirst = false;
            }

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                value = EditorGUI.IntSlider(position, label, value, rangeAttribute.min, rangeAttribute.max);

                value = (value / rangeAttribute.step) * rangeAttribute.step;
                property.intValue = value;
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                value = EditorGUI.IntSlider(position, label, value, rangeAttribute.min, rangeAttribute.max);
                value = (value / rangeAttribute.step) * rangeAttribute.step;
                property.floatValue = value;
            }
        }
    }
#endif
}