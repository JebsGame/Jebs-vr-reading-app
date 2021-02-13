using JebsReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Events;
using System;
using JebsReadingGame.Globals;

namespace JebsReadingGame.Systems.Scene
{
    public class SceneView : GlobalView
    {
        // Singleton
        static SceneView _singleton;
        public static SceneView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<SceneView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class SceneViewModel
        {
            SceneModel model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } }
            public int configurationValue { get { return model.asset.configurationValue; } } // placeholder

            public string currentScene { get { return model.currentScene; } }
            public SceneState state { get { return model.state; } }
            public float timeScale { get { return model.timeScale; } }
            public string[] availableScenes { get { return model.availableScenes.ToArray(); } }

            // Constructor
            public SceneViewModel(SceneModel model)
            {
                this.model = model;
            }
        }

        public SceneViewModel viewModel;

        // Events
        public StringEvent onSceneChange = new StringEvent();
        public SceneStateEvent onStateChange = new SceneStateEvent();
        public FloatEvent onTimeScaleChange = new FloatEvent();

    }
}