using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.Systems.Progression;
using JebsReadingGame.Systems.Gamemode;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JebsReadingGame.Systems.Progression
{
    public class ProgressionDebugTool : MonoBehaviour
    {
        public ProgressionController controller;

        [TextArea(15, 20)]
        public string log;

        private void Start()
        {
            controller.view.onLevelUp.AddListener(OnLevelUp);
            controller.view.onGamemodeGroupUnlocked.AddListener(OnGamemodeGroupUnlocked);
            controller.view.onGamemodeUnlocked.AddListener(OnGamemodeUnlock);
            controller.view.onGameFinished.AddListener(OnGameFinished);
            controller.view.onGamemodeBroken.AddListener(OnGamemodeBroken);

            GamemodeView.singleton.onGamemodeRepaired.AddListener(OnGamemodeRepaired);

            log = "(Listening to ProgressionView events...)";
            log += "\n";
        }

        void OnLevelUp(GamemodeGroup gmdGroup, Gamemode gmd, Level lvl)
        {
            log += "Level unlocked! Now you can play " + gmd.activity.ToString() + " for " + lvl.letterGroup.ToString();
            log += "\n";
        }

        void OnGamemodeGroupUnlocked(GamemodeGroup gmdGroup)
        {
            log += "Gamemode group unlocked!";
            log += "\n";
        }

        void OnGamemodeUnlock(GamemodeGroup gmdGroup, Gamemode gmd)
        {
            log += "Gamemode unlocked! Now you can play " + gmd.activity.ToString();
            log += "\n";
        }

        void OnGameFinished()
        {
            log += "Congratulations! You finished the game!";
            log += "\n";
        }

        void OnGamemodeBroken(GamemodeGroup gmdGroup, Gamemode gmd)
        {
            log += "Gamemode for activity " + gmd.activity.ToString() + " broke!";
            log += "\n";
        }

        void OnGamemodeRepaired(GamemodeGroup gmdGroup, Gamemode gmd)
        {
            log += "Gamemode for activity " + gmd.activity.ToString() + " was repaired!";
            log += "\n";
        }

        public void DoNext()
        {
            controller.Next();
        }

        public void DoReset()
        {
            controller.Reset();
        }

        public void DoClear()
        {
            log = "";
        }
    }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(ProgressionDebugTool))]
    public class ProgressionDebugTool_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ProgressionDebugTool myScript = (ProgressionDebugTool)target;

            if (GUILayout.Button("NEXT"))
            {
                myScript.DoNext();
            }
            if (GUILayout.Button("RESET"))
            {
                myScript.DoReset();
            }
            if (GUILayout.Button("CLEAR"))
            {
                myScript.DoClear();
            }
        }
        
    }
#endif

