using JebsReadingGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace JebsReadingGame.Pools
{
    public class PoolService : MonoBehaviour
    {
        [Serializable]
        public sealed class Pool
        {
            public string tag;
            public GameObject prefab;
            public Transform container;
            public int size;
        }

        // Singleton
        static PoolService _singleton;
        public static PoolService singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<PoolService>();

                return _singleton;
            }
        }

        public List<Pool> pools = new List<Pool>();
        public Dictionary<string, Queue<GameObject>> dictionary = new Dictionary<string, Queue<GameObject>>();

        public void Awake()
        {
            foreach (Pool pool in pools)
            {
                Queue<GameObject> queue = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.transform.parent = pool.container;
                    obj.SetActive(false); // This prevents Awake, Start and Update functions from executing
                    queue.Enqueue(obj);

                    IPooledObject respawnable = obj.GetComponent<IPooledObject>();
                    if (respawnable != null)
                    {
                        respawnable.OnInstantiation();
                    }
                }

                dictionary.Add(pool.tag, queue);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 worldPosition, Quaternion worldRotation)
        {
            if (!dictionary.ContainsKey(tag))
            {
                Debug.LogError("Dictionary does not contain key '" + tag + "'");
                return null;
            }

            GameObject objectToSpawn = dictionary[tag].Dequeue(); // Get oldest element

            objectToSpawn.SetActive(true);

            NavMeshAgent agent = objectToSpawn.GetComponent<NavMeshAgent>();

            if (agent)
            {
                agent.Warp(worldPosition);
            }
            else
            {
                objectToSpawn.transform.position = worldPosition;
            }

            objectToSpawn.transform.rotation = worldRotation;

            IPooledObject respawnable = objectToSpawn.GetComponent<IPooledObject>();
            if (respawnable != null)
            {
                respawnable.OnRespawn();
            }

            dictionary[tag].Enqueue(objectToSpawn); // Store it again as the newest

            return objectToSpawn;
        }

        /*
        public GameObject SpawnFromPool(string tag, Vector3 worldPosition, Quaternion worldRotation, Transform parent)
        {
            GameObject objectToSpawn = SpawnFromPool(tag,worldPosition,worldRotation);
            
            objectToSpawn.transform.parent = parent;

            return objectToSpawn;
        }
        */

        public Pool GetPool(string tag)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i].tag == tag)
                    return pools[i];
            }

            return null;
        }
    }
}
