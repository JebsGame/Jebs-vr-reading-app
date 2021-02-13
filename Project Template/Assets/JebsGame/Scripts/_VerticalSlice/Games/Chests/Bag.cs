using JebsReadingGame.Letters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace JebsReadingGame.Games.Chests
{
    [RequireComponent(typeof(Collider))]
    public class Bag : MonoBehaviour
    {
        Collider collider;

        private void Start()
        {
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.attachedRigidbody)
                return;

            XRGrabInteractable grabbable = other.attachedRigidbody.GetComponent<XRGrabInteractable>();
            if (!grabbable || grabbable.isSelected)
                return;

            Letter letter = grabbable.GetComponent<Letter>();
            if (!letter)
                return;

            Manager.singleton.Evaluate(letter, true);

            letter.gameObject.SetActive(false);
        }
    }
}
