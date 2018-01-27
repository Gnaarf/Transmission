using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SegmentStart))]
public class SegmentEditor : Editor {


    public override void OnInspectorGUI()
    {
        SegmentStart start = (SegmentStart)target;
        if(GUILayout.Button("Generate Connections"))
        {
            Debug.Log("Go Generate some line");
            start.GenerateConnections();
        }

        base.OnInspectorGUI();
    }
}
