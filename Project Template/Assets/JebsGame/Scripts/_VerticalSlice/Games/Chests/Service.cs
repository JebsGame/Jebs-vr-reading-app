using JebsReadingGame.Helpers;
using JebsReadingGame.Pools;
using JebsReadingGame.Systems.Engagement;
using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JebsReadingGame.Games.Chests
{
    public class Service : MonoBehaviour
    {
        // Singleton
        static Service _singleton;
        public static Service singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<Service>();

                return _singleton;
            }
        }

        [Header("Chests")]
        public Transform stealedChestContainer;
        [Range(0.2f,3.0f)]
        public float stealingDuration = 1.0f;
        public Chest stealedChest;

        [Header("Crabs")]
        public List<Home> homes = new List<Home>();
        public Transform aliveCrabsContainer;
        public string poolTag = "crabs";
        [Range(1.0f, 10.0f)]
        public float crabsSpeed = 2.0f;
        [Range(0.0f, 10.0f)]
        public float crabsPerSecond = 2.0f;
        [Range(0.0f, 1.0f)]
        public float chestProbability = 0.5f;
        [Range(0, 10)]
        public int hitsToSteal = 3;

        [Header("Difficulty")]
        public Difficulty minDifficulty;
        public Difficulty maxDifficulty;

        [Header("Distractors")]
        public int requiredDistractors = 0;

        [Header("Debug")]
        [TextArea]
        public string log;
        public string inbox;
        public TextMeshPro logPanel;

        float timeSinceLastSpawn = 0.0f;

        int from, to;
        Home home;
        GameObject crabObj;
        Crab crab;
        List<Home> neighbors;

        float previousCrabsPerSecond;

        private void Start()
        {
            EngagementView.singleton.onDistractorRequired.AddListener(OnDistractorRequired);
        }

        private void Update()
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (!stealedChest)
            {
                crabsSpeed = Mathf.Lerp(minDifficulty.crabsSpeed, maxDifficulty.crabsSpeed, EngagementView.singleton.viewModel.currentDifficultyLerp);
                crabsPerSecond = Mathf.Lerp(minDifficulty.crabsPerSecond, maxDifficulty.crabsPerSecond, EngagementView.singleton.viewModel.currentDifficultyLerp);
                chestProbability = Mathf.Lerp(minDifficulty.chestProbability, maxDifficulty.chestProbability, EngagementView.singleton.viewModel.currentDifficultyLerp);
                hitsToSteal = Convert.ToInt32(Mathf.Lerp((float)minDifficulty.hitsToSteal, (float)maxDifficulty.hitsToSteal, EngagementView.singleton.viewModel.currentDifficultyLerp));
            }

            if (timeSinceLastSpawn > 1.0f / crabsPerSecond)
            {
                SpawnCrab();

                timeSinceLastSpawn = 0.0f;
            }

            UpdateLog();
            logPanel.text = log;
        }

        public Crab SpawnCrab()
        {
            from = UnityEngine.Random.Range(0, homes.Count - 1);
            home = homes[from];

            crabObj = PoolService.singleton.SpawnFromPool(poolTag, home.door.position, Quaternion.identity);

            crabObj.transform.parent = aliveCrabsContainer;

            crab = crabObj.GetComponent<Crab>();

            neighbors = new List<Home>(homes);
            neighbors.RemoveAt(from);
            to = UnityEngine.Random.Range(0, neighbors.Count - 1);

            crab.homeDestination = neighbors[to];

            crab.hasChest = UnityEngine.Random.value < chestProbability;

            crab.agent.SetDestination(crab.homeDestination.door.position);

            return crab;
        }

        public void HideCrab(Crab crab)
        {
            if (crab.carriedChest.hitsReceived > 0 && crab.carriedChest.hitsReceived < crab.carriedChest.hitsToSteal)
                Manager.singleton.DoSkillFail();

            crab.transform.parent = PoolService.singleton.GetPool(poolTag).container;
            crab.gameObject.SetActive(false);
        }

        public void HideAllChests()
        {
            Crab[] crabs = aliveCrabsContainer.GetComponentsInChildren<Crab>();

            for (int i = 0; i < crabs.Length; i++)
            {
                crabs[i].hasChest = false;
            }
        }

        public Chest StealChest(Crab crab)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif

            crab.hasChest = false;

            // We copy it so the crab can be reused from pool while conserving its chest
            GameObject chestCopy = Instantiate(crab.carriedChest.gameObject, stealedChestContainer);
            chestCopy.SetActive(true);

            GameObject start = new GameObject("ChestAnimationStart");
            BasicHelpers.CopyTransform(crab.transform,start.transform);

            Chest chest = chestCopy.GetComponent<Chest>();
            chest.hitsReceived = chest.hitsToSteal;
            chest.healthBar.SetFillAmount(0.0f);
            chest.hitTrigger.isActive = false;
            chest.topTrigger.isActive = false;

            stealedChest = chest;

            HideAllChests();

            previousCrabsPerSecond = crabsPerSecond;
            crabsPerSecond = 0.0f;

            Manager.singleton.DoSkillWin();

            LerpService.singleton.Play(new LerpService.Animation(
                chestCopy.transform,
                start.transform,
                stealedChestContainer,
                stealingDuration,
                true, true, false, true, false),
                () => {
                    chest.hitTrigger.isActive = false;
                    chest.topTrigger.isActive = true;
                });

            return chest;
        }

        public void ResumeGame()
        {
            // Remove letter
            if (stealedChest.content)
                stealedChest.content.gameObject.SetActive(false); // Disable as it's a pooled object

            // Remove chest
            Destroy(stealedChest.gameObject);

            crabsPerSecond = previousCrabsPerSecond;
        }

        void OnDistractorRequired()
        {
            requiredDistractors++;
        }

        void UpdateLog()
        {
            log = "Game Service\n"
                + "Stealed chest? " + (stealedChest != null) + "\n"
                + "Total living crabs: " + aliveCrabsContainer.childCount + "\n"
                + "Max crabs allowed: " + PoolService.singleton.GetPool(poolTag).size + "\n"
                + "Speed of crabs: " + crabsSpeed + "\n"
                + "Crabs per second: " + crabsPerSecond + "\n"
                + "Probability of carrying chest: " + (chestProbability * 100.0f).ToString("F1") + "%\n"
                + "Inbox: " + inbox;
        }
    }
}