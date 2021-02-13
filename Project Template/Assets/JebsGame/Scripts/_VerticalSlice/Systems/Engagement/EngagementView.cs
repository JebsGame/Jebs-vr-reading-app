using JebsReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Events;
using System;
using JebsReadingGame.Globals;
using JebsReadingGame.Systems.Gamemode;

namespace JebsReadingGame.Systems.Engagement
{
    public class EngagementView : GlobalView
    {
        // Singleton
        static EngagementView _singleton;
        public static EngagementView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<EngagementView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class EngagementViewModel
        {
            EngagementModel model;

            // Properties
            public EngagementPersistent persistent { get { return model.persistent; } }
            public float currentLocomotiveActivity { get { return model.currentLocomotiveActivity; } }
            public float currentStressEstimation { get { return model.currentStressEstimation; } }

            ActivityDifficultyState _currentDifficulty;
            public float currentDifficultyLerp
            {
                get
                {
                    if (_currentDifficulty == null)
                    {
                        _currentDifficulty = GetDifficulty(GamemodeView.singleton.viewModel.activity);
                    }
                    return _currentDifficulty.difficultyLerp;
                }
            }

            // Constructor
            public EngagementViewModel(EngagementModel model)
            {
                this.model = model;
            }

            // Functions
            ActivityDifficultyState GetDifficulty(Activity activity)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        return persistent.state.activities[i];
                    }
                }

                Debug.LogError("Difficulty lerp for that activity was not found!");

                return null;
            }
        }

        public EngagementViewModel viewModel;

        // Events
        public UnityEvent onBored = new UnityEvent();
        public UnityEvent onEngaged = new UnityEvent();
        public UnityEvent onFrustrated = new UnityEvent();

        public UnityEvent onDistractorRequired = new UnityEvent();
    }
}
