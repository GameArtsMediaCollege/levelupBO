using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FirstPersonController))]
public class FirstPersonCharacterEditor : Editor
{
    FirstPersonController charactercontrol;
    bool clicked;


    public override void OnInspectorGUI()
    {
        charactercontrol = target as FirstPersonController;
        EditorGUILayout.LabelField("Karakter Controle Script");

        if (GUILayout.Button("uitleg"))
        {
            if (clicked)
                clicked = false;
            else
                clicked = true;
        }

        if (clicked)
        {
            EditorGUILayout.HelpBox("Dit script bestuurt het karakter.\nHieronder kun je verschillende dingen veranderen zoals de loopsnelheid, de hoogte van de sprong en de hoeveelheid sprongen die het karakter kan maken\n speel met de knoppen hieronder en kijk wat voor invloed het heeft op de speler", MessageType.Info);
        }


        EditorGUI.BeginChangeCheck();
        float movespeed = EditorGUILayout.Slider("loopsnelheid", charactercontrol.MoveSpeed, 1, 50);
        float sprintspeed = EditorGUILayout.Slider("sprintsnelheid", charactercontrol.SprintSpeed, 1, 50);
        float jumpheight = EditorGUILayout.Slider("sprintsnelheid", charactercontrol.JumpHeight, 1, 50);
        int maxjump = EditorGUILayout.IntField("maximale hoeveelheid sprongen", charactercontrol.maxjump);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
            charactercontrol.MoveSpeed = movespeed;
            charactercontrol.SprintSpeed = sprintspeed;
            charactercontrol.JumpHeight = jumpheight;
            charactercontrol.maxjump = maxjump;
        }



    }
}
