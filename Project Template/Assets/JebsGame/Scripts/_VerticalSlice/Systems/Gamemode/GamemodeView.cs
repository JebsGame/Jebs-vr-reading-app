using JebsReadingGame.Skeletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JebsReadingGame.Events;
using JebsReadingGame.Globals;
using JebsReadingGame.Log;

namespace JebsReadingGame.Systems.Gamemode
{
    public class GamemodeView : GlobalView
    {
        // Singleton
        static GamemodeView _singleton;
        public static GamemodeView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<GamemodeView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class GamemodeViewModel
        {
            GamemodeModel model;

            // Properties
            public int persistentValue { get { return model.persistent.persistentValue; } } // placeholder
            public int letterGroupStreakLength { get { return model.asset.letterGroupStreakLength; } }

            public Activity activity { get { return model.activity; } }

            public int currentLetterGroupCombo { get { return model.currentLetterGroupCombo; } set { model.currentLetterGroupCombo = value; } }

            // public Item nextEquipableItem { get { return model.nextEquipableItem; } } -> Store system is not implemented yet

            public int gameplayLetterWins { get { return model.gameplayLetterWins; } set { model.gameplayLetterWins = value; } }
            public int gameplayLetterFails { get { return model.gameplayLetterFails; } set { model.gameplayLetterFails = value; } }
            public int gameplayLetterGroupWins { get { return model.gameplayLetterGroupWins; } set { model.gameplayLetterGroupWins = value; } }
            public int gameplayLetterGroupFails { get { return model.gameplayLetterGroupFails; } set { model.gameplayLetterGroupFails = value; } }

            // Constructor
            public GamemodeViewModel(GamemodeModel model)
            {
                this.model = model;
            }
        }
        public GamemodeViewModel viewModel;

        // Events
        public ActivityLetterGroupEvent onLetterGroupWin = new ActivityLetterGroupEvent();
        public ActivityLetterGroupEvent onLetterGroupFail = new ActivityLetterGroupEvent();

        public ActivityLetterEvent onLetterWin = new ActivityLetterEvent();
        public ActivityLetterEvent onLetterFail = new ActivityLetterEvent();

        public ActivityEvent onSkillWin = new ActivityEvent();
        public ActivityEvent onSkillFail = new ActivityEvent();

        public UnityEvent onTip = new UnityEvent(); // Small optional help

        public UnityEvent onPositiveLetterGroupStreakCompleted = new UnityEvent();
        public UnityEvent onPositiveLetterGroupStreakBroken = new UnityEvent();
        public UnityEvent onNegativeLetterGroupStreakCompleted = new UnityEvent();
        public UnityEvent onNegativeLetterGroupStreakBroken = new UnityEvent();

        public GamemodeEvent onGamemodeRepaired = new GamemodeEvent();

        public StringEvent onSceneChangeRequest = new StringEvent();

        // Function
        public void DoLetterGroupWin(Activity activity, LetterGroup letterGroup)
        {
            viewModel.gameplayLetterGroupWins++;

            onLetterGroupWin.Invoke(activity, letterGroup);

            LogService.singleton.Log("LETTER GROUP WIN! Activity: " + activity.ToString() + " - Letter Group: " + letterGroup.ToString());

            if (viewModel.currentLetterGroupCombo < 0)
            {
                viewModel.currentLetterGroupCombo = 1;

                onNegativeLetterGroupStreakBroken.Invoke();
            }
            else
                viewModel.currentLetterGroupCombo++;
        }

        public void DoLetterGroupFail(Activity activity, LetterGroup letterGroup)
        {
            viewModel.gameplayLetterGroupFails++;

            onLetterGroupFail.Invoke(activity, letterGroup);

            LogService.singleton.Log("LETTER GROUP FAIL! Activity: " + activity.ToString() + " - Letter Group: " + letterGroup.ToString());

            if (viewModel.currentLetterGroupCombo > 0)
            {
                viewModel.currentLetterGroupCombo = -1;

                onPositiveLetterGroupStreakBroken.Invoke();
            }
            else
                viewModel.currentLetterGroupCombo--;
        }

        public void DoLetterWin(Activity activity, char letter)
        {
            onLetterWin.Invoke(activity, letter);

            viewModel.gameplayLetterWins++;

            LogService.singleton.Log("LETTER WIN! Activity: " + activity.ToString() + " - Letter: " + letter.ToString());
        }

        public void DoLetterFail(Activity activity, char letter)
        {
            onLetterFail.Invoke(activity, letter);

            viewModel.gameplayLetterFails++;

            LogService.singleton.Log("LETTER WIN! Activity: " + activity.ToString() + " - Letter: " + letter.ToString());
        }

        public void DoSkillWin(Activity activity)
        {
            // ...
            onSkillWin.Invoke(activity);
        }

        public void DoSkillFail(Activity activity)
        {
            // ...
            onSkillFail.Invoke(activity);          
        }

    }
}
