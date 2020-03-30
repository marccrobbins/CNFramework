using System;
using System.Collections.Generic;
using UnityEngine;

namespace CNFramework
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class FilteredKeycodeAttribute : PropertyAttribute
    {
        public readonly string[] filter;

        public FilteredKeycodeAttribute(string filter)
        {
            this.filter = new [] {filter};
        }

        public FilteredKeycodeAttribute(params string[] filter)
        {
            this.filter = filter;
        }
    }
    
    [UnityEditor.CustomPropertyDrawer(typeof(FilteredKeycodeAttribute))]
    public class FilteredKeycodeAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            var filteredKeycode = (FilteredKeycodeAttribute) attribute;
            var keycodeNames = Enum.GetNames(typeof(KeyCode));
            var filteredNames = new List<string>() {"None"};

            for (var i = 0; i < keycodeNames.Length; i++)
            {
                var name = keycodeNames[i];

                foreach (var filter in filteredKeycode.filter)
                {
                    if (name.Contains(filter))
                    {
                        filteredNames.Add(name);
                        break;
                    }
                }
            }

            var propertyEnumValue = (KeyCode) property.intValue;
            var selectedIndex = filteredNames.IndexOf(propertyEnumValue.ToString());
            var index = UnityEditor.EditorGUI.Popup(position, label.text, selectedIndex, filteredNames.ToArray());

            if (selectedIndex == index) return;
            
            var selectedName = filteredNames[index];
            if (!Enum.TryParse(selectedName, out KeyCode code))
            {
                Debug.LogWarningFormat("Could not parse {0} as KeyCode value", selectedName);
                return;
            }

            //Assign enum index
            var codeInt = (int) code;
            property.intValue = codeInt;
        }
    }
}
