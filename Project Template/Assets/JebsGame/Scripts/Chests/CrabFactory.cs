﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class CrabFactory : MonoBehaviour
{
    public static CrabFactory factory;

    public int numOfCrabs = 20;
    public float crabsSpeed = 2.0f;
    public float speedIncrease = 0.25f;

    public List<CrabBrain> crabs = new List<CrabBrain>();
    public List<CrabHome> homes = new List<CrabHome>();

    public GameObject crabPrefab;

    [Range(0.0f, 1.0f)]
    public float chestProbability;

    public Transform chestDestination;
    public Spawnable currentContent;

    bool crabsAreRunning = true;

    void Awake()
    {
        if (!factory) factory = this;
    }

    void Update()
    {
        if (crabsAreRunning)
            EvaluatePopulation();
    }

    void EvaluatePopulation()
    {
        for (int i = 0; i < crabs.Count; i++)
        {
            if (!crabs[i])
            {
                crabs.RemoveAt(i);
                i--;
            }
        }

        if (crabs.Count < numOfCrabs)
        {
            int doorNumber = Random.Range(0, homes.Count);

            CrabHome spawnPoint = homes[doorNumber];
            GameObject newBornCrab = Instantiate(crabPrefab,spawnPoint.transform.position,Quaternion.identity);
            newBornCrab.transform.parent = transform;
            CrabBrain smolBrain = newBornCrab.GetComponent<CrabBrain>();
            crabs.Add(smolBrain);

            List<CrabHome> availableNewDestinations = new List<CrabHome>(homes);

            availableNewDestinations.RemoveAt(doorNumber);

            doorNumber = Random.Range(0, availableNewDestinations.Count);

            smolBrain.homeDestination = homes[doorNumber];

            if (Random.value < chestProbability)
                smolBrain.CarryChest();
        }
    }

    public void ToggleCrabs(bool b)
    {
        crabsAreRunning = b;
    }

    public void Restart()
    {
        currentContent.Destroy();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        ToggleCrabs(true);
    }
}
