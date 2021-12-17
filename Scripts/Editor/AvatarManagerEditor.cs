using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AvatarManager))]
public class AvatarManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var controller = (AvatarManager)target;

        if (GUILayout.Button("ToggleAvatar"))
        {
            controller.ToggleAvatar();
        }

    }

}
