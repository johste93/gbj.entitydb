
using UnityEditor;
using UnityEngine;

namespace GBJ.EntityDB.Editor
{
    public static class TableViewStyles
    {
        internal const float LineHeight = 21;
        internal const float StandardHorizontalSpacing = 3;

        internal const float LeftMargin = 40;
        internal const float RightMargin = 24;

        internal const float ScrollBarWidth = 12;

        internal static Color HighlightRowColor = new Color(1f, 1f, 1f, 0.3f);
        internal static Color EditorLightBackgroundColor = new Color(0.2196078f, 0.2196078f, 0.2196078f, 1);
        internal static Color EditorBackgroundColor = new Color(0.16470588235f, 0.16470588235f, 0.16470588235f, 1);
        internal static Color AlternateRowColor = new Color(1f, 1f, 1f, 0.025f);
  

        private static GUIStyle _centeredLabelStyle;
        internal static GUIStyle CenteredLabelStyle
        {
            get
            {
                if (_centeredLabelStyle == null)
                {
                    _centeredLabelStyle = new GUIStyle(EditorStyles.label);
                    _centeredLabelStyle.alignment = TextAnchor.UpperCenter;
                }

                return _centeredLabelStyle;
            }
        }

        private static GUIStyle _paginationNextPreviousButtonStyle;
        internal static GUIStyle PaginationNextPreviousButtonStyle
        {
            get
            {
                if (_paginationNextPreviousButtonStyle == null)
                {
                    _paginationNextPreviousButtonStyle = new GUIStyle(GUI.skin.button);
                    _paginationNextPreviousButtonStyle.fontSize = 15;
                }

                return _paginationNextPreviousButtonStyle;
            }
        }

        private static GUIStyle _paginationFirstLastButtonStyle;
        internal static GUIStyle PaginationFirstLastButtonStyle
        {
            get
            {
                if (_paginationFirstLastButtonStyle == null)
                {
                    _paginationFirstLastButtonStyle = new GUIStyle(GUI.skin.button);
                    _paginationFirstLastButtonStyle.fontSize = 22;
                }

                return _paginationFirstLastButtonStyle;
            }
        }
    }
}