using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Collectible))]
public class CollectibleEditor : Editor
{
    private bool clicked;
    private bool rotclicked = true;
    private bool floatclicked = true;

    private bool xaxis;
    private bool yaxis;
    private bool zaxis;
    private float speed;
    private float amplitude;
    private float frequency;

    public override void OnInspectorGUI()
    {
        Collectible collectible = (Collectible)target;

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

        if(GUILayout.Button("rotatie knoppen")) 
        {
            if (rotclicked)
                rotclicked = false;
            else
                rotclicked = true;
        }

        EditorGUI.BeginChangeCheck();
        if (rotclicked)
        {
            EditorGUILayout.LabelField("collectible laten draaien");
            xaxis = EditorGUILayout.Toggle("x_rotatie", collectible.xaxis);
            yaxis = EditorGUILayout.Toggle("y_rotatie", collectible.yaxis);
            zaxis = EditorGUILayout.Toggle("z_rotatie", collectible.zaxis);
            speed = EditorGUILayout.Slider("draaisnelheid", collectible.speed, 1, 50);

        }

        if (GUILayout.Button("beweeg knoppen"))
        {
            if (floatclicked)
                floatclicked = false;
            else
                floatclicked = true;
        }


        if (floatclicked)
        {
            EditorGUILayout.LabelField("collectible op en neer bewegen");
            amplitude = EditorGUILayout.Slider("afstand", collectible.amplitude, 0, 10);
            frequency = EditorGUILayout.Slider("snelheid", collectible.frequency, 0, 10);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
            collectible.xaxis = xaxis;
            collectible.yaxis = yaxis;
            collectible.zaxis = zaxis;
            collectible.speed = speed;
            collectible.amplitude = amplitude;
            collectible.frequency = frequency;  
        }
    }
}
