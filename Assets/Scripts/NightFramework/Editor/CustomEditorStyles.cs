using UnityEditor;
using UnityEngine;

// ========================
// Revision 2020.09.27
// ========================

public static class CustomEditorStyles
{
    public static Texture2D MakeColoredTexture(int width, int height, Color color)
    {
        var pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = color;

        var result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    public static Texture2D MakeGradientTexture(int width, int height, Color colorFrom, Color colorTo)
    {
        var pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
        {
            float d = (float)(i % width) / width;
            pix[i] = colorFrom * (1 - d) + colorTo * d;
        }

        var result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    private static GUIStyle _centeredObjectThumbStyle;
    public static GUIStyle CenteredObjectThumbStyle
    {
        get
        {
            if (_centeredObjectThumbStyle == null)
            {
                _centeredObjectThumbStyle = new GUIStyle(EditorStyles.objectFieldThumb);
                _centeredObjectThumbStyle.alignment = TextAnchor.MiddleCenter;
                _centeredObjectThumbStyle.imagePosition = ImagePosition.ImageAbove;
            }

            return _centeredObjectThumbStyle;
        }
    }

    private static GUIStyle _centeredLabelStyle;
    public static GUIStyle CenteredLabelStyle
    {
        get
        {
            if (_centeredLabelStyle == null)
            {
                _centeredLabelStyle = new GUIStyle(GUI.skin.label);
                _centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            return _centeredLabelStyle;
        }
    }

    private static GUIStyle _rightLabelStyle;
    public static GUIStyle RightLabelStyle
    {
        get
        {
            if (_rightLabelStyle == null)
            {
                _rightLabelStyle = new GUIStyle(GUI.skin.label);
                _rightLabelStyle.alignment = TextAnchor.MiddleRight;
            }

            return _rightLabelStyle;
        }
    }

    private static GUIStyle _centeredMiniLabelStyle;
    public static GUIStyle CenteredMiniLabelStyle
    {
        get
        {
            if (_centeredMiniLabelStyle == null)
            {
                _centeredMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                _centeredMiniLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            return _centeredMiniLabelStyle;
        }
    }

    private static GUIStyle _centeredBoldLabelStyle;
    public static GUIStyle CenteredBoldLabelStyle
    {
        get
        {
            if (_centeredBoldLabelStyle == null)
            {
                _centeredBoldLabelStyle = new GUIStyle(GUI.skin.label);
                _centeredBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
                _centeredBoldLabelStyle.fontStyle = FontStyle.Bold;
            }

            return _centeredBoldLabelStyle;
        }
    }

    private static GUIStyle _richWordWrappedLabelStyle;
    public static GUIStyle RichWordWrappedLabelStyle
    {
        get
        {
            if (_richWordWrappedLabelStyle == null)
            {
                _richWordWrappedLabelStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                _richWordWrappedLabelStyle.richText = true;
            }

            return _richWordWrappedLabelStyle;
        }
    }

    private static GUIStyle _richHelpBoxStyle;
    public static GUIStyle RichHelpBoxStyle
    {
        get
        {
            if (_richHelpBoxStyle == null)
            {
                _richHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                _richHelpBoxStyle.richText = true;
            }

            return _richHelpBoxStyle;
        }
    }

    private static GUIStyle _miniButtonLeftWithNoPadding;
    public static GUIStyle MiniButtonLeftWithNoPadding
    {
        get
        {
            if (_miniButtonLeftWithNoPadding == null)
            {
                _miniButtonLeftWithNoPadding = new GUIStyle(EditorStyles.miniButtonLeft);
                _miniButtonLeftWithNoPadding.padding = new RectOffset(0, 0, 0, 0);
            }

            return _miniButtonLeftWithNoPadding;
        }
    }

    private static GUIStyle _miniButtonMidWithNoPadding;
    public static GUIStyle MiniButtonMidWithNoPadding
    {
        get
        {
            if (_miniButtonMidWithNoPadding == null)
            {
                _miniButtonMidWithNoPadding = new GUIStyle(EditorStyles.miniButtonMid);
                _miniButtonMidWithNoPadding.padding = new RectOffset(0, 0, 0, 0);
            }

            return _miniButtonMidWithNoPadding;
        }
    }

    private static GUIStyle _miniButtonRightWithNoPadding;
    public static GUIStyle MiniButtonRightWithNoPadding
    {
        get
        {
            if (_miniButtonRightWithNoPadding == null)
            {
                _miniButtonRightWithNoPadding = new GUIStyle(EditorStyles.miniButtonRight);
                _miniButtonRightWithNoPadding.padding = new RectOffset(0, 0, 0, 0);
            }

            return _miniButtonRightWithNoPadding;
        }
    }

    private static GUIStyle _textFieldAutoCompleteRecommendKeywordStyle;
    public static GUIStyle TextFieldAutoCompleteRecommendKeywordStyle
    {
        get
        {
            if (_textFieldAutoCompleteRecommendKeywordStyle == null)
            {
                _textFieldAutoCompleteRecommendKeywordStyle = EditorStyles.miniButton;
                _textFieldAutoCompleteRecommendKeywordStyle.alignment = TextAnchor.MiddleLeft;
            }

            return _textFieldAutoCompleteRecommendKeywordStyle;
        }
    }

    private static GUIStyle _oddListStyle;
    public static GUIStyle OddListStyle
    {
        get
        {
            if (_oddListStyle == null)
            {
                _oddListStyle = new GUIStyle(GUI.skin.box);
                _oddListStyle.margin = new RectOffset(0, 0, 0, 0);
                _oddListStyle.padding = new RectOffset(2, 2, 4, 4);
                _oddListStyle.normal.background = MakeColoredTexture(1, 1, new Color(0.56f, 0.56f, 0.56f));
            }

            return _oddListStyle;
        }
    }

    private static GUIStyle _evenListStyle;
    public static GUIStyle EvenListStyle
    {
        get
        {
            if (_evenListStyle == null)
            {
                _evenListStyle = new GUIStyle(GUI.skin.box);
                _evenListStyle.margin = new RectOffset(0, 0, 0, 0);
                _evenListStyle.padding = new RectOffset(2, 2, 4, 4);
                _evenListStyle.normal.background = MakeColoredTexture(1, 1, new Color(0.28f, 0.28f, 0.28f));
            }

            return _evenListStyle;
        }
    }

    private static GUIStyle _greenBackgroundStyle;
    public static GUIStyle GreenBackgroundStyle
    {
        get
        {
            if (_greenBackgroundStyle == null)
            {
                _greenBackgroundStyle = new GUIStyle();
                _greenBackgroundStyle.normal.background = MakeColoredTexture(1, 1, Color.green);
            }

            return _greenBackgroundStyle;
        }
    }

    private static GUIStyle _redBackgroundStyle;
    public static GUIStyle RedBackgroundStyle
    {
        get
        {
            if (_redBackgroundStyle == null)
            {
                _redBackgroundStyle = new GUIStyle();
                _redBackgroundStyle.normal.background = MakeColoredTexture(1, 1, Color.red);
            }

            return _redBackgroundStyle;
        }
    }

    private static GUIStyle _whiteBackgroundStyle;
    public static GUIStyle WhiteBackgroundStyle
    {
        get
        {
            if (_whiteBackgroundStyle == null)
            {
                _whiteBackgroundStyle = new GUIStyle();
                _whiteBackgroundStyle.normal.background = MakeColoredTexture(1, 1, new Color(1f, 1f, 0.9f));
            }

            return _whiteBackgroundStyle;
        }
    }

    private static Texture2D _errorIcon;
    public static Texture2D ErrorIcon
    {
        get
        {
            if (_errorIcon == null)
                _errorIcon = EditorGUIUtility.FindTexture("console.erroricon.sml");

            return _errorIcon;
        }
    }

    private static Texture2D _warningIcon;
    public static Texture2D WarningIcon
    {
        get
        {
            if (_warningIcon == null)
                _warningIcon = EditorGUIUtility.FindTexture("console.warnicon.sml");

            return _warningIcon;
        }
    }

    private static Texture2D _minIcon;
    public static Texture2D MinIcon
    {
        get
        {
            if (_minIcon == null)
                _minIcon = EditorGUIUtility.FindTexture("winbtn_win_min");

            return _minIcon;
        }
    }

    private static Texture2D _maxIcon;
    public static Texture2D MaxIcon
    {
        get
        {
            if (_maxIcon == null)
                _maxIcon = EditorGUIUtility.FindTexture("winbtn_win_rest");

            return _maxIcon;
        }
    }

    private static Texture2D _refreshIcon;
    public static Texture2D RefreshIcon
    {
        get
        {
            if (_refreshIcon == null)
                _refreshIcon = EditorGUIUtility.FindTexture("Refresh");

            return _refreshIcon;
        }
    }

    private static Texture2D _saveIcon;
    public static Texture2D SaveIcon
    {
        get
        {
            if (_saveIcon == null)
                _saveIcon = EditorGUIUtility.FindTexture("CollabEdit Icon");

            return _saveIcon;
        }
    }

    private static Texture2D _loadIcon;
    public static Texture2D LoadIcon
    {
        get
        {
            if (_loadIcon == null)
                _loadIcon = EditorGUIUtility.FindTexture("CollabMoved Icon");

            return _loadIcon;
        }
    }

    private static Texture2D _showIcon;
    public static Texture2D ShowIcon
    {
        get
        {
            if (_showIcon == null)
                _showIcon = EditorGUIUtility.FindTexture("animationvisibilitytoggleon");

            return _showIcon;
        }
    }

    private static Texture2D _hideIcon;
    public static Texture2D HideIcon
    {
        get
        {
            if (_hideIcon == null)
                _hideIcon = EditorGUIUtility.FindTexture("animationvisibilitytoggleoff");

            return _hideIcon;
        }
    }
}
