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
        protected virtual string ConstantPath => $"Constants/";
        protected virtual string GetPropertyConstantName(T entity) => string.Empty;

        protected abstract int maxColumnCount { get; }
        public float columnWidth;

        protected bool descending;
        protected string sortingColumn;

        protected Vector2 scrollPosition;
        protected Vector2 mousePos;
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

        protected virtual void Update()
        {
            if (!safeAreaContainsMouse)
                return;
            
            Repaint();
        }

        private bool safeAreaContainsMouse;
        
        protected virtual void OnGUI()
        {
            safeAreaContainsMouse = Screen.safeArea.Contains(Event.current.mousePosition);
            
            float totalColumnWidth = position.width - TableViewStyles.RightMargin - TableViewStyles.LeftMargin - (TableViewStyles.StandardHorizontalSpacing * horizontalColumnCount) - (TableViewStyles.StandardHorizontalSpacing * 3) - 1 - TableViewStyles.ScrollBarWidth;
            columnWidth = totalColumnWidth / horizontalColumnCount;
            
            mousePos = Event.current.mousePosition;
            mousePos.y += scrollPosition.y - TableViewStyles.LineHeight;

            GUILayout.Space(TableViewStyles.LineHeight);
            
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
                var rect = EditorGUILayout.BeginHorizontal();
                rect.height += EditorGUIUtility.standardVerticalSpacing;
                rect.y -= (EditorGUIUtility.standardVerticalSpacing * 0.5f);
                
                Color color = rect.Contains(mousePos) ? TableViewStyles.HighlightRowColor : (i % 2 == 0 ? Color.clear: TableViewStyles.AlternateRowColor);
                
                GUI.color = color;
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = Color.white;
                
                if (GUILayout.Button("Copy", GUILayout.Width(TableViewStyles.LeftMargin)))
                {
                    EditorGUIUtility.systemCopyBuffer = entry.Key;
                    Debug.Log($"{entry.Key} copied to clipboard");
                }

                DrawRow(entry.Value, UnmodifiedTable.GetById(((Entity) entry.Value).Id), true);

                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(TableViewStyles.RightMargin)))
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
            GUILayout.Space(TableViewStyles.LineHeight);
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.green;
            if (GUILayout.Button("+", GUILayout.Width(TableViewStyles.LeftMargin), GUILayout.Height(18)))
            {
                Table.Insert(@new);
                RefreshDictionary();
                CreateNewEntry();
            }
            
            GUI.color = Color.white;

            DrawRow(@new, @new, false);
            EditorGUILayout.EndHorizontal();
        }

        private bool DrawSortingColumnButton(float xPos, string label, float width)
        {
            Rect rect = new Rect(xPos, 0, width, TableViewStyles.LineHeight);
            bool clicked = GUI.Button(rect, sortingColumn == label ? $"{label} {(@descending ? "???" : "???")}" : label);
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
            DrawBackground(0, TableViewStyles.LineHeight + EditorGUIUtility.standardVerticalSpacing, TableViewStyles.EditorLightBackgroundColor);
            columnXPos = TableViewStyles.StandardHorizontalSpacing + -scrollPosition.x;
            DrawColumn(columnXPos,"#", x => ((Entity) x.Value).Index, TableViewStyles.LeftMargin);
            columnXPos += TableViewStyles.LeftMargin + TableViewStyles.StandardHorizontalSpacing;
            DrawColumnNames();
        }

        private void DrawBackground(float y, float height, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(new Rect(new Vector2(0, y), new Vector2(position.width, height)), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        protected void DrawFooter()
        {
            DrawBackground(position.height - TableViewStyles.LineHeight - EditorGUIUtility.standardVerticalSpacing, TableViewStyles.LineHeight + EditorGUIUtility.standardVerticalSpacing, TableViewStyles.EditorBackgroundColor);

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
            float buttonHeight = TableViewStyles.LineHeight - EditorGUIUtility.standardVerticalSpacing;

            GUI.enabled = pageIndex > 0;
            if (GUILayout.Button("???", TableViewStyles.PaginationFirstLastButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = 0;
            if (GUILayout.Button("???", TableViewStyles.PaginationNextPreviousButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = Mathf.Max(0, pageIndex - 1);
            GUI.enabled = true;

            GUILayout.Label($"Page {pageIndex}", TableViewStyles.CenteredLabelStyle, GUILayout.Width(100));

            GUI.enabled = pageIndex < numberOfPages;
            if (GUILayout.Button("???", TableViewStyles.PaginationNextPreviousButtonStyle, GUILayout.Height(buttonHeight)))
                pageIndex = Mathf.Min(pageIndex + 1, numberOfPages);
            if (GUILayout.Button("???", TableViewStyles.PaginationFirstLastButtonStyle, GUILayout.Height(buttonHeight)))
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
            columnXPos += columnWidth + TableViewStyles.StandardHorizontalSpacing;
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

            string path = Path.Combine(Application.dataPath, ConstantPath);
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, $"{className}.cs"), code);
        }
    }
}