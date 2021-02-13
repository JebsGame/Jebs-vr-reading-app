using JebsReadingGame.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Learning
{
    [Serializable]
    public class LetterLearningState
    {
        public char letter;
        public float totalWins;
        public float totalFails;
        public float learningScore;

        public LetterLearningState(char letter)
        {
            this.letter = letter;
        }

        public LetterLearningState(LetterLearningState state)
        {
            letter = state.letter;
            totalWins = state.totalWins;
            totalFails = state.totalFails;
            learningScore = state.learningScore;
        }
    }

    [Serializable]
    public class LetterGroupLearningState
    {
        public LetterGroup letterGroup;
        public float totalWins;
        public float totalFails;
        public float learningScore;

        public LetterGroupLearningState(LetterGroup letterGroup)
        {
            this.letterGroup = letterGroup;
        }

        public LetterGroupLearningState(LetterGroupLearningState state)
        {
            letterGroup = state.letterGroup;
            totalWins = state.totalWins;
            totalFails = state.totalFails;
            learningScore = state.learningScore;
        }
    }

    [Serializable]
    public class ActivityLearningState
    {
        public Activity activity;
        public LetterLearningState[] letters;
        public LetterGroupLearningState[] letterGroups;

        public ActivityLearningState(Activity activity)
        {
            // Lists init
            List<LetterLearningState> letters = new List<LetterLearningState>();
            List<LetterGroupLearningState> letterGroups = new List<LetterGroupLearningState>();

            // Letters
            for (int i = 0; i < Globals.Environment.alphabet.Length; i++)
            {
                letters.Add(new LetterLearningState(Globals.Environment.alphabet[i]));
            }

            // Letter groups
            letterGroups.Add(new LetterGroupLearningState(LetterGroup.AtoG));
            letterGroups.Add(new LetterGroupLearningState(LetterGroup.HtoM));
            letterGroups.Add(new LetterGroupLearningState(LetterGroup.NtoT));
            letterGroups.Add(new LetterGroupLearningState(LetterGroup.UtoZ));

            //
            this.activity = activity;
            this.letters = letters.ToArray();
            this.letterGroups = letterGroups.ToArray();
        }
    }

    [Serializable]
    public class LearningState
    {
        public ActivityLearningState[] activities;

        public LearningState()
        {
            // Lists init
            List<ActivityLearningState> activities = new List<ActivityLearningState>();

            // Activities
            activities.Add(new ActivityLearningState(Activity.LetterRecognition));
            activities.Add(new ActivityLearningState(Activity.LetterSequencing));
            activities.Add(new ActivityLearningState(Activity.LetterMissing));
            activities.Add(new ActivityLearningState(Activity.LetterPairing));

            //
            this.activities = activities.ToArray();
        }
    }
}
