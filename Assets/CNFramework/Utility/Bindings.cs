using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CNFramework.Utility
{
	[InitializeOnLoad]
	public class Bindings : MonoBehaviour
	{
		private const string MENU_ITEM_NAME = "CNFramework/ValidateBindings";
		private const string PREF_NAME = "CNFramework_Binding_IsValid";
		private static bool isBindingValid;

		static Bindings()
		{
			EditorApplication.delayCall += ValidateBindings;
		}
		
		[MenuItem(MENU_ITEM_NAME)]
		static void ToggleUtilities()
		{
			isBindingValid = !isBindingValid;
			Menu.SetChecked(MENU_ITEM_NAME, isBindingValid);

			int newValue = (isBindingValid) ? 1 : 0;
			PlayerPrefs.SetInt(PREF_NAME, newValue);
			PlayerPrefs.Save();

			Debug.Log("Using required project settings: " + isBindingValid);
		}
		
		private static void ValidateBindings()
		{
			isBindingValid = EditorPrefs.GetInt(PREF_NAME, 1) != 0;
			Menu.SetChecked(MENU_ITEM_NAME, isBindingValid);
			
			if (!isBindingValid) return;
			SetBindings();
		}

		private static void SetBindings()
		{
			BindAxis(new Axis { name = "CNFramework_LeftHorizontal", sensitivity = 3, axis = 0 });
			BindAxis(new Axis { name = "CNFramework_LeftVertical", sensitivity = 3, axis = 1, invert = true });
			BindAxis(new Axis { name = "CNFramework_RightHorizontal", sensitivity = 3, axis = 3 });
			BindAxis(new Axis { name = "CNFramework_RightVertical", sensitivity = 3, axis = 4, invert = true });
			BindAxis(new Axis { name = "CNFramework_LeftTrigger", axis = 8 });
			BindAxis(new Axis { name = "CNFramework_RightTrigger", axis = 9 });
			BindAxis(new Axis { name = "CNFramework_LeftGrip", axis = 10});
			BindAxis(new Axis { name = "CNFramework_RightGrip", axis = 11});
			BindAxis(new Axis { name = "CNFramework_LeftIndexCapacitance", axis = 19});
			BindAxis(new Axis { name = "CNFramework_RightIndexCapacitance", axis = 20});
			BindAxis(new Axis { name = "CNFramework_LeftMiddleCapacitance", axis = 21});
			BindAxis(new Axis { name = "CNFramework_RightMiddleCapacitance", axis = 22});
			BindAxis(new Axis { name = "CNFramework_LeftRingCapacitance", axis = 23});
			BindAxis(new Axis { name = "CNFramework_RightRingCapacitance", axis = 24});
			BindAxis(new Axis { name = "CNFramework_LeftPinkyCapacitance", axis = 25});
			BindAxis(new Axis { name = "CNFramework_RightPinkyCapacitance", axis = 26});
		}
		
		private class Axis
		{
			public string name = String.Empty;
			public string descriptiveName = String.Empty;
			public string descriptiveNegativeName = String.Empty;
			public string negativeButton = String.Empty;
			public string positiveButton = String.Empty;
			public string altNegativeButton = String.Empty;
			public string altPositiveButton = String.Empty;
			public float gravity = 0.0f;
			public float dead = 0.001f;
			public float sensitivity = 1.0f;
			public bool snap = false;
			public bool invert = false;
			public int type = 2;
			public int axis = 0;
			public int joyNum = 0;
		}

		private static void BindAxis(Axis axis)
		{
			SerializedObject serializedObject =
				new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
			SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

			SerializedProperty axisIter = axesProperty.Copy();
			axisIter.Next(true);
			axisIter.Next(true);
			while (axisIter.Next(false))
			{
				if (axisIter.FindPropertyRelative("m_Name").stringValue == axis.name)
				{
					// Axis already exists. Don't create binding.
					return;
				}
			}

			axesProperty.arraySize++;
			serializedObject.ApplyModifiedProperties();

			SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
			axisProperty.FindPropertyRelative("m_Name").stringValue = axis.name;
			axisProperty.FindPropertyRelative("descriptiveName").stringValue = axis.descriptiveName;
			axisProperty.FindPropertyRelative("descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
			axisProperty.FindPropertyRelative("negativeButton").stringValue = axis.negativeButton;
			axisProperty.FindPropertyRelative("positiveButton").stringValue = axis.positiveButton;
			axisProperty.FindPropertyRelative("altNegativeButton").stringValue = axis.altNegativeButton;
			axisProperty.FindPropertyRelative("altPositiveButton").stringValue = axis.altPositiveButton;
			axisProperty.FindPropertyRelative("gravity").floatValue = axis.gravity;
			axisProperty.FindPropertyRelative("dead").floatValue = axis.dead;
			axisProperty.FindPropertyRelative("sensitivity").floatValue = axis.sensitivity;
			axisProperty.FindPropertyRelative("snap").boolValue = axis.snap;
			axisProperty.FindPropertyRelative("invert").boolValue = axis.invert;
			axisProperty.FindPropertyRelative("type").intValue = axis.type;
			axisProperty.FindPropertyRelative("axis").intValue = axis.axis;
			axisProperty.FindPropertyRelative("joyNum").intValue = axis.joyNum;
			serializedObject.ApplyModifiedProperties();
		}
	}
}
