using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Games.Chests
{
    [RequireComponent(typeof(Collider))]
    public class Home : MonoBehaviour
    {
        public Transform door;

        private void Start()
        {
            if (!door) door = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            Crab crab = other.GetComponent<Crab>();
            if (crab && crab.homeDestination == this)
            {
                Service.singleton.HideCrab(crab);
            }
        }
    }
}
