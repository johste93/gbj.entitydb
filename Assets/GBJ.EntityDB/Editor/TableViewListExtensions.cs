#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using GBJ.EntityDB.AssetReferenceHolders;
using GBJ.EntityDB.CustomAssetReferencers;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AssetReferenceAtlasedSprite = UnityEngine.AddressableAssets.AssetReferenceAtlasedSprite;

namespace GBJ.EntityDB.Editor
{
    public static class TableViewListExtensions
    {
        public static void DrawCellList<T>(this TableView<T> view, Getter<List<string>> getter, Setter<List<string>> setter, bool changeColorIfChanged = false, Getter<List<string>?> oldValue = null) where T : Entity
        {
            List<string> list = getter();
            List<string> oldList = oldValue();

            GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
            for (int i = 0; i < list.Count; i++)
            {
                if (changeColorIfChanged && oldValue != null)
                    GUI.color = oldList != null && i < oldList.Count && list[i].Equals(oldList[i]) ? Color.white : Color.cyan;

                GUILayout.BeginHorizontal();
                list[i] = EditorGUILayout.TextField(list[i], GUILayout.Width(view.columnWidth - TableViewStyles.standardHorizontalSpacing - TableViewStyles.rightMargin - 0.5f));

                GUI.color = Color.white;
                if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.rightMargin)))
                    list.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
                list.Add(string.Empty);

            GUILayout.EndVertical();

            setter(list);
        }
        
        public static void DrawCellList<T>(this TableView<T> view, Getter<List<bool>> getter, Setter<List<bool>> setter, bool changeColorIfChanged = false, Getter<List<bool>?> oldValue = null) where T : Entity
        {
            List<bool> list = getter();
            List<bool> oldList = oldValue();

            GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
            for (int i = 0; i < list.Count; i++)
            {
                if (changeColorIfChanged && oldValue != null)
                    GUI.color = oldList != null && i < oldList.Count && list[i].Equals(oldList[i]) ? Color.white : Color.cyan;

                GUILayout.BeginHorizontal();
                list[i] = EditorGUILayout.Toggle(list[i], GUILayout.Width(view.columnWidth - TableViewStyles.standardHorizontalSpacing - TableViewStyles.rightMargin - 0.5f));

                GUI.color = Color.white;
                if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.rightMargin)))
                    list.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
                list.Add(false);

            GUILayout.EndVertical();

            setter(list);
        }
        
        public static void DrawCellList<T>(this TableView<T> view, Getter<List<int>> getter, Setter<List<int>> setter, bool changeColorIfChanged = false, Getter<List<int>?> oldValue = null) where T : Entity
        {
            List<int> list = getter();
            List<int> oldList = oldValue();

            GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
            for (int i = 0; i < list.Count; i++)
            {
                if (changeColorIfChanged && oldValue != null)
                    GUI.color = oldList != null && i < oldList.Count && list[i].Equals(oldList[i]) ? Color.white : Color.cyan;

                GUILayout.BeginHorizontal();
                list[i] = EditorGUILayout.IntField(list[i], GUILayout.Width(view.columnWidth - TableViewStyles.standardHorizontalSpacing - TableViewStyles.rightMargin - 0.5f));

                GUI.color = Color.white;
                if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.rightMargin)))
                    list.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
                list.Add(0);

            GUILayout.EndVertical();

            setter(list);
        }
        
        public static void DrawCellList<T>(this TableView<T> view, Getter<List<float>> getter, Setter<List<float>> setter, bool changeColorIfChanged = false, Getter<List<float>?> oldValue = null) where T : Entity
        {
            List<float> list = getter();
            List<float> oldList = oldValue();

            GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
            for (int i = 0; i < list.Count; i++)
            {
                if (changeColorIfChanged && oldValue != null)
                    GUI.color = oldList != null && i < oldList.Count && list[i].Equals(oldList[i]) ? Color.white : Color.cyan;

                GUILayout.BeginHorizontal();
                list[i] = EditorGUILayout.FloatField(list[i], GUILayout.Width(view.columnWidth - TableViewStyles.standardHorizontalSpacing - TableViewStyles.rightMargin - 0.5f));

                GUI.color = Color.white;
                if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.rightMargin)))
                    list.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
                list.Add(0);

            GUILayout.EndVertical();

            setter(list);
        }
        
        public static void DrawCellList<T, TEnum>(this TableView<T> view, Getter<List<TEnum>> getter, Setter<List<TEnum>> setter, Type enumType, bool changeColorIfChanged = false, Getter<List<TEnum>?> oldValue = null) where T : Entity where TEnum : Enum 
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            List<int> list = getter().Cast<int>().ToList();
            List<int> oldList = oldValue().Cast<int>().ToList();

            GUILayout.BeginVertical(GUILayout.Width(view.columnWidth));
            for (int i = 0; i < list.Count; i++)
            {
                if (changeColorIfChanged && oldValue != null)
                    GUI.color = oldList != null && i < oldList.Count && list[i].Equals(oldList[i]) ? Color.white : Color.cyan;

                GUILayout.BeginHorizontal();
                list[i] = EditorGUILayout.Popup(list[i] , Enum.GetNames(enumType), GUILayout.Width(view.columnWidth - TableViewStyles.standardHorizontalSpacing - TableViewStyles.rightMargin - 0.5f));

                GUI.color = Color.white;
                if (GUILayout.Button("-", GUILayout.Width(TableViewStyles.rightMargin)))
                    list.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUI.color = Color.white;
            if (GUILayout.Button("+", GUILayout.Width(view.columnWidth)))
                list.Add(0);

            GUILayout.EndVertical();

            setter(list.Cast<TEnum>().ToList());
        }
    }
}