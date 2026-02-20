using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Collectible))]
public class CollectibleEditor : Editor
{
    bool showAdvanced = false;
    bool showMocement = false;
    bool clicked;

    SerializedProperty speedProp;
    SerializedProperty xaxis;
    SerializedProperty yaxis;
    SerializedProperty zaxis;
    SerializedProperty amplitude;
    SerializedProperty frequency;

    void OnEnable()
    {
        speedProp = serializedObject.FindProperty("speed");
        xaxis = serializedObject.FindProperty("xaxis");
        yaxis = serializedObject.FindProperty("yaxis");
        zaxis = serializedObject.FindProperty("zaxis");
        amplitude = serializedObject.FindProperty("amplitude");
        frequency = serializedObject.FindProperty("frequency");
    }

    public override void OnInspectorGUI()
    {
        Collectible collectible = (Collectible)target;
        serializedObject.Update();


        if (GUILayout.Button("uitleg"))
        {
            if (clicked)
                clicked = false;
            else
                clicked = true;
        }
        
        if(collectible.GetComponent<Collider>() == null)
            EditorGUILayout.HelpBox("Je moet een collider toevoegen om dit te laten werken", MessageType.Info);

        if (clicked)
        {
            EditorGUILayout.HelpBox("Dit script bestuurt het opraapbare object. \nWanneer de speler binnen de collider loopt, dan zal dit script een particle effect afspelen en het object verwijderen. \nJe kunt de collectible laten draaien met de volgende rotatie knoppen en de snelheid", MessageType.Info);
        }

        // Toggle knop
        if (GUILayout.Button("Collectible Draaien"))
        {
            showAdvanced = !showAdvanced;
        }

        // Alleen tonen wanneer knop actief is
        if (showAdvanced)
        {
            EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(xaxis);
            EditorGUILayout.PropertyField(yaxis);
            EditorGUILayout.PropertyField(zaxis);
            speedProp.floatValue = EditorGUILayout.Slider("Draaisnelheid", speedProp.floatValue, 1f, 100f);
        }

        if (GUILayout.Button("Collectible Bewegen"))
        {
            showMocement = !showMocement;
        }
        if (showMocement)
        {
            amplitude.floatValue = EditorGUILayout.Slider("Amplitude", amplitude.floatValue, 0.1f, 2f);
            frequency.floatValue = EditorGUILayout.Slider("Frequentie", frequency.floatValue, 0.1f, 5f);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
