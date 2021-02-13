using JebsReadingGame.Helpers;
using JebsReadingGame.Systems.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Log
{
    public class LogService : MonoBehaviour
    {
        // Singleton
        static LogService _singleton;
        public static LogService singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<LogService>();

                return _singleton;
            }
        }

        public List<Log> gameplayLog = new List<Log>();

        [Header("Debug")]
        public string log;

        string fileName = "log.json";

        private void Awake()
        {
            Log("LOG INIT");
        }

        private void Start()
        {
            SceneView.singleton.onSceneChange.AddListener(OnSceneChange); // Right before leaving the scene
        }

        private void OnApplicationPause()
        {
            Save();
        }

        void OnSceneChange(string newScene)
        {
            Save();
        }

        void Save()
        {
            // Append and reset list
            FileHelpers.AppendJson<History>(fileName, new History(gameplayLog), ",\n");
            gameplayLog.Clear();

            DebugHelpers.Log("Saved!", ref log);
        }

        public void Log(string message)
        {
            gameplayLog.Add(new Log(message, Time.time, System.DateTime.Now));
        }
    }
}