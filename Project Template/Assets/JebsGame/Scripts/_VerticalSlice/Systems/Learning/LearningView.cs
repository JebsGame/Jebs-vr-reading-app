using JebsReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Events;
using System;
using JebsReadingGame.Globals;
using JebsReadingGame.Systems.Progression;
using JebsReadingGame.Systems.Gamemode;
using System.Linq;

namespace JebsReadingGame.Systems.Learning
{
    public class LearningView : GlobalView
    {
        // Singleton
        static LearningView _singleton;
        public static LearningView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<LearningView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class LearningViewModel
        {
            LearningModel model;

            // Properties
            public LearningPersistent persistent { get { return model.persistent; } }

            LetterGroupLearningState _currentLetterGroupLearning;
            public float currentLetterGroupLearningLerp
            {
                get
                {
                    if (_currentLetterGroupLearning == null)
                    {
                        _currentLetterGroupLearning = GetLearningState(GamemodeView.singleton.viewModel.activity,ProgressionView.singleton.viewModel.currentLetterGroup);
                    }
                    return _currentLetterGroupLearning.learningScore;
                }
            }

            // Constructor
            public LearningViewModel(LearningModel model)
            {
                this.model = model;
            }

            // Functions
            public LetterGroupLearningState GetLearningState(Activity activity, LetterGroup letterGroup)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letterGroups.Length; j++)
                        {
                            if (persistent.state.activities[i].letterGroups[j].letterGroup == letterGroup)
                                return new LetterGroupLearningState(persistent.state.activities[i].letterGroups[j]);
                        }
                    }
                }

                Debug.LogError("Activity or letter group not found!");

                return null;
            }

            public LetterLearningState GetLearningState(Activity activity, char letter)
            {
                for (int i = 0; i < persistent.state.activities.Length; i++)
                {
                    if (persistent.state.activities[i].activity == activity)
                    {
                        for (int j = 0; j < persistent.state.activities[i].letters.Length; j++)
                        {
                            if (persistent.state.activities[i].letters[j].letter == letter)
                                return new LetterLearningState(persistent.state.activities[i].letters[j]);
                        }
                    }
                }

                Debug.LogError("Activity or letter not found!");

                return null;
            }

            public string GetLettersFromWorstToBest(Activity activity, LetterGroup letterGroup)
            {
                string letters = Globals.Environment.FromLetterGroupToString(letterGroup);

                List<LetterLearningState> lettersLearningState = new List<LetterLearningState>();
                for (int i = 0; i < letters.Length; i++)
                {
                    lettersLearningState.Add(GetLearningState(activity, letters[i]));
                }

                lettersLearningState.OrderBy(x => x.learningScore);

                return new string(lettersLearningState.Select(x => x.letter).ToArray());
            }

            public char WorstLetter(Activity activity, LetterGroup letterGroup)
            {
                return GetLettersFromWorstToBest(activity, letterGroup)[0];
            }

        }

        public LearningViewModel viewModel;

        // Events
        public ActivityLetterGroupEvent onForgottenSkill = new ActivityLetterGroupEvent();
        public ActivityLetterGroupEvent onLevelMastered = new ActivityLetterGroupEvent();
    }
}
