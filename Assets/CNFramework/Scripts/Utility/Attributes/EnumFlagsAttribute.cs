﻿using UnityEngine;

namespace CNFramework
{
    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute() { }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            property.intValue = UnityEditor.EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}