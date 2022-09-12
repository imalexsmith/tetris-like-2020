using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using CES = CustomEditorStyles;

// ========================
// Revision 2020.10.17
// ========================

public sealed class EditorExtend
{
    // ========================================================================================
    private const float ToggleWidth = 13f;


    // ========================================================================================
    public enum InputAxisType
    {
        KeyOrMouseButton,
        MouseMovement,
        JoystickAxis
    };


    // ========================================================================================
    public static string[] ReadInputAxes(InputAxisType? axisType = null)
    {
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

        var obj = new SerializedObject(inputManager);

        var axisArray = obj.FindProperty("m_Axes");

        if (axisArray.arraySize == 0)
            return new string[0];

        var list = new List<string>();
        for (int i = 0; i < axisArray.arraySize; ++i)
        {
            var axis = axisArray.GetArrayElementAtIndex(i);

            var inputType = (InputAxisType)axis.FindPropertyRelative("type").intValue;
            if (!axisType.HasValue || axisType.Value == inputType)
                list.Add(axis.FindPropertyRelative("m_Name").stringValue);
        }

        return list.ToArray();
    }

    public static void LoadUniqueScriptableObject<T>() where T : ScriptableObject
    {
        var tName = typeof(T).Name;
        var ids = AssetDatabase.FindAssets("t:" + tName);

        if (ids.Length == 1)
            AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(ids[0]));
    }

    public static T CreateUniqueScriptableObjectOrSelect<T>(string exceptionText = "") where T : ScriptableObject
    {
        var tName = typeof(T).Name;
        var ids = AssetDatabase.FindAssets("t:" + tName);

        if (ids.Length == 0)
        {
            var asset = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, $"Assets/Data/{tName}.asset");
            return asset;
        }

        if (ids.Length == 1)
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(ids[0]));
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
            return asset;
        }

        Debug.LogWarning(exceptionText);
        return null;
    }

    public static void MakeReorderableListHeaderFoldout(ReorderableList list, Rect rect, ref bool value)
    {
        var e = Event.current;
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(new Rect(rect.x, rect.y, ToggleWidth, rect.height), false, false, value, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            value = !value;
            SwitchReorderableListFoldout(list, value);
            e.Use();
        }
    }

    public static void MakeReorderableListHeaderLabel(ReorderableList list, Rect rect)
    {
        EditorGUI.LabelField(new Rect(rect.x + ToggleWidth, rect.y, rect.width - ToggleWidth, rect.height),
            new GUIContent(string.Format("[{0}] {1}", list.count, list.serializedProperty.name), ""),
            EditorStyles.boldLabel);
    }

    public static void MakeReorderableListHeaderLabel(ReorderableList list, Rect rect, string label)
    {
        EditorGUI.LabelField(new Rect(rect.x + ToggleWidth, rect.y, rect.width - ToggleWidth, rect.height),
            new GUIContent(string.Format("[{0}] {1}", list.count, label), ""),
            EditorStyles.boldLabel);
    }

    public static void SwitchReorderableListFoldout(ReorderableList list, bool value)
    {
        list.displayAdd = value;
        list.displayRemove = value;
        list.draggable = value;
        list.elementHeight = value ? EditorGUIUtility.singleLineHeight : 0f;
        list.footerHeight = value ? EditorGUIUtility.singleLineHeight : 0f;
    }

    public static int GetLastControlId()
    {
        var getLastControlId = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
        if (getLastControlId != null)
            return (int)getLastControlId.GetValue(null);
        return 0;
    }

    private const string m_AutoCompleteField = "AutoCompleteField";
    private static List<string> m_CacheCheckList = null;
    private static string m_AutoCompleteLastInput;
    private static string m_EditorFocusAutoComplete;
    /// <summary>A textField to popup a matching popup, based on developers input values.</summary>
    /// <param name="input">string input.</param>
    /// <param name="source">the data of all possible values (string).</param>
    /// <param name="maxShownCount">the amount to display result.</param>
    /// <param name="levenshteinDistance">
    /// value between 0f ~ 1f,
    /// - more then 0f will enable the fuzzy matching
    /// - 1f = anything thing is okay.
    /// - 0f = require full match to the reference
    /// - recommend 0.4f ~ 0.7f
    /// </param>
    /// <returns>output string.</returns>
    public static string TextFieldAutoComplete(Rect position, string input, IEnumerable<string> source, int maxShownCount = 5, float levenshteinDistance = 0.5f)
    {
        string tag = m_AutoCompleteField + GUIUtility.GetControlID(FocusType.Passive);
        int uiDepth = GUI.depth;
        GUI.SetNextControlName(tag);
        string rst = EditorGUI.TextField(position, input);
        if (!string.IsNullOrEmpty(input) && GUI.GetNameOfFocusedControl() == tag)
        {
            if (m_AutoCompleteLastInput != input || // input changed
                m_EditorFocusAutoComplete != tag) // another field.
            {
                // Update cache
                m_EditorFocusAutoComplete = tag;
                m_AutoCompleteLastInput = input;

                List<string> uniqueSrc = new List<string>(new HashSet<string>(source)); // remove duplicate
                int srcCnt = uniqueSrc.Count;
                m_CacheCheckList = new List<string>(System.Math.Min(maxShownCount, srcCnt)); // optimize memory alloc

                // Start with - slow
                for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                {
                    if (uniqueSrc[i].ToLower().StartsWith(input.ToLower()))
                    {
                        m_CacheCheckList.Add(uniqueSrc[i]);
                        uniqueSrc.RemoveAt(i);
                        srcCnt--;
                        i--;
                    }
                }

                // Contains - very slow
                if (m_CacheCheckList.Count == 0)
                {
                    for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                    {
                        if (uniqueSrc[i].ToLower().Contains(input.ToLower()))
                        {
                            m_CacheCheckList.Add(uniqueSrc[i]);
                            uniqueSrc.RemoveAt(i);
                            srcCnt--;
                            i--;
                        }
                    }
                }

                // Levenshtein Distance - very very slow.
                if (levenshteinDistance > 0f && // only developer request
                    input.Length > 3 && // 3 characters on input, hidden value to avoid doing too early.
                    m_CacheCheckList.Count < maxShownCount) // have some empty space for matching.
                {
                    levenshteinDistance = Mathf.Clamp01(levenshteinDistance);
                    string keywords = input.ToLower();
                    for (int i = 0; i < srcCnt && m_CacheCheckList.Count < maxShownCount; i++)
                    {
                        int distance = LevenshteinDistance(uniqueSrc[i], keywords, caseSensitive: false);
                        bool closeEnough = (int)(levenshteinDistance * uniqueSrc[i].Length) > distance;
                        if (closeEnough)
                        {
                            m_CacheCheckList.Add(uniqueSrc[i]);
                            uniqueSrc.RemoveAt(i);
                            srcCnt--;
                            i--;
                        }
                    }
                }
            }

            // Draw recommend keyward(s)
            if (m_CacheCheckList.Count > 0)
            {
                int cnt = m_CacheCheckList.Count;
                float height = cnt * EditorGUIUtility.singleLineHeight;
                Rect area = position;
                area = new Rect(area.x, area.y + position.height, area.width, height);
                //GUI.depth -= 10;
                 //GUI.BeginGroup(area);
                // area.position = Vector2.zero;
                GUI.BeginClip(area);
                Rect line = new Rect(0, 0, area.width, EditorGUIUtility.singleLineHeight);

                for (int i = 0; i < cnt; i++)
                {
                    if (GUI.Button(line, m_CacheCheckList[i], CES.TextFieldAutoCompleteRecommendKeywordStyle))
                    {
                        rst = m_CacheCheckList[i];
                        GUI.changed = true;
                        GUI.FocusControl(""); // force update
                    }
                    line.y += line.height;
                }
                GUI.EndClip();
                //GUI.EndGroup();
                //GUI.depth += 10;
            }
        }
        return rst;
    }
    
    public static string TextFieldAutoComplete(string input, IEnumerable<string> source, int maxShownCount = 5, float levenshteinDistance = 0.5f)
    {
        Rect rect = EditorGUILayout.GetControlRect();
        return TextFieldAutoComplete(rect, input, source, maxShownCount, levenshteinDistance);
    }

    /// <summary>Computes the Levenshtein Edit Distance between two enumerables.</summary>
    /// <typeparam name="T">The type of the items in the enumerables.</typeparam>
    /// <param name="lhs">The first enumerable.</param>
    /// <param name="rhs">The second enumerable.</param>
    /// <returns>The edit distance.</returns>
    /// <see cref="https://blogs.msdn.microsoft.com/toub/2006/05/05/generic-levenshtein-edit-distance-with-c/"/>
    public static int LevenshteinDistance<T>(IEnumerable<T> lhs, IEnumerable<T> rhs) where T : System.IEquatable<T>
    {
        // Validate parameters
        if (lhs == null) throw new System.ArgumentNullException("lhs");
        if (rhs == null) throw new System.ArgumentNullException("rhs");

        // Convert the parameters into IList instances
        // in order to obtain indexing capabilities
        IList<T> first = lhs as IList<T> ?? new List<T>(lhs);
        IList<T> second = rhs as IList<T> ?? new List<T>(rhs);

        // Get the length of both.  If either is 0, return
        // the length of the other, since that number of insertions
        // would be required.
        int n = first.Count, m = second.Count;
        if (n == 0) return m;
        if (m == 0) return n;

        // Rather than maintain an entire matrix (which would require O(n*m) space),
        // just store the current row and the next row, each of which has a length m+1,
        // so just O(m) space. Initialize the current row.
        int curRow = 0, nextRow = 1;

        int[][] rows = new int[][] { new int[m + 1], new int[m + 1] };
        for (int j = 0; j <= m; ++j)
            rows[curRow][j] = j;

        // For each virtual row (since we only have physical storage for two)
        for (int i = 1; i <= n; ++i)
        {
            // Fill in the values in the row
            rows[nextRow][0] = i;

            for (int j = 1; j <= m; ++j)
            {
                int dist1 = rows[curRow][j] + 1;
                int dist2 = rows[nextRow][j - 1] + 1;
                int dist3 = rows[curRow][j - 1] +
                            (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                rows[nextRow][j] = System.Math.Min(dist1, System.Math.Min(dist2, dist3));
            }

            // Swap the current and next rows
            if (curRow == 0)
            {
                curRow = 1;
                nextRow = 0;
            }
            else
            {
                curRow = 0;
                nextRow = 1;
            }
        }

        // Return the computed edit distance
        return rows[curRow][m];
    }

    /// <summary>Computes the Levenshtein Edit Distance between two enumerables.</summary>
    /// <param name="lhs">The first enumerable.</param>
    /// <param name="rhs">The second enumerable.</param>
    /// <returns>The edit distance.</returns>
    /// <see cref="https://en.wikipedia.org/wiki/Levenshtein_distance"/>
    public static int LevenshteinDistance(string lhs, string rhs, bool caseSensitive = true)
    {
        if (!caseSensitive)
        {
            lhs = lhs.ToLower();
            rhs = rhs.ToLower();
        }
        char[] first = lhs.ToCharArray();
        char[] second = rhs.ToCharArray();
        return LevenshteinDistance<char>(first, second);
    }
}