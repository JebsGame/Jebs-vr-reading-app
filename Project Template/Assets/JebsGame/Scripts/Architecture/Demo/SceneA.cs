using JebsReadingGame.Systems.Currency;
using JebsReadingGame.Systems.Gamemode;
using JebsReadingGame.Notifiers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using JebsReadingGame.Systems.Scene;

public class SceneA : MonoBehaviour
{
    public TriggerNotifier triggerNotifier;
    public Collider movableCollider;
    public MeshRenderer movableMeshRenderer;
    public ParticleSystem particles;
    public AK.Wwise.Event positiveFeedback;
    public CoinsPanel coinsPanel;

    public int coinsToChangeScene = 50;

    public string nextScene = "SceneB";

    int hits = 0;
    
    void Start()
    {
        //triggerNotifier.onEnterCollider.AddListener(OnHit);
        CurrencyView.singleton.onCoinsEarned.AddListener(OnCoinsEarned);

        coinsPanel.UpdateCoinsCounter();
    }

    void OnHit(Collider collider)
    {
        if (collider == movableCollider)
        {
            hits++;

            /*
            if (hits == GamemodeView.singleton.viewModel.streakLength)
            {
                GamemodeView.singleton.onPositiveStreakCompleted.Invoke();

                hits = 0;

                // Check scene change
                if (CurrencyView.singleton.viewModel.totalCoins >= coinsToChangeScene)
                    GamemodeView.singleton.onSceneChangeRequest.Invoke(nextScene);
            }
            */
        }
    }

    void OnCoinsEarned(int coins)
    {
        coinsPanel.UpdateCoinsCounter();

        // Sound feedback
        positiveFeedback.Post(this.gameObject);

        // Visual feedback
        PlayParticleBurst(coins);

        movableMeshRenderer.material.SetColor("_Color", new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        ));
    }

    void PlayParticleBurst(int burstSize)
    {
        if (particles)
        {
            ParticleSystem.Burst burst = particles.emission.GetBurst(0);
            burst.count = burstSize;
            particles.emission.SetBurst(0, burst);
            particles.Play();
        }
    }
}

