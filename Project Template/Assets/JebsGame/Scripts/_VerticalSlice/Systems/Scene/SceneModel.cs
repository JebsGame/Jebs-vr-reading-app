using JebsReadingGame.Skeletons;
using UnityEngine;
using JebsReadingGame.Globals;
using System.Collections.Generic;

namespace JebsReadingGame.Systems.Scene
{
    public class SceneModel : GlobalModel
    {
        [HideInInspector]
        public SceneView view; // Optional

        public SceneConfiguration asset;

        public ScenePersistent persistent = new ScenePersistent();

        public string currentScene;
        public List<string> availableScenes = new List<string>();

        [Header("Updated by Controller")]
        public SceneState state;
        public float timeScale = 1.0f;
    }

    // Persistent model: Persistent between scenes
    public class ScenePersistent
    {
        string persistentValueKey = "key";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }
    }
}
