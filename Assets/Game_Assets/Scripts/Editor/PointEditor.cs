using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(PlateauBeweger))]
public class PointEditor : Editor
{
    PlateauBeweger plateau;
    bool clicked;

    private void OnSceneGUI()
    {
        PlateauBeweger plateau = target as PlateauBeweger;
        Handles.color = Color.white;
        Transform handletransform = plateau.transform;


        Quaternion handleRotation = handletransform.rotation;
        Vector3 p0 = handletransform.position;
        Vector3 p1 = plateau.position2;

        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(plateau, "Move Point");
            EditorUtility.SetDirty(plateau);
            plateau.position2 = p1;
        }

        plateau.gameObject.transform.localScale = Vector3.one;
    }

    public override void OnInspectorGUI()
    {
        PlateauBeweger plateau = (PlateauBeweger)target;

        if (GUILayout.Button("uitleg"))
        {
            if (clicked)
                clicked = false;
            else
                clicked = true;
        }

        if(clicked)
        {
            EditorGUILayout.HelpBox("Dit script kan een plateau doen bewegen tussen twee punten. \nJe bepaalt de twee uiteindes van de beweging met de twee 'positie' punten en bepaald vervolgens de snelheid met de 'snelheid' slider. \nJe kunt je eigen plateau toevoegen door het simpelweg in het 'plateauobject' object te slepen", MessageType.Info);
        }



        //plateau.position1 = EditorGUILayout.Vector3Field("positie 1", plateau.position1);
        //plateau.position2 = EditorGUILayout.Vector3Field("positie 2", plateau.position2);


        //SerializedProperty positie1property = serializedObject.FindProperty("position1");

        //EditorGUILayout.Vector3Field("positie 1", positie1property);
        //serializedObject.ApplyModifiedProperties();


        //SerializedProperty speedproperty = serializedObject.FindProperty("speed");
        //EditorGUILayout.Slider(speedproperty,  0, 100);
        //serializedObject.ApplyModifiedProperties();



        //EditorGUI.BeginChangeCheck();
        //float snelheidvar = EditorGUILayout.Slider("sprintsnelheid", plateau.speed, 1, 50);
        //EditorUtility.SetDirty(plateau);
        //plateau.speed = snelheidvar;
        /*if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(plateau, "Move Point");
            EditorUtility.SetDirty(plateau);
            plateau.speed = snelheidvar;
        }*/
        

        EditorGUI.BeginChangeCheck();
        float snelheidvar = EditorGUILayout.Slider("sprintsnelheid", plateau.timeToReach, 1, 50);
        float pauzevar = EditorGUILayout.Slider("pauzetijd", plateau.hold, 0, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
            plateau.timeToReach = snelheidvar;
            plateau.hold = pauzevar;
        }
    }
}
