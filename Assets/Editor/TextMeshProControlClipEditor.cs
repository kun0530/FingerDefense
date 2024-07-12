using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;

[CustomEditor(typeof(TextMeshProControlClip))]
public class TextMeshProControlClipEditor : Editor
{
    SerializedProperty textProperty;
    Vector2 scrollPos;

    private void OnEnable()
    {
        textProperty = serializedObject.FindProperty("text");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(50));
        textProperty.stringValue = EditorGUILayout.TextArea(textProperty.stringValue, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }
}