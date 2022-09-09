using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBJ.EntityDB.Editor
{
	public static class TableViewConstantDropdownExtension
	{
		public static void DrawDropdown<T>(this TableView<T> view, Type constantType, Getter<string> getter, Setter<string> setter, bool changeColorIfChanged = false, Getter<string?> oldValue = null) where T : Entity
		{
			if (changeColorIfChanged && oldValue != null)
				GUI.color = oldValue() == getter() ? Color.white : Color.cyan;
            
			//Todo: Cache these?
			var properties = constantType.GetFields();
			string[] names = new string[properties.Length];
			List<string> values = new List<string>();

			for (int i = 0; i < properties.Length; i++)
			{
				names[i] = properties[i].Name;
				values.Add(properties[i].GetRawConstantValue().ToString());
			}

			int index = Mathf.Max(0, values.IndexOf(getter()));
			setter(values[EditorGUILayout.Popup(index, names, GUILayout.Width(view.columnWidth))]);
		}
	}
}