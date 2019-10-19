using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorExtensions
{
    public static class EditorFunctionCollections
    {
        public static void DrawLine(bool isDotted = false)
        {
            EditorGUILayout.Space();
            var rect = EditorGUILayout.BeginHorizontal();
            Handles.color = Color.black;
            if (isDotted)
            {
                Handles.DrawDottedLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y), 10f);
            }
            else
            {
                Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
    }
}