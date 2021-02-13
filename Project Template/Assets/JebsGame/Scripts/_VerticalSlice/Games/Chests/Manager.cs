using JebsReadingGame.Globals;
using JebsReadingGame.Helpers;
using JebsReadingGame.Letters;
using JebsReadingGame.Systems.Engagement;
using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Systems.Learning;
using JebsReadingGame.Systems.Progression;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JebsReadingGame.Games.Chests
{
    public class Manager : MonoBehaviour
    {
        // This class communicates with Gamemode System

        // Singleton
        static Manager _singleton;
        public static Manager singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<Manager>();

                return _singleton;
            }
        }

        [Range(0.1f,0.25f)]
        public float minCorrectLetterProbability = 0.25f;
        [Range(0.25f, 0.5f)]
        public float maxCorrectLetterProbability = 0.75f;

        public bool followLetterGroupOrder = false;

        [Header("Difficulty")]
        public Difficulty minDifficulty;
        public Difficulty maxDifficulty;

        char _correctLetter;
        public char correctLetter { get { return letterSequence[correctLetterIndex]; } }

        [Header("Refs")]
        public ImageFiller comboBar;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        int incorrectCounter = 0;
        float correctLetterProbability = 0.5f;

        string letterSequence;
        int correctLetterIndex = 0;
        bool wasLetterWin;

        private void Start()
        {
            letterSequence = Environment.GetRandomizedLetterGroup(ProgressionView.singleton.viewModel.currentLetterGroup);
        }

        private void Update()
        {
            UpdateCorrectLetterProbability();

            UpdateLog();
            logPanel.text = log;
        }

        public void Evaluate(Letter letter, bool accepted)
        {
            if (IsCorrect(letter.letter) && accepted)
            {
                DebugHelpers.LogEvent("(GamemodeSys) onLetterWin!", ref inbox);
                GamemodeView.singleton.DoLetterWin(GamemodeView.singleton.viewModel.activity, letterSequence[correctLetterIndex]);

                NextLetter(wasLetterWin, true);

                wasLetterWin = true;
            }
            else if (!IsCorrect(letter.letter) && !accepted)
            {
                GamemodeView.singleton.onTip.Invoke();
            }
            else
            {
                DebugHelpers.LogEvent("(GamemodeSys) onLetterFail!", ref inbox);
                GamemodeView.singleton.DoLetterFail(GamemodeView.singleton.viewModel.activity, letterSequence[correctLetterIndex]);

                NextLetter(wasLetterWin, false);

                wasLetterWin = false;
            }

            Service.singleton.ResumeGame();
        }

        void NextLetter(bool previousWasWin, bool latestWasWin)
        {
            LetterGroup letterGroup = ProgressionView.singleton.viewModel.currentLetterGroup;

            if (latestWasWin)
            {
                // If continuing positive combo
                if (previousWasWin)
                {
                    // If letter group uncompleted, continue in the existing randomized letter group
                    // If completed, DoLetterGroupWin

                    if (correctLetterIndex < letterSequence.Length - 1)
                    {
                        inbox = "You're doing well! " + (correctLetterIndex + 1) + "/" + letterSequence.Length; ;

                        correctLetterIndex++;

                        comboBar.SetFillAmount((float)correctLetterIndex/(float)letterSequence.Length);
                    }
                    else
                    {
                        DebugHelpers.LogEvent("(GamemodeSys) onLetterGroupWin!", ref inbox);
                        GamemodeView.singleton.DoLetterGroupWin(GamemodeView.singleton.viewModel.activity, letterGroup);

                        ReloadSequence(letterGroup);

                        comboBar.SetFillAmount(0.0f);
                    }
                }
                // If negative combo broke
                else
                {
                    // Reload randomized letter group or get the correct order
                    // Set index to 0

                    inbox = "Phew! You broke a negative combo!";

                    ReloadSequence(letterGroup);

                    comboBar.SetFillAmount(0.5f / (float)letterSequence.Length);
                }
            }
            else
            {
                // If postive combo broke
                if (previousWasWin)
                {
                    // Reload randomized letter group or get the correct order
                    // Set index to 0

                    inbox = "Oh no! You broke a positive combo!";

                    ReloadSequence(letterGroup);

                    comboBar.SetFillAmount(0.0f);
                }
                // If continuing negative combo
                else
                {
                    // If letter group uncompleted, continue in the existing randomized letter group
                    // If completed, DoLetterGroupFail

                    if (correctLetterIndex < letterSequence.Length - 1)
                    {
                        inbox = "Oh really? You're still failing! " + (correctLetterIndex + 1) + "/" + letterSequence.Length;

                        correctLetterIndex++;
                    }
                    else
                    {
                        DebugHelpers.LogEvent("(GamemodeSys) onLetterGroupFail!", ref inbox);
                        GamemodeView.singleton.DoLetterGroupFail(GamemodeView.singleton.viewModel.activity, letterGroup);

                        ReloadSequence(letterGroup);
                    }
                }
            }
        }

        public void RandomizeLetter(Letter letter)
        {
            LetterService.singleton.RandomizeStyle(letter);

            string incorrectLetters = Environment.FromLetterGroupToString(ProgressionView.singleton.viewModel.currentLetterGroup).Replace(letterSequence[correctLetterIndex].ToString(), "");

            if (Random.value < correctLetterProbability)
            {
                letter.letter = letterSequence[correctLetterIndex];
                incorrectCounter = 0;
            }
            else
            {
                letter.letter = incorrectLetters[Random.Range(0, incorrectLetters.Length - 1)];
                incorrectCounter++;
            }
        }

        public void DoSkillWin()
        {
            GamemodeView.singleton.onSkillWin.Invoke(GamemodeView.singleton.viewModel.activity);
        }

        public void DoSkillFail()
        {
            GamemodeView.singleton.onSkillFail.Invoke(GamemodeView.singleton.viewModel.activity);
        }

        void UpdateCorrectLetterProbability()
        {
            if (incorrectCounter >= (int)Mathf.Lerp(minDifficulty.maxIncorrectLetters, maxDifficulty.maxIncorrectLetters, EngagementView.singleton.viewModel.currentDifficultyLerp))
                correctLetterProbability = 1.0f;
            else
            {
                if (EngagementView.singleton.viewModel.currentDifficultyLerp > 0.5f)
                    correctLetterProbability = minCorrectLetterProbability;
                else
                    correctLetterProbability = maxCorrectLetterProbability;
            }
        }

        bool IsCorrect(char c)
        {
            return c == letterSequence[correctLetterIndex];
        }

        void ReloadSequence(LetterGroup letterGroup)
        {
            if (followLetterGroupOrder) letterSequence = Environment.FromLetterGroupToString(letterGroup);
            else letterSequence = Environment.GetRandomizedLetterGroup(letterGroup);

            correctLetterIndex = 0;
        }

        void UpdateLog()
        {
            Activity activity = GamemodeView.singleton.viewModel.activity;
            LetterLearningState letterState = LearningView.singleton.viewModel.GetLearningState(activity, correctLetter);

            log = "Game Manager\n"
                + "Shuffled letter group: " + letterSequence + "\n"
                + "Correct letter probability: " + (correctLetterProbability * 100.0f).ToString("F1") + "%\n"
                + "Max incorrect random letters: " + (int)Mathf.Lerp(minDifficulty.maxIncorrectLetters, maxDifficulty.maxIncorrectLetters, EngagementView.singleton.viewModel.currentDifficultyLerp) + "\n"
                + "Counter incorrect random letters: " + incorrectCounter + "\n"
                + "\t Letter: " + correctLetter.ToString() + "\n"
                + "\t\t Learning lerp: " + (letterState.learningScore * 100.0f).ToString("F1") + "%\n"
                + "\t\t Total wins: " + letterState.totalWins + " - Total fails: " + letterState.totalFails + "\n"
                + "Inbox: " + inbox;
        }
    }
}
