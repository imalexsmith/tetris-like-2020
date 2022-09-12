using UnityEngine;
using UnityEditor;
using NightFramework;
using UltEvents.Editor;

// ========================
// Revision 2020.10.16
// ========================

[CustomPropertyDrawer(typeof(SuperTimer))]
public class SuperTimerPropertyDrawer : PropertyDrawer
{
    // ========================================================================================
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            var result = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3f;

            var onStart = property.FindPropertyRelative(nameof(SuperTimer.NeedOnStart));
            var onPause = property.FindPropertyRelative(nameof(SuperTimer.NeedOnPause));
            var onResume = property.FindPropertyRelative(nameof(SuperTimer.NeedOnResume));
            var onComplete = property.FindPropertyRelative(nameof(SuperTimer.NeedOnComplete));

            if (onStart.boolValue)
            {
                var p = property.FindPropertyRelative(nameof(SuperTimer.OnStart));
                if (p.isExpanded)
                    result += EditorGUI.GetPropertyHeight(p);
                else
                    result += EditorGUIUtility.singleLineHeight;

                result += EditorGUIUtility.standardVerticalSpacing;
            }    

            if (onPause.boolValue)
            {
                var p = property.FindPropertyRelative(nameof(SuperTimer.OnPause));
                if (p.isExpanded)
                    result += EditorGUI.GetPropertyHeight(p);
                else
                    result += EditorGUIUtility.singleLineHeight;
                
                result += EditorGUIUtility.standardVerticalSpacing;
            }

            if (onResume.boolValue)
            {
                var p = property.FindPropertyRelative(nameof(SuperTimer.OnResume));
                if (p.isExpanded)
                    result += EditorGUI.GetPropertyHeight(p);
                else
                    result += EditorGUIUtility.singleLineHeight;
                
                result += EditorGUIUtility.standardVerticalSpacing;
            }

            if (onComplete.boolValue)
            {
                var p = property.FindPropertyRelative(nameof(SuperTimer.OnComplete));
                if (p.isExpanded)
                    result += EditorGUI.GetPropertyHeight(p);
                else
                    result += EditorGUIUtility.singleLineHeight;
                
                result += EditorGUIUtility.standardVerticalSpacing;
            }

            var subTimers = property.FindPropertyRelative(nameof(SuperTimer.SubTimers));
            if (subTimers.isExpanded)
                result += EditorGUI.GetPropertyHeight(subTimers);
            else
                result += EditorGUIUtility.singleLineHeight; 

            return result + EditorGUIUtility.standardVerticalSpacing;
        }
        else
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = new Rect(position.x + EditorGUI.indentLevel * 15f, position.y, position.width - EditorGUI.indentLevel * 15f, position.height);

        var ind = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var timerObj = property.GetValue<SuperTimer>();

        // Draw property name and current state and progress
        var r0 = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
        var r1 = new Rect(r0.x + r0.width, r0.y, position.width - r0.width, r0.height);
        var r2 = new Rect(r1.x + 2f, r0.y, (r1.width - 4f) / 2f, r0.height);
        var r3 = new Rect(r2.x + r2.width, r0.y, (r1.width - 4f) / 2f, r0.height);

        property.isExpanded = EditorGUI.Foldout(r0, property.isExpanded, label, true);
        EditorGUI.ProgressBar(r1, timerObj.Progress, "");
        EditorGUI.LabelField(r2, new GUIContent(timerObj.Status.ToString(), ObjectNames.NicifyVariableName(nameof(SuperTimer.Status))));
        EditorGUI.LabelField(r3, new GUIContent(timerObj.NumberOfCompletions.ToString(), ObjectNames.NicifyVariableName(nameof(SuperTimer.NumberOfCompletions))), CustomEditorStyles.RightLabelStyle);

        var rRemaining = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing);

        if (property.isExpanded)
        {
            // Draw line with show/hide toggles for events
            var p5 = property.FindPropertyRelative(nameof(SuperTimer.NeedOnStart));
            var p6 = property.FindPropertyRelative(nameof(SuperTimer.NeedOnPause));
            var p7 = property.FindPropertyRelative(nameof(SuperTimer.NeedOnResume));
            var p8 = property.FindPropertyRelative(nameof(SuperTimer.NeedOnComplete));

            var r4 = new Rect(rRemaining.x, rRemaining.y, rRemaining.width, EditorGUIUtility.singleLineHeight);
            var r5 = new Rect(r4.x + 2f, r4.y, (rRemaining.width - 4f) / 4f, r4.height);
            var r6 = new Rect(r5.x + r5.width, r5.y, r5.width, r5.height);
            var r7 = new Rect(r6.x + r6.width, r6.y, r6.width, r6.height);
            var r8 = new Rect(r7.x + r7.width, r7.y, r7.width, r7.height);

            EditorGUI.HelpBox(r4, "", MessageType.None);
            p5.boolValue = EditorGUI.ToggleLeft(r5, "OnStart", p5.boolValue, EditorStyles.miniLabel);
            p6.boolValue = EditorGUI.ToggleLeft(r6, "OnPause", p6.boolValue, EditorStyles.miniLabel);
            p7.boolValue = EditorGUI.ToggleLeft(r7, "OnResume", p7.boolValue, EditorStyles.miniLabel);
            p8.boolValue = EditorGUI.ToggleLeft(r8, "OnComplete", p8.boolValue, EditorStyles.miniLabel);

            rRemaining = new Rect(rRemaining.x, rRemaining.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, rRemaining.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing);

            // Draw UltEvent property block for each event, if needed
            if (p5.boolValue)
            {
                var pEvent = property.FindPropertyRelative(nameof(SuperTimer.OnStart));
                var h = EditorGUI.GetPropertyHeight(pEvent);

                EditorGUI.PropertyField(new Rect(rRemaining.x, rRemaining.y, rRemaining.width, h), pEvent, pEvent.isExpanded);

                rRemaining = new Rect(rRemaining.x, rRemaining.y + h + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, rRemaining.height - h - EditorGUIUtility.standardVerticalSpacing);
            }

            if (p6.boolValue)
            {
                var pEvent = property.FindPropertyRelative(nameof(SuperTimer.OnPause));
                var h = EditorGUI.GetPropertyHeight(pEvent);

                EditorGUI.PropertyField(new Rect(rRemaining.x, rRemaining.y, rRemaining.width, h), pEvent, pEvent.isExpanded);

                rRemaining = new Rect(rRemaining.x, rRemaining.y + h + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, rRemaining.height - h - EditorGUIUtility.standardVerticalSpacing);
            }

            if (p7.boolValue)
            {
                var pEvent = property.FindPropertyRelative(nameof(SuperTimer.OnResume));
                var h = EditorGUI.GetPropertyHeight(pEvent);

                EditorGUI.PropertyField(new Rect(rRemaining.x, rRemaining.y, rRemaining.width, h), pEvent, pEvent.isExpanded);

                rRemaining = new Rect(rRemaining.x, rRemaining.y + h + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, rRemaining.height - h - EditorGUIUtility.standardVerticalSpacing);
            }

            if (p8.boolValue)
            {
                var pEvent = property.FindPropertyRelative(nameof(SuperTimer.OnComplete));
                var h = EditorGUI.GetPropertyHeight(pEvent);

                EditorGUI.PropertyField(new Rect(rRemaining.x, rRemaining.y, rRemaining.width, h), pEvent, pEvent.isExpanded);

                rRemaining = new Rect(rRemaining.x, rRemaining.y + h + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, rRemaining.height - h - EditorGUIUtility.standardVerticalSpacing);
            }

            // Draw duration and repeate toggle
            var p13 = property.FindPropertyRelative(nameof(SuperTimer.Duration));
            var p14 = property.FindPropertyRelative(nameof(SuperTimer.Repeatable));

            var r13 = new Rect(rRemaining.x, rRemaining.y, rRemaining.width - 36f, EditorGUIUtility.singleLineHeight);
            var r14 = new Rect(r13.x + r13.width, r13.y, 36f, r13.height);

            EditorGUI.PropertyField(r13, p13);
            EditorGUIUtility.labelWidth = 18f;
            p14.boolValue = EditorGUI.Toggle(r14, new GUIContent(CustomEditorStyles.RefreshIcon, p14.displayName), p14.boolValue);
            EditorGUIUtility.labelWidth = 0f;

            // Draw subtimers
            var p15 = property.FindPropertyRelative(nameof(SuperTimer.SubTimers));

            rRemaining = new Rect(rRemaining.x, rRemaining.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, rRemaining.width, (position.y + position.height) - rRemaining.y - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing);
            
            EditorGUI.indentLevel++;
            EditorGUI.PropertyField(rRemaining, p15, new GUIContent($"{p15.displayName} ({p15.arraySize})"), p15.isExpanded);
        }

        EditorGUI.indentLevel = ind;
    }
}