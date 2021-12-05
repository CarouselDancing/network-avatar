using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackerTarget))]
public class TrackerTargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var controller = (TrackerTarget)target;

        if (GUILayout.Button("SetOffset"))
        {
            controller.SetOffset();
        }

    }

}
