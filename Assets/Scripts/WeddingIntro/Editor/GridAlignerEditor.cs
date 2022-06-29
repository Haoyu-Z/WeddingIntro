using UnityEditor;
using UnityEngine;
using WeddingIntro.Utility;

namespace WeddingIntro.Editor
{
    [CustomEditor(typeof(GridAligner))]
    public class GridAlignerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Align to Pixel Grid"))
            {
                (target as GridAligner).Align();
            }
        }
    }
}