using JebsReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JebsReadingGame.Helpers;

namespace JebsReadingGame.Systems.Engagement
{
    public class EngagementModel : GlobalModel
    {
        [HideInInspector]
        public EngagementView view; // Optional

        public EngagementConfiguration asset;

        public EngagementPersistent persistent = new EngagementPersistent();

        [Header("Updated by Controller")]
        [Range(0.0f,1.0f)]
        public float currentLocomotiveActivity = 0.0f;
        [Tooltip("0 = Bored, 0.5 = Engaged, 1 = Frustrated")]
        [Range(0.0f, 1.0f)]
        public float currentStressEstimation = 0.5f;
    }

    // Persistent model: Persistent between scenes
    public class EngagementPersistent
    {
        string fileName = "DifficultyState.json";
        bool resetOnError = true;

        DifficultyState _state;
        public DifficultyState state
        {
            get
            {
                if (_state == null)
                {
                    try
                    {
                        _state = FileHelpers.ReadJson<DifficultyState>(fileName);

                        if (_state == null)
                        {
                            Debug.LogWarning("State was empty. Resetting state!");
                            _state = new DifficultyState();
                            Save();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception found while getting state: " + e.Message);

                        if (resetOnError)
                        {
                            Debug.LogWarning("Resetting state!");
                            _state = new DifficultyState();
                            Save();
                        }
                    }

                }
                return _state;
            }
            set
            {
                _state = value;
                //SaveIntoJson(value);      Doing this every write operation is very expensive. Please call Save() before leaving the scene
            }
        }

        public void Save()
        {
            FileHelpers.ReplaceJson<DifficultyState>(fileName, _state);
        }
    }
}
