using JebsReadingGame.Events;
using JebsReadingGame.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Notifiers
{
    public enum PlayerInteractors
    {
        Head,
        LeftHand,
        RightHand,
        LeftTool,
        RightTool
    }

    [RequireComponent(typeof(Collider))]
    public class PlayerNotifier : MonoBehaviour
    {
        public bool isActive = true;

        [Header("Reacts with")]
        public bool head;
        public bool rightHand;
        public bool leftHand;
        public bool rightTool;
        public bool leftTool;

        [Header("Events")]
        public RigidbodyEvent onTouchEnter = new RigidbodyEvent();
        public RigidbodyEvent onTouchStay = new RigidbodyEvent();
        public RigidbodyEvent onTouchExit = new RigidbodyEvent();

        Collider collider;

        Rigidbody tempRb;

        private void Start()
        {
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
                return;

            tempRb = GetHandFromCollider(other);

            if (tempRb)
                onTouchEnter.Invoke(tempRb);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isActive)
                return;

            tempRb = GetHandFromCollider(other);

            if (tempRb)
                onTouchStay.Invoke(tempRb);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isActive)
                return;

            tempRb = GetHandFromCollider(other);

            if (tempRb)
                onTouchExit.Invoke(tempRb);
        }

        Rigidbody GetHandFromCollider(Collider c)
        {
            if (c.attachedRigidbody == PlayerService.singleton.head && head)
                return PlayerService.singleton.head;
            else if (c.attachedRigidbody == PlayerService.singleton.rightHand && rightHand)
                return PlayerService.singleton.rightHand;
            else if (c.attachedRigidbody == PlayerService.singleton.leftHand && leftHand)
                return PlayerService.singleton.leftHand;
            else if (PlayerService.singleton.rightTool && c.attachedRigidbody == PlayerService.singleton.rightTool && rightTool)
                return PlayerService.singleton.rightTool;
            else if (PlayerService.singleton.leftTool && c.attachedRigidbody == PlayerService.singleton.leftTool && leftTool)
                return PlayerService.singleton.leftTool;
            else
                return null;
        }

    }
}
