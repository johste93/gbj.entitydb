using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GBJ.EntityDB.Editor
{
	public static class TableViewConstantDropdownExtension
	{
		public static void DrawConstantDropdown<T>(this TableView<T> view, Type constantType, Getter<string> getter, Setter<string> setter, bool changeColorIfChanged = false, Getter<string?> oldValue = null) where T : Entity
			=> view.DrawConstantDropdown(constantType, getter, setter, changeColorIfChanged, oldValue, GUILayout.Width(view.columnWidth));

		public static void DrawConstantDropdown<T>(this TableView<T> view, Type constantType, Getter<string> getter, Setter<string> setter, bool changeColorIfChanged = false, Getter<string?> oldValue = null, params GUILayoutOption[] options) where T : Entity
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
			setter(values[EditorGUILayout.Popup(index, names, options)]);
		}
		
		public static void DrawConstantDropdownList<T>(this TableView<T> view, Type type, Getter<List<string>> getter, Setter<List<string>> setter, bool changeColorIfChanged = false, Getter<List<string>?> oldValue = null) where T : Entity
	    {
	        List<string> list = getter();
	        List<string> oldList = oldValue();

	        GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
	        for (int i = 0; i < list.Count; i++)
	        {
		        GUILayout.BeginHorizontal();
		        view.DrawConstantDropdown(type, () => list[i], x => list[i] = x, changeColorIfChanged, () => oldList == null || i >= oldList.Count ? string.Empty : oldList[i], GUILayout.MaxWidth(view.columnWidth - TableViewStyles.StandardHorizontalSpacing - TableViewStyles.RightMargin - 0.5f));
	            
	            if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.RightMargin)))
	                list.RemoveAt(i);

	            GUILayout.EndHorizontal();
	        }

	        GUI.color = Color.white;
	        if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
	            list.Add(string.Empty);

	        GUILayout.EndVertical();

	        setter(list);
	    }
	}
}