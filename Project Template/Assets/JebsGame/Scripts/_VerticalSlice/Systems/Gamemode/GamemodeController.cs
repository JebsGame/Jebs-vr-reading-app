using JebsReadingGame.Globals;
using JebsReadingGame.Skeletons;
using JebsReadingGame.Systems.Engagement;
using JebsReadingGame.Systems.Learning;
using JebsReadingGame.Systems.Progression;
using JebsReadingGame.Systems.Scene;
using JebsReadingGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JebsReadingGame.Log;

namespace JebsReadingGame.Systems.Gamemode
{
    public class GamemodeController : GlobalController
    {
        public GamemodeModel model;
        public GamemodeView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        private void Awake()
        {
            if (!model)
                model = GetComponent<GamemodeModel>();

            if (!view)
                view = GetComponent<GamemodeView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new GamemodeView.GamemodeViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            // Listen to events invoked by other systems
            LearningView.singleton.onForgottenSkill.AddListener(OnForgottenSkill);
            SceneView.singleton.onSceneChange.AddListener(OnSceneChange);

            // Get letter group
            Debug.Log("LETTER-GROUP: " + ProgressionView.singleton.viewModel.currentLetterGroup);

            // Get initial difficulty for current activity
            Debug.Log("ACTIVITY DIFFICULTY LERP: " + EngagementView.singleton.viewModel.currentDifficultyLerp);

            // Debug - Get learning lerp for this activity and this lettter group
            Debug.Log("LETTER-GROUP LEARNING LERP: " + LearningView.singleton.viewModel.currentLetterGroupLearningLerp);

        }

        private void Update()
        {
            UpdateLog();
            logPanel.text = log;

            if (model.currentLetterGroupCombo >= model.asset.letterGroupStreakLength)
            {
                view.onPositiveLetterGroupStreakCompleted.Invoke();

                LogService.singleton.Log("POSITIVE LETTER GROUP STREAK COMPLETED! Activity: " + model.activity.ToString() + " - Letter Group: " + ProgressionView.singleton.viewModel.currentLetterGroup.ToString());

                model.currentLetterGroupCombo = 0;
            }

            if (model.currentLetterGroupCombo <= -model.asset.letterGroupStreakLength)
            {
                view.onNegativeLetterGroupStreakCompleted.Invoke();

                LogService.singleton.Log("NEGATIVE LETTER GROUP STREAK COMPLETED! Activity: " + model.activity.ToString() + " - Letter Group: " + ProgressionView.singleton.viewModel.currentLetterGroup.ToString());

                model.currentLetterGroupCombo = 0;
            }
        }

        private void OnApplicationPause()
        {
            // model.persistent.Save();
        }

        void OnForgottenSkill(Activity activity, LetterGroup letterGroup)
        {
            DebugHelpers.Log($"Player has forgotten the skill related to gamemode {activity.ToString()}! Let's stop time and start minigame for that gamemode", ref inbox);

            // ...
        }

        void OnSceneChange(string nextScene)
        {
            // ...
        }

        void UpdateLog()
        {
            log = "Gamemode System\n"
                + "Activity: " + view.viewModel.activity + "\n"
                + "Letter wins: " + view.viewModel.gameplayLetterWins + "\n"
                + "Letter fails: " + view.viewModel.gameplayLetterFails + "\n"
                + "Letter group combo: " + view.viewModel.currentLetterGroupCombo + "\n"
                + "Letter group wins: " + view.viewModel.gameplayLetterGroupWins + "\n"
                + "Letter group fails: " + view.viewModel.gameplayLetterGroupFails + "\n"
                + "Letter group streak length: " + view.viewModel.letterGroupStreakLength+ "\n"
                + "Inbox: " + inbox;
        }
    }
 
}
