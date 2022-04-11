using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridAligner))]
public class GridAlignerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Align to Pixel Grid"))
        {
            (target as GridAligner).Align();
        }
    }
}
