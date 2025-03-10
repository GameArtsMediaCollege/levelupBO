using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Deadzone))]
public class DeadzoneEditor : Editor
{
    private bool clicked;
    private void OnSceneGUI()
    {
        Deadzone deadzone = target as Deadzone;

        EditorGUI.BeginChangeCheck();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(deadzone, "Move Point");
            EditorUtility.SetDirty(deadzone);
        }
    }

    public override void OnInspectorGUI()
    {
        Deadzone deadzone = target as Deadzone;

        if (GUILayout.Button("uitleg"))
        {
            if (clicked)
                clicked = false;
            else
                clicked = true;
        }

        if (clicked)
        {
            EditorGUILayout.HelpBox("Dit script controlleert de deadzone. \nWanneer de speler in de collider doos valt, dan teleporteert de speler naar het spawn punt. Je kunt deze besturen met de groene sfeer", MessageType.Info);
        }
    }
}
