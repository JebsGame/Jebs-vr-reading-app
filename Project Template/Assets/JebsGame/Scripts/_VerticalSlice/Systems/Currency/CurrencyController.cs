using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Systems.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.Systems.Currency;
using JebsReadingGame.Globals;
using TMPro;
using JebsReadingGame.Helpers;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JebsReadingGame.Systems.Currency
{
    public class CurrencyController : MonoBehaviour
    {
        public CurrencyModel model;
        public CurrencyView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        private void Awake()
        {
            view.viewModel = new CurrencyView.CurrencyViewModel(model);
            model.view = view;
            model.persistent.Load();
        }

        private void Start()
        {
            // Subscribe to events
            GamemodeView.singleton.onLetterWin.AddListener(OnLetterWin);
            GamemodeView.singleton.onLetterGroupWin.AddListener(OnLetterGroupWin);
            GamemodeView.singleton.onPositiveLetterGroupStreakCompleted.AddListener(OnPositiveStreak);
            GamemodeView.singleton.onTip.AddListener(OnTip);
            Scene.SceneView.singleton.onSceneChange.AddListener(SceneChanged);
        }

        private void Update()
        {
            UpdateLog();   
            logPanel.text = log;
        }

        private void OnApplicationPause()
        {
            model.persistent.Save();
        }

        void SceneChanged(string sceneName)
        {
            model.persistent.Save();
        }

        void OnLetterWin(Activity activity, char letter)
        {
            EarnCoins(model.asset.letterWinPrice);
        }

        void OnLetterGroupWin(Activity activity, LetterGroup letterGroup)
        {
            EarnCoins(model.asset.letterGroupWinPrice);
        }

        void OnPositiveStreak()
        {
            EarnCoins(model.asset.positiveStreakPrice);
        }

        void OnTip()
        {
            EarnCoins(model.asset.tipPrice);
        }
        
        void EarnCoins(int coins)
        {
            model.persistent.totalCoins += coins;

            DebugHelpers.LogEvent("onFurstrated!", ref inbox);
            view.onCoinsEarned.Invoke(coins);
        }

        public void ResetTotalCoins()
        {
            model.persistent.totalCoins = 0;
        }

        void UpdateLog()
        {
            log = "Currency System\n"
                + "(P) Total coins: " + model.persistent.totalCoins + "\n"
                + "Inbox: " + inbox;
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CurrencyController))]
public class CurrencySystemControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CurrencyController myScript = (CurrencyController)target;
        if (GUILayout.Button("RESET TOTAL COINS"))
        {
            myScript.ResetTotalCoins();
        }
    }
}
#endif

