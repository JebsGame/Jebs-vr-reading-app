using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Skeletons;
using JebsReadingGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace JebsReadingGame.Systems.Scene
{
    public class SceneController : GlobalController
    {
        public SceneModel model;
        public SceneView view;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        private void Awake()
        {
            if (!model)
                model = GetComponent<SceneModel>();

            if (!view)
                view = GetComponent<SceneView>();

            if (!model || !view)
            {
                Debug.LogError("Missing system components!");
                gameObject.SetActive(false);
            }

            // Components connection
            view.viewModel = new SceneView.SceneViewModel(model);

            model.view = view; // Optional

            model.state = Globals.SceneState.Loading;
        }

        private void Start()
        {
            GamemodeView.singleton.onSceneChangeRequest.AddListener(LoadScene);

            model.state = Globals.SceneState.Gameplay;
        }

        private void Update()
        {
            UpdateLog();
            logPanel.text = log;
        }

        private void OnApplicationPause()
        {
            // model.persistent.Save();
        }

        // ...

        public void LoadScene(string scene)
        {
            // Check that the scene name exists and can be teleported
            if (model.availableScenes.Contains(scene))
            {
                // Notify other that the scene will change in next frame
                DebugHelpers.LogEvent("onSceneChange! Scene will change in next frame", ref inbox);
                view.onSceneChange.Invoke(scene);

                // Wait 1 frame before changing the scene
                StartCoroutine(LoadSceneOnNextFrame(scene));
            }
        }

        IEnumerator LoadSceneOnNextFrame(string scene)
        {
            yield return new WaitForEndOfFrame();

            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        void UpdateLog()
        {
            log = "Scene System\n"
                + "Current scene: " + view.viewModel.currentScene + "\n"
                + "Scene state: " + view.viewModel.state.ToString() + "\n"
                + "Inbox: " + inbox;
        }
    }

}