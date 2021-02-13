using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using JebsReadingGame.Games;
using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(JebsReadingGame.Games.Chests.Debug))]
    public class DebugEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            JebsReadingGame.Games.Chests.Debug myScript = (JebsReadingGame.Games.Chests.Debug)target;
            if (GUILayout.Button("TOGGLE"))
            {
                myScript.ToggleDebugMode();
            }
        }
    }
#endif
