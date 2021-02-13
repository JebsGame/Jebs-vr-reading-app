using JebsReadingGame.Pools;
using JebsReadingGame.Systems.Engagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace JebsReadingGame.Games.Chests
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Crab : MonoBehaviour, IPooledObject
    {
        [HideInInspector]
        public NavMeshAgent agent;

        public Home homeDestination;
        public Chest carriedChest;

        [Header("Aesthetics")]
        public MeshRenderer meshRenderer;
        public Material easyMat;
        public Material hardMat;

        [Header("Distractor")]
        public Material distractorMat;
        public GameObject distractivePart;

        bool _hasChest = true;
        public bool hasChest
        {
            get { return _hasChest; }
            set
            {
                _hasChest = value;
                if (carriedChest) carriedChest.gameObject.SetActive(_hasChest);
            }
        }

        public void OnInstantiation()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void OnRespawn()
        {
            ApplyDifficulty(EngagementView.singleton.viewModel.currentDifficultyLerp, Service.singleton.requiredDistractors > 0);

            if (Service.singleton.requiredDistractors > 0)
                Service.singleton.requiredDistractors--;

            carriedChest.hitTrigger.isActive = true;
            carriedChest.topTrigger.isActive = false;
            carriedChest.hitsReceived = 0;
            carriedChest.healthBar.SetFillAmount(0.0f);
        }

        public void ApplyDifficulty(float difficultyLerp, bool isDistractor)
        {
            carriedChest.hitsToSteal = Service.singleton.hitsToSteal;

            distractivePart.SetActive(isDistractor);
            carriedChest.distractivePart.SetActive(isDistractor);

            if (isDistractor)
            {
                meshRenderer.material = distractorMat;
                carriedChest.topMeshRenderer.materials = carriedChest.distractorMats;
                carriedChest.bottomMeshRenderer.materials = carriedChest.distractorMats;

                agent.speed = Random.Range(Service.singleton.minDifficulty.crabsSpeed, Service.singleton.maxDifficulty.crabsSpeed);
            }
            else
            {
                if (Random.value < difficultyLerp)
                {
                    meshRenderer.material = hardMat;
                    carriedChest.topMeshRenderer.materials = carriedChest.hardMats;
                    carriedChest.bottomMeshRenderer.materials = carriedChest.hardMats;
                }
                else
                {
                    meshRenderer.material = easyMat;
                    carriedChest.topMeshRenderer.materials = carriedChest.easyMats;
                    carriedChest.bottomMeshRenderer.materials = carriedChest.easyMats;
                }

                agent.speed = Service.singleton.crabsSpeed;

                distractivePart.SetActive(false);
            }
        }
    }
}
