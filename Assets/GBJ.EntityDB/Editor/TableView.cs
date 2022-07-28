using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GBJ.EntityDB.Editor
{
    public abstract class TableView<T> : EditorWindow where T : Entity
    {
        protected Table<T> Table;
        protected Table<T> UnmodifiedTable;
        protected Dictionary<string, T> tableDictionary;
        protected T @new;

        protected virtual bool GenerateConstants => false;
        protected virtual string GetPropertyConstantName(T entity) => string.Empty;

        protected abstract int maxColumnCount { get; }
        public float columnWidth;

        protected bool descending;
        protected string sortingColumn;

        protected Vector2 scrollPosition;
        protected int selectedRow;

        private int pageIndex = 0;
        private int entitiesPrPage = 100;
        private int entitiesPrPageIndex = 3;
        private string[] entitiesPrPageOptions = {"10", "25", "50", "100", "200"};
        
        private int horizontalColumnCount = int.MaxValue;
        private const int minHorizontalColumnCount = 2;

        protected static void Init<T>() => GetWindow(typeof(T)).Show();

        protected abstract void DrawColumnNames();

        protected abstract void CreateNewEntry();

        protected abstract void DrawRow(T entry, T unmodified, bool changeColorIfChanged = false);

        protected virtual void OnEnable()
        {
            horizontalColumnCount = EditorPrefs.GetInt($"{typeof(T).Name}_HorizontalColumnCount", maxColumnCount);
            entitiesPrPageIndex = EditorPrefs.GetInt($"{typeof(T).Name}_EntitiesPrPage", 3);
            entitiesPrPage = int.Parse(entitiesPrPageOptions[entitiesPrPageIndex]);
            
            CreateNewEntry();
            Table = new Table<T>();

            if (Table.Exists())
            {
                Table.Load();
            }
            else
            {
                Table.Create();
                Table.Save();
            }

            UnmodifiedTable = new Table<T>();
            UnmodifiedTable.Load();
            RefreshDictionary();
        }

        protected virtual void OnGUI()
        {
            Vector2 mousePos = Event.current.mousePosition;
            mousePos.y += scrollPosition.y - EditorGUIUtility.standardVerticalSpacing;

            selectedRow = Mathf.FloorToInt(mousePos.y / TableViewStyles.lineHeight);
            float totalColumnWidth = position.width - TableViewStyles.rightMargin - TableViewStyles.leftMargin - (TableViewStyles.standardHorizontalSpacing * horizontalColumnCount) - (TableViewStyles.standardHorizontalSpacing * 3) - 1 - TableViewStyles.scrollBarWidth;
            columnWidth = totalColumnWidth / horizontalColumnCount;
            
            int rowsVisibleOnScreen = tableDictionary.Count;

            if (selectedRow > 0 && selectedRow <= rowsVisibleOnScreen)
            {
                GUI.DrawTexture(new Rect(new Vector2(0, (selectedRow * TableViewStyles.lineHeight) - scrollPosition.y + EditorGUIUtility.standardVerticalSpacing), new Vector2(position.width, TableViewStyles.lineHeight)), Texture2D.whiteTexture);
            }
            
            GUILayout.Space(TableViewStyles.lineHeight);
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            ListEntries();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            DrawHeader();
            DrawFooter();
        }

        protected void ListEntries()
        {
            int startIndex = pageIndex * entitiesPrPage;
            for (int i = startIndex; i < Mathf.Min(startIndex + entitiesPrPage, tableDictionary.Count); i++)
            {
                var entry = tableDictionary.ElementAt(i);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Copy", GUILayout.Width(TableViewStyles.leftMargin)))
                {
                    EditorGUIUtility.systemCopyBuffer = entry.Key;
                    Debug.Log($"{entry.Key} copied to clipboard");
                }

                DrawRow(entry.Value, UnmodifiedTable.GetById(((Entity) entry.Value).Id), true);

                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(TableViewStyles.rightMargin)))
                {
                    Table.Remove(entry.Value);
                    RefreshDictionary();
                }

                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;
            }

            DrawNewEntry();
        }

        protected void DrawNewEntry()
        {
            GUILayout.Space(TableViewStyles.lineHeight);
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.green;
            if (GUILayout.Button("+", GUILayout.Width(TableViewStyles.leftMargin), GUILayout.Height(18)))
            {
                Table.Insert(@new);
                RefreshDictionary();
                CreateNewEntry();
            }

            //GUILayout.Space(2);
            GUI.color = Color.white;

            DrawRow(@new, @new, false);
            EditorGUILayout.EndHorizontal();
        }

        private bool DrawSortingColumnButton(float xPos, string label, float width)
        {
            Rect rect = new Rect(xPos, 0, width, TableViewStyles.lineHeight);
            bool clicked = GUI.Button(rect, sortingColumn == label ? $"{label} {(@descending ? "↓" : "↑")}" : label);
            if (clicked)
            {
                descending = label == sortingColumn && !@descending;
                sortingColumn = label;
                pageIndex = 0;
            }

            return clicked;
        }

        private float columnXPos;
        protected void DrawHeader()
        {
            columnXPos = TableViewStyles.standardHorizontalSpacing + -scrollPosition.x;
            DrawColumn(columnXPos,"#", x => ((Entity) x.Value).Index, TableViewStyles.leftMargin);
            columnXPos += TableViewStyles.leftMargin + TableViewStyles.standardHorizontalSpacing;
            DrawColumnNames();
        }

        private void DrawRowBackground(float y, float height, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(new Rect(new Vector2(0, y), new Vector2(position.width, height)), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        protected void DrawFooter()
        {
            DrawRowBackground(position.height - TableViewStyles.lineHeight - EditorGUIUtility.standardVerticalSpacing, TableViewStyles.lineHeight + EditorGUIUtility.standardVerticalSpacing, TableViewStyles.editorBackgroundColor);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button("Save", GUILayout.MinWidth(100)))
                    {
                        SaveAndReloadTable();
                    }

                    GUI.color = Color.white;

                    if (GUILayout.Button("Reload", GUILayout.MinWidth(100)))
                    {
                        Reload();
                    }

                    if (GUILayout.Button(
                            $"Reveal in {(Application.platform == RuntimePlatform.WindowsEditor ? "Explorer" : "Finder")}",
                            GUILayout.MinWidth(100)))
                    {
                        RevealInFinder();
                    }

                    GUILayout.FlexibleSpace();

                    DrawPageView();

                    GUILayout.FlexibleSpace();

                    int columnCount = EditorGUILayout.IntSlider("Horizontal Column Count", horizontalColumnCount, minHorizontalColumnCount, maxColumnCount);
                    if (horizontalColumnCount != columnCount)
                    {
                        horizontalColumnCount = columnCount;
                        EditorPrefs.SetInt($"{typeof(T).Name}_HorizontalColumnCount", horizontalColumnCount);
                    }

                    int index = EditorGUILayout.Popup("Entities pr page", entitiesPrPageIndex, entitiesPrPageOptions);
                    if (entitiesPrPageIndex != index)
                    {
                        entitiesPrPageIndex = index;
                        entitiesPrPage = int.Parse(entitiesPrPageOptions[entitiesPrPageIndex]);
                        EditorPrefs.SetInt($"{typeof(T).Name}_EntitiesPrPage", entitiesPrPageIndex);
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }

        protected void DrawPageView()
        {
            int numberOfPages = Mathf.FloorToInt(tableDictionary.Count / entitiesPrPage);
            float buttonHeight = TableViewStyles.lineHeight - EditorGUIUtility.standardVerticalSpacing;

            GUI.enabled = pageIndex > 0;
            if (GUILayout.Button("⇤", TableViewStyles.paginationFirstLastButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = 0;
            if (GUILayout.Button("⟵", TableViewStyles.paginationNextPreviousButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = Mathf.Max(0, pageIndex - 1);
            GUI.enabled = true;

            GUILayout.Label($"Page {pageIndex}", TableViewStyles.centeredLabelStyle, GUILayout.Width(100));

            GUI.enabled = pageIndex < numberOfPages;
            if (GUILayout.Button("⟶", TableViewStyles.paginationNextPreviousButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = Mathf.Min(pageIndex + 1, numberOfPages);
            if (GUILayout.Button("⇥", TableViewStyles.paginationFirstLastButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = numberOfPages;
            GUI.enabled = true;
        }

        protected void RevealInFinder() => Table.RevealInFinder();

        protected void SaveAndReloadTable()
        {
            Table.Save();
            UpdateConstants();
            Reload();
        }

        protected void Reload()
        {
            AssetDatabase.Refresh();
            Table.Load();
            UnmodifiedTable.Load();
            RefreshDictionary();
            sortingColumn = string.Empty;
            CreateNewEntry();
            pageIndex = 0;
        }

        protected void RefreshDictionary() => tableDictionary = Table.All().ToDictionary(x => ((Entity) x).Id, x => x);

        protected void DrawColumn(string label, Func<KeyValuePair<string, T>, dynamic> Value)
        {
            DrawColumn(columnXPos, label, Value, columnWidth);
            columnXPos += columnWidth + TableViewStyles.standardHorizontalSpacing;
        }
        
        protected void DrawColumn(float xPos, string label, Func<KeyValuePair<string, T>, dynamic> Value, float width)
        {
            if (DrawSortingColumnButton(xPos, label, width))
            {
                if (descending)
                    tableDictionary = tableDictionary.OrderByDescending(Value).ToDictionary(x => x.Key, x => x.Value);
                else
                    tableDictionary = tableDictionary.OrderBy(Value).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        private void UpdateConstants()
        {
            if (!GenerateConstants)
                return;

            string className = typeof(T).Name.Replace("Entity", "");

            string pattern = @"[a-zA-Z0-9_]";
            string code = $"public class {className}\n{{";

            foreach (var entry in Table.All())
            {
                string propertyName = string.Concat(Regex.Matches(GetPropertyConstantName(entry), pattern).Select(x => string.Concat(x.Groups.Select(y => y.Value))).ToList());
                if (propertyName.Length > 0)
                {
                    code += $"\n\tpublic const string {propertyName} = \"{entry.Id}\";";
                }
            }

            code += "\n}";
            
            string directory = $"{Application.dataPath}/Constants/";
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, $"{className}.cs"), code);
        }
    }
}