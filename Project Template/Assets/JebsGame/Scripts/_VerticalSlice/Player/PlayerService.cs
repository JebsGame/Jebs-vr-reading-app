using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Player
{
    public class PlayerService : MonoBehaviour
    {
        // Singleton
        static PlayerService _singleton;
        public static PlayerService singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<PlayerService>();

                return _singleton;
            }
        }

        [Header("Body")]
        public Rigidbody head;
        public Rigidbody leftHand;
        public Rigidbody rightHand;

        [Header("Tools")]
        public Rigidbody leftTool;
        public Rigidbody rightTool;

        [Header("Updated by Script")]
        public float headSpeed;
        public float leftHandSpeed;
        public float rightHandSpeed;

        Vector3 previousHeadPos, previousLeftHandPos, previousRightHandPos;

        private void Update()
        {
            // Update locomotive activity
            headSpeed = ((head.position - previousHeadPos) / Time.deltaTime).magnitude;
            leftHandSpeed = ((leftHand.position - previousLeftHandPos) / Time.deltaTime).magnitude;
            rightHandSpeed = ((rightHand.position - previousRightHandPos) / Time.deltaTime).magnitude;

            previousHeadPos = head.position;
            previousLeftHandPos = leftHand.position;
            previousRightHandPos = rightHand.position;
        }
    }
}
