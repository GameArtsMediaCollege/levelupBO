using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Deadzone))]
public class DeadzoneEditor : Editor
{
    private bool clicked;
    private bool extendedclicked;
    private bool colliderpresent;
    private Collider col;


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

        if(deadzone.collider == null)
        {
            deadzone.colliderfound = false;
            warning();
        }
        else
        {
            deadzone.colliderfound = true;
        }

        if (GUILayout.Button("uitleg"))
        {
            if (clicked)
                clicked = false;
            else
                clicked = true;
        }

        if (GUILayout.Button("advanced"))
        {
            if (extendedclicked)
                extendedclicked = false;
            else
                extendedclicked = true;
        }

        if (clicked)
        {
            EditorGUILayout.HelpBox("Dit script controlleert de deadzone. \nWanneer de speler in de collider doos valt, dan teleporteert de speler naar het spawn punt. Je kunt deze besturen met de groene sfeer", MessageType.Info);
        }

        if (extendedclicked)
        {
            EditorGUI.BeginChangeCheck();
            col = EditorGUILayout.ObjectField(deadzone.collider, typeof(Collider), true) as Collider;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(col, "Changed Area Of Effect");
                if(col == null)
                {
                    deadzone.colliderfound = false;
                }
                deadzone.collider = col;
            }
        }
    }

    private void warning()
    {
        EditorGUILayout.HelpBox("Er is geen collider toegevoegd aan de deadzone. Voeg eentje toe bij de advanced settings", MessageType.Warning);
    }
}
