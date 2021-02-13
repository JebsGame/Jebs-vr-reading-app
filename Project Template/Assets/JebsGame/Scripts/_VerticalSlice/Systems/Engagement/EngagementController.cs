using JebsReadingGame.Globals;
using JebsReadingGame.Helpers;
using JebsReadingGame.Player;
using JebsReadingGame.Skeletons;
using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Systems.Scene;
using TMPro;
using UnityEngine;

namespace JebsReadingGame.Systems.Engagement
{
    public class EngagementController : GlobalController
    {
        public EngagementModel model;
        public EngagementView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        float previousEngagmentEstimation = 0.5f;

        float timeBetweenDistractors = 10.0f;
        float timeSinceLastDistractor = 0.0f;

        private void Awake()
        {
            if (!model)
                model = GetComponent<EngagementModel>();

            if (!view)
                view = GetComponent<EngagementView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new EngagementView.EngagementViewModel(model);

            model.view = view; // Optional
        }

        private void Start()
        {
            GamemodeView.singleton.onSkillWin.AddListener(OnSkillWin);
            GamemodeView.singleton.onSkillFail.AddListener(OnSkillFail);

            GamemodeView.singleton.onPositiveLetterGroupStreakCompleted.AddListener(OnPositiveStreak);
            GamemodeView.singleton.onNegativeLetterGroupStreakCompleted.AddListener(OnNegativeStreak);

            SceneView.singleton.onSceneChange.AddListener(OnSceneChange); // Right before leaving the scene
        }

        private void Update()
        {
            model.currentLocomotiveActivity = Mathf.InverseLerp(0.0f, model.asset.maxLocomotiveSpeed * 3.0f, PlayerService.singleton.headSpeed + PlayerService.singleton.leftHandSpeed + PlayerService.singleton.rightHandSpeed);

            // Update engagement estimation
            previousEngagmentEstimation = model.currentStressEstimation;

            // Placeholder of algorithm for engagement estimation
            model.currentStressEstimation = (model.currentLocomotiveActivity + (0.5f + (Mathf.Sin(Time.time * 0.2f) / 2.0f))) / 2.0f;

            // Emit engagement events
            EmitEvents(previousEngagmentEstimation, model.currentStressEstimation);

            // Ask for distractors (the more bored, the more distractors)
            timeBetweenDistractors = model.asset.minTimeBetweenDistractors + Mathf.Lerp(0.0f,model.asset.rangeForTimeBetweenDistractors, 1 - model.currentStressEstimation);

            if (timeSinceLastDistractor > timeBetweenDistractors)
            {
                DebugHelpers.LogEvent("onDistractorRequired!", ref inbox);
                view.onDistractorRequired.Invoke();

                timeSinceLastDistractor = 0.0f;
            }
            else
                timeSinceLastDistractor += Time.deltaTime;

            UpdateLog();
            logPanel.text = log;
        }

        private void OnApplicationPause()
        {
            model.persistent.Save();
            DebugHelpers.Log("Saved!", ref inbox);
        }

        void OnSkillWin(Activity activity)
        {
            // Letter group is ignored. We use difficulty at activity level

            ActivityDifficultyState activityState = FindActivity(model.persistent.state.activities, activity);

            activityState.difficultyLerp += model.asset.skillWinEffect;
            activityState.difficultyLerp = Mathf.Clamp(activityState.difficultyLerp, 0.0f, 1.0f);

            // Save only before leaving the scene
        }

        void OnSkillFail(Activity activity)
        {
            // Letter group is ignored. We use difficulty at activity level

            ActivityDifficultyState activityState = FindActivity(model.persistent.state.activities, activity);

            activityState.difficultyLerp += model.asset.skillFailEffect;
            activityState.difficultyLerp = Mathf.Clamp(activityState.difficultyLerp, 0.0f, 1.0f);

            // Save only before leaving the scene
        }

        void OnPositiveStreak()
        {
            ActivityDifficultyState activityState = FindActivity(model.persistent.state.activities, GamemodeView.singleton.viewModel.activity);

            activityState.difficultyLerp += model.asset.streakDifficultyRate;
            activityState.difficultyLerp = Mathf.Clamp(activityState.difficultyLerp, 0.0f, 1.0f);

            // Save only before leaving the scene
        }

        void OnNegativeStreak()
        {
            ActivityDifficultyState activityState = FindActivity(model.persistent.state.activities, GamemodeView.singleton.viewModel.activity);

            activityState.difficultyLerp -= model.asset.streakDifficultyRate;
            activityState.difficultyLerp = Mathf.Clamp(activityState.difficultyLerp, 0.0f, 1.0f);

            // Save only before leaving the scene
        }

        void EmitEvents(float previousStress, float newStress)
        {
            if (previousStress < 1.0f && newStress >= 1.0f)
            {
                DebugHelpers.LogEvent("EMITS onFurstrated!",ref inbox);
                view.onFrustrated.Invoke();
            }
            else if (previousStress > 0.0f && newStress <= 0.0f)
            {
                DebugHelpers.LogEvent("EMITS onBored!", ref inbox);
                view.onBored.Invoke();
            }
            else if (previousStress > 0.6f && (newStress <= 0.6f && newStress >= 0.4f))
            {
                DebugHelpers.LogEvent("EMITS onEngaged!", ref inbox);
                view.onEngaged.Invoke();
            }
            else if (previousStress < 0.4f && (newStress <= 0.6f && newStress >= 0.4f))
            {
                DebugHelpers.LogEvent("EMITS onEngaged!", ref inbox);
                view.onEngaged.Invoke();
            }
        }

        void OnSceneChange(string nextScene)
        {
            model.persistent.Save();
        }

        ActivityDifficultyState FindActivity(ActivityDifficultyState[] activities, Activity activity)
        {
            for (int i = 0; i < activities.Length; i++)
            {
                if (activities[i].activity == activity)
                    return activities[i];
            }

            DebugHelpers.LogError("Activity not found!", ref inbox);

            return null;
        }

        void UpdateLog()
        {
            string reaction;

            if (view.viewModel.currentStressEstimation <= 0.2f)
                reaction = "Booring!";
            else if (view.viewModel.currentStressEstimation > 0.2f && view.viewModel.currentStressEstimation <= 0.4)
                reaction = "Meh";
            else if (view.viewModel.currentStressEstimation > 0.4f && view.viewModel.currentStressEstimation <= 0.6)
                reaction = "I like this!";
            else if (view.viewModel.currentStressEstimation > 0.6f && view.viewModel.currentStressEstimation <= 0.8)
                reaction = "Challenging";
            else
                reaction = "Too hard!";

            log = "Engagement System\n"
                + "Stress: " + (view.viewModel.currentStressEstimation * 100.0f).ToString("F1") + "% (" + reaction + ")\n"
                + "Locomotion: " + (view.viewModel.currentLocomotiveActivity * 100.0f).ToString("F1") + "%\n"
                + "Difficulty: " + (view.viewModel.currentDifficultyLerp * 100.0f).ToString("F1") + "%\n"
                + "Inbox: " + inbox;
        }
    }
  
}
