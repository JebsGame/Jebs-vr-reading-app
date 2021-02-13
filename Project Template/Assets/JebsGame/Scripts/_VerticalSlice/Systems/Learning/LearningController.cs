using JebsReadingGame.Globals;
using JebsReadingGame.Systems.Engagement;
using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Skeletons;
using JebsReadingGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JebsReadingGame.Systems.Progression;

namespace JebsReadingGame.Systems.Learning
{
    public class LearningController : GlobalController
    {
        public LearningModel model;
        public LearningView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        private void Awake()
        {
            if (!model)
                model = GetComponent<LearningModel>();

            if (!view)
                view = GetComponent<LearningView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new LearningView.LearningViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            GamemodeView.singleton.onLetterGroupWin.AddListener(OnLetterGroupWin);
            GamemodeView.singleton.onLetterGroupFail.AddListener(OnLetterGroupFail);
            GamemodeView.singleton.onLetterWin.AddListener(OnLetterWin);
            GamemodeView.singleton.onLetterFail.AddListener(OnLetterFail);
        }

        private void Update()
        {
            UpdateLog();
            logPanel.text = log;
        }

        private void OnApplicationPause()
        {
            model.persistent.Save();
            DebugHelpers.Log("Saved!", ref inbox);
        }

        void OnLetterGroupWin(Activity activity, LetterGroup letterGroup)
        {
            ActivityLearningState activityState = FindActivity(model.persistent.state.activities,activity);
            LetterGroupLearningState letterGroupState = FindLetterGroup(activityState.letterGroups,letterGroup);

            letterGroupState.totalWins++;
            letterGroupState.learningScore = CalculateLearningScore(letterGroupState);

            model.persistent.Save();

            if (letterGroupState.learningScore >= model.asset.letterGroupMasteredOver)
            {
                DebugHelpers.LogEvent("Level mastered! The player is good at " + activity.ToString() + " for " + letterGroup.ToString(), ref inbox);
                view.onLevelMastered.Invoke(activity, letterGroup);
            }
        }

        void OnLetterGroupFail(Activity activity, LetterGroup letterGroup)
        {
            ActivityLearningState activityState = FindActivity(model.persistent.state.activities, activity);
            LetterGroupLearningState letterGroupState = FindLetterGroup(activityState.letterGroups, letterGroup);

            letterGroupState.totalFails++;
            letterGroupState.learningScore = CalculateLearningScore(letterGroupState);

            model.persistent.Save();

            if (letterGroupState.learningScore <= model.asset.letterGroupForgottenUnder)
            {
                DebugHelpers.LogEvent("Level forgotten! The player is very bad at " + activity.ToString() + " for " + letterGroup.ToString(), ref inbox);
                view.onForgottenSkill.Invoke(activity, letterGroup);
            }
        }

        void OnLetterWin(Activity activity, char letter)
        {
            ActivityLearningState activityState = FindActivity(model.persistent.state.activities, activity);
            LetterLearningState letterState = FindLetter(activityState.letters, letter);

            letterState.totalWins++;
            letterState.learningScore = CalculateLearningScore(letterState);

            DebugHelpers.Log("Lettter win -> Learning updated for letter " + letter + "!", ref inbox);
            model.persistent.Save();
        }

        void OnLetterFail(Activity activity, char letter)
        {
            ActivityLearningState activityState = FindActivity(model.persistent.state.activities, activity);
            LetterLearningState letterState = FindLetter(activityState.letters, letter);

            letterState.totalFails++;
            letterState.learningScore = CalculateLearningScore(letterState);

            DebugHelpers.Log("Lettter fail -> Learning updated for letter " + letter + "!", ref inbox);
            model.persistent.Save();
        }

        // To do: Implement log/registry and register time elapsed between letter group win/fails
        float CalculateLearningScore(LetterLearningState letterState)
        {
            if (letterState.totalWins + letterState.totalFails > model.asset.updateLearningScoreFrom)
                return letterState.totalWins / (letterState.totalWins + letterState.totalFails);
            else
                return 0.0f;
        }
        float CalculateLearningScore(LetterGroupLearningState letterGroupState)
        {
            if (letterGroupState.totalWins + letterGroupState.totalFails > model.asset.updateLearningScoreFrom)
                return letterGroupState.totalWins / (letterGroupState.totalWins + letterGroupState.totalFails);
            else
                return 0.0f;
        }

        ActivityLearningState FindActivity(ActivityLearningState[] activities,Activity activity)
        {
            for (int i = 0; i < activities.Length; i++)
            {
                if (activities[i].activity == activity)
                    return activities[i];
            }

            DebugHelpers.LogError("Activity not found!", ref inbox);

            return null;
        }

        LetterGroupLearningState FindLetterGroup(LetterGroupLearningState[] letterGroups, LetterGroup letterGroup)
        {
            for (int i = 0; i < letterGroups.Length; i++)
            {
                if (letterGroups[i].letterGroup == letterGroup)
                    return letterGroups[i];
            }

            DebugHelpers.LogError("Letter group not found!", ref inbox);

            return null;
        }

        LetterLearningState FindLetter(LetterLearningState[] letters, char letter)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i].letter == letter)
                    return letters[i];
            }

            DebugHelpers.LogError("Letter not found!", ref inbox);

            return null;
        }

        void UpdateLog()
        {
            Activity activity = GamemodeView.singleton.viewModel.activity;
            LetterGroup letterGroup = ProgressionView.singleton.viewModel.currentLetterGroup;
            LetterGroupLearningState letterGroupState = view.viewModel.GetLearningState(activity, letterGroup);

            log = "Learning System\n"
                + "Activity: " + activity.ToString() + "\n"
                + "\t Letter group: " + letterGroup.ToString() + "\n"
                + "\t\t Learning lerp: " + (letterGroupState.learningScore * 100.0f).ToString("F1") + "%\n"
                + "\t\t Total wins: " + letterGroupState.totalWins + " - Total fails: " + letterGroupState.totalFails + "\n"
                + "From worst to best: " + view.viewModel.GetLettersFromWorstToBest(activity, letterGroup) + "\n"
                + "Inbox: " + inbox;
        }

    }

}