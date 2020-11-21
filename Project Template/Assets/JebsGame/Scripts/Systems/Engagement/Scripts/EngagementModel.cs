﻿using JesbReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JesbReadingGame.Helpers;

namespace JebsReadingGame.System.Engagement
{
    public class EngagementModel : GlobalModel
    {
        [HideInInspector]
        public EngagementView view; // Optional

        public EngagementConfigurationAsset asset;

        public EngagementPersistent persistent = new EngagementPersistent(/*Application.persistentDataPath + "/DifficultyState.json"*/);

        [Header("References")]
        public Transform head;
        public Transform leftHand;
        public Transform rightHand;

        [Header("Updated by Controller")]
        public float realtimeEngagementEstimation;
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
            FileHelpers.WriteJson<DifficultyState>(fileName, _state);
        }
    }

    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Engagement Configuration Asset", order = 1)]
    public class EngagementConfigurationAsset : ScriptableObject
    {
        public int configurationValue = 0;
    }
}
