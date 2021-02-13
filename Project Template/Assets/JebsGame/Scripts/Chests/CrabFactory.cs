using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using JebsReadingGame.Games.Chests;

public class CrabFactory : MonoBehaviour
{
    public static CrabFactory factory;

    public int numOfCrabs = 20;
    public float crabsSpeed = 2.0f;
    public float speedIncrease = 0.25f;

    public List<Crab> crabs = new List<Crab>();
    public List<Home> homes = new List<Home>();

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

            Home spawnPoint = homes[doorNumber];
            GameObject newBornCrab = Instantiate(crabPrefab,spawnPoint.transform.position,Quaternion.identity);
            newBornCrab.transform.parent = transform;
            Crab smolBrain = newBornCrab.GetComponent<Crab>();
            crabs.Add(smolBrain);

            List<Home> availableNewDestinations = new List<Home>(homes);

            availableNewDestinations.RemoveAt(doorNumber);

            doorNumber = Random.Range(0, availableNewDestinations.Count);

            smolBrain.homeDestination = homes[doorNumber];

            if (Random.value < chestProbability)
                smolBrain.hasChest = true;
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
