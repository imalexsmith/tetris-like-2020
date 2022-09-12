using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using NightFramework;
using CES = CustomEditorStyles;


[CustomEditor(typeof (Localization), true)]
public class LocalizationEditor : CustomEditorBase<Localization>
{
    // ========================================================================================
    private static readonly Color _activeOrderButtonColor = new Color(0.65f, 0.8f, 1f);
    private static int _orderBy = -1;

    private int _maxRecommendCount = 4;
    private string _searchStr;
    private int _selectedIndex = -1;
    private string _newVal;
    private string _newKey;


    // ========================================================================================
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        Undo.RecordObject(target, "Localization Changes");


        var keys = Target.GetKeys().ToList();
        var entries = Target.GetEntries().ToList();
        var lCount = Enum.GetValues(typeof(Localization.Languages)).Length;
        var kCount = keys.Count;
        var eCount = entries.Count;
        
        InformationMessage += string.Format("<color=blue>{0}</color> languages\n<color=blue>{1}</color> keys\n"
                                            + (eCount == lCount * kCount
                                                ? "<color=blue>{2}</color>"
                                                : "<color=red>{2}</color>") + " entries\n" , lCount, kCount, eCount);


        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.BeginVertical(GUILayout.Width(225f));
        if (GUILayout.Button("Open in new window"))
        {
            this.OpenInNewWindow();
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        var defColor = GUI.color;
        if (_orderBy == -1)
            GUI.color = _activeOrderButtonColor;
        if (GUILayout.Button("Order default"))
        {
            _selectedIndex = -1;
            _orderBy = -1;
        }
        var ordKeyStr = "Order by key (A-Z)";
        if (_orderBy == 0 || _orderBy == 1)
        {
            GUI.color = _activeOrderButtonColor;
            ordKeyStr = _orderBy == 0 ? "Order by key (A-Z)" : "Order by key (Z-A)";
        }
        else
            GUI.color = defColor;
        if (GUILayout.Button(ordKeyStr))
        {
            _selectedIndex = -1;
            switch (_orderBy)
            {
                case 0:
                    _orderBy = 1;
                    break;
                default:
                    _orderBy = 0;
                    break;
            }
        }
        var ordLangStr = "Order by lang (A-Z)";
        if (_orderBy == 2 || _orderBy == 3)
        {
            GUI.color = _activeOrderButtonColor;
            ordLangStr = _orderBy == 2 ? "Order by lang (A-Z)" : "Order by lang (Z-A)";
        }
        else
            GUI.color = defColor;
        if (GUILayout.Button(ordLangStr))
        {
            _selectedIndex = -1;
            switch (_orderBy)
            {
                case 2:
                    _orderBy = 3;
                    break;
                default:
                    _orderBy = 2;
                    break;
            }
        }
        GUI.color = defColor;
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Load from file..."))
        {
            var path = EditorUtility.OpenFilePanel("Load localization from file", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                Target.Clear();
                CSVReader.LoadFromFile(path, AddEntry);
                EditorUtility.SetDirty(target);
            }
        }
        if (GUILayout.Button("Save to file..."))
        {
            var path = EditorUtility.SaveFilePanel("Save localization to file", "", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                var sb = new StringBuilder();

                for (var i = 0; i < eCount; i++)
                {
                    sb.Append("\"")
                      .Append(entries[i].Key)
                      .Append("\",\"")
                      .Append(entries[i].Language)
                      .Append("\",\"")
                      .Append(entries[i].Value)
                      .Append("\"")
                      .Append(Environment.NewLine);
                }

                File.WriteAllText(path, sb.ToString());
            }
        }
        if (GUILayout.Button("Restore missing"))
        {
            Target.RestoreMissingEntries();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight * (_maxRecommendCount + 1)));
        EditorGUILayout.LabelField("Browse key:", GUILayout.Width(75f));
        GUI.SetNextControlName("_searchStrTextField");
        _searchStr = EditorExtend.TextFieldAutoComplete(_searchStr, keys, _maxRecommendCount);
        if (GUI.GetNameOfFocusedControl().StartsWith("_searchStrTextField"))
            _selectedIndex = -1;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();


        if (!string.IsNullOrEmpty(_searchStr))
        {
            entries = entries.Where(x => x.Key.Contains(_searchStr)).ToList();
            keys = keys.Where(x => x.Contains(_searchStr)).ToList();
        }
        switch (_orderBy)
        {
            case 0:
                entries = entries.OrderBy(x => x.Key).ToList();
                break;
            case 1:
                entries = entries.OrderByDescending(x => x.Key).ToList();
                break;
            case 2:
                entries = entries.OrderBy(x => x.Language).ToList();
                break;
            case 3:
                entries = entries.OrderByDescending(x => x.Language).ToList();
                break;
        }
        var entriesList = entries.ToList();
        var keysList = keys.ToList();


        var k = 1;
        for (int i = 0; i < entriesList.Count; i++)
        {
            /*if (_orderBy == 0)
            {
                if (k == 1)
                {
                    EditorGUILayout.BeginHorizontal(i % 2 == 0 ? CES.EvenListStyle : CES.OddListStyle);
                    EditorGUILayout.SelectableLabel(entriesList[i].Key, GUILayout.Width(130f));
                }

                if (_selectedIndex != i)
                {
                    EditorGUILayout.SelectableLabel(entriesList[i].Value, CES.RichWordWrappedLabelStyle,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f), GUILayout.MaxWidth(430f));

                    if (GUILayout.Button("Edit", EditorStyles.miniButtonLeft, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        _selectedIndex = i;
                        _newVal = entriesList[i].Value;
                        GUIUtility.keyboardControl = 0;
                    }

                    if (GUILayout.Button("Remove", EditorStyles.miniButtonRight, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        if (Target.RemoveEntry(entriesList[i].Key, entriesList[i].Language))
                        {
                            GUIUtility.keyboardControl = 0;
                            EditorUtility.SetDirty(target);
                        }
                    }
                }
                else
                {
                    _newVal = EditorGUILayout.TextArea(_newVal,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f), GUILayout.MaxWidth(430f));
                    if (GUILayout.Button("Apply", EditorStyles.miniButtonLeft, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        Target.AddEntry(new Localization.LocalizationEntry
                        {
                            Key = entriesList[_selectedIndex].Key,
                            Language = entriesList[_selectedIndex].Language,
                            Value = _newVal.TrimEnd()
                        }, true);
                        _selectedIndex = -1;
                        _newVal = "";
                        GUIUtility.keyboardControl = 0;
                        EditorUtility.SetDirty(target);
                    }
                    if (GUILayout.Button("Cancel", EditorStyles.miniButtonRight, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        _selectedIndex = -1;
                        _newVal = "";
                        GUIUtility.keyboardControl = 0;
                    }
                }

                if (k == 3)
                {
                    EditorGUILayout.EndHorizontal();
                    k = 0;
                }

                k++;
            }
            else*/
            {
                EditorGUILayout.BeginHorizontal(i % 2 == 0 ? CES.EvenListStyle : CES.OddListStyle);
                EditorGUILayout.SelectableLabel(entriesList[i].Key, GUILayout.Width(130f));
                EditorGUILayout.SelectableLabel(entriesList[i].Language.ToString(), GUILayout.Width(25f));

                if (_selectedIndex != i)
                {
                    EditorGUILayout.SelectableLabel(entriesList[i].Value, CES.RichWordWrappedLabelStyle,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f), GUILayout.MaxWidth(430f));

                    if (GUILayout.Button("Edit", EditorStyles.miniButtonLeft, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        _selectedIndex = i;
                        _newVal = entriesList[i].Value;
                        GUIUtility.keyboardControl = 0;
                    }

                    if (GUILayout.Button("Remove", EditorStyles.miniButtonRight, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        if (Target.RemoveEntry(entriesList[i].Key, entriesList[i].Language))
                        {
                            GUIUtility.keyboardControl = 0;
                            EditorUtility.SetDirty(target);
                        }
                    }
                }
                else
                {
                    _newVal = EditorGUILayout.TextArea(_newVal,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f), GUILayout.MaxWidth(430f));
                    if (GUILayout.Button("Apply", EditorStyles.miniButtonLeft, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        Target.AddEntry(new Localization.LocalizationEntry
                        {
                            Key = entriesList[_selectedIndex].Key,
                            Language = entriesList[_selectedIndex].Language,
                            Value = _newVal.TrimEnd()
                        }, true);
                        _selectedIndex = -1;
                        _newVal = "";
                        GUIUtility.keyboardControl = 0;
                        EditorUtility.SetDirty(target);
                    }
                    if (GUILayout.Button("Cancel", EditorStyles.miniButtonRight, GUILayout.Height(16f),
                        GUILayout.Width(50f)))
                    {
                        _selectedIndex = -1;
                        _newVal = "";
                        GUIUtility.keyboardControl = 0;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.Space();


        EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight * (_maxRecommendCount + 1)));
        _newKey = EditorExtend.TextFieldAutoComplete(_newKey, keysList, _maxRecommendCount);
        if (GUILayout.Button("Add key", EditorStyles.miniButton, GUILayout.Height(16f)))
        {
            if (!string.IsNullOrEmpty(_newKey))
            {
                Target.AddKey(_newKey);
                _newKey = "";
                GUIUtility.keyboardControl = 0;
                EditorUtility.SetDirty(target);
            }
        }
        EditorGUILayout.EndHorizontal();



        serializedObject.ApplyModifiedProperties();
    }

    private void AddEntry(int index, List<string> line)
    {
        Target.AddEntry(new Localization.LocalizationEntry
        {
            Key = line[0],
            Language = (Localization.Languages)Enum.Parse(typeof(Localization.Languages), line[1]),
            Value = line[2]
        });
    }
}





//            //GUI.SetNextControlName("entriesList_Value" + i);
//            if (_currentIndex == i)
//            {
//                if (Event.current.type == EventType.Repaint)
//               
//                    EditorGUILayout.TextField(entriesList[i].Value,
//                        GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f));
//                else
//                   
//            }


//Debug.Log(GUIUtility.hotControl + " /// " + GUI.GetNameOfFocusedControl());
//            if (GUI.GetNameOfFocusedControl() != ("value"+i))
//            {
//                Debug.Log("value"+i);
//                //GUIUtility.hotControl
//                //GUIUtility.GetControlID()
//            }

//            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "user")
//                Debug.Log("Login");

//            if (i == 0)
//            {
//                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(false));
//                str = EditorGUILayout.TextArea(str, GUILayout.ExpandHeight(true));
//                EditorGUILayout.EndScrollView();
//            }

//        if (!string.IsNullOrEmpty(_newKey))
//        {
//            var gm = new GenericMenu();
//            for (int i = 0; i < keysList.Count; i++)
//            {
//                var k = keysList[i];
//                if (k.Contains(_newKey))
//                    gm.AddItem(new GUIContent(k), false, () => { _newKey = k; GUI.FocusControl("_newKeyTextField"); });
//            }
//            //gm.DropDown(GUILayoutUtility.GetLastRect());
////            GUIUtility.hotControl
////            EditorGUIUtility.editingTextField
//
//            GUI.FocusControl("_newKeyTextField"); 
//        }


//        if (_lastFocusedName != GUI.GetNameOfFocusedControl())
//        {
//            var _lastIndexStr = _lastFocusedName.Replace("entriesList_Value", "");
//            var _lastIndex = int.Parse(_lastIndexStr);
//            Debug.Log(_lastFocusedName + " //// " + _lastIndex + " //// " + _values[_lastIndex]);
//
////            Target.AddEntry(new Localization.LocalizationEntry
////            {
////                Key = entriesList[_currentIndex].Key,
////                Language = entriesList[_currentIndex].Language,
////                Value = _newVal
////            }, true);
//        }
//        _lastFocusedName = GUI.GetNameOfFocusedControl();

