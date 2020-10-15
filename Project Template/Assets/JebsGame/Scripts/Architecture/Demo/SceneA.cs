﻿using JebsReadingGame.CurrencySystem;
using JebsReadingGame.GamemodeManager;
using JebsReadingGame.Notifiers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneA : MonoBehaviour
{
    public TriggerNotifier triggerNotifier;
    public Collider movableCollider;
    public MeshRenderer movableMeshRenderer;
    public ParticleSystem particles;
    public AK.Wwise.Event positiveFeedback;
    public CoinsPanel coinsPanel;

    public int coinsToChangeScene = 50;
    public string pathForNextScene;

    int hits = 0;
    
    void Start()
    {
        triggerNotifier.onEnterCollider.AddListener(OnHit);
        CurrencySystemView.singleton.onCoinsEarned.AddListener(OnCoinsEarned);

        coinsPanel.UpdateCoinsCounter();
    }

    void OnHit(Collider collider)
    {
        if (collider == movableCollider)
        {
            hits++;

            if (hits == GamemodeManagerView.singleton.viewModel.streakLength)
            {
                GamemodeManagerView.singleton.onPositiveStreak.Invoke();

                hits = 0;

                // Check scene change
                if (CurrencySystemView.singleton.viewModel.totalCoins >= coinsToChangeScene)
                    SceneManager.LoadScene(pathForNextScene, LoadSceneMode.Single);
            }
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

