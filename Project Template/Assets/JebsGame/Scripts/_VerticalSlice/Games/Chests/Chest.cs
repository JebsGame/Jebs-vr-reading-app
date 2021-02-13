using JebsReadingGame.Letters;
using JebsReadingGame.Notifiers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Games.Chests
{
    public class Chest : MonoBehaviour
    {
        public Crab crab;

        public PlayerNotifier hitTrigger;
        public PlayerNotifier topTrigger;
        public Collider interiorTrigger;

        public Letter content;
        public Transform contentContainer;
        public TextMeshPro[] letterSigns;

        [Header("Stealing")]
        public int hitsToSteal = 3;
        public int hitsReceived = 0;
        public float chestAnimationSpeed = 1.0f;
        public ImageFiller healthBar;

        [Header("Open/Close")]
        public Transform pivot;
        public float semiClosedAngle = 340.0f;     
        
        [Header("Aesthetics")]
        public MeshRenderer topMeshRenderer;
        public MeshRenderer bottomMeshRenderer;
        public Material[] easyMats;
        public Material[] hardMats;

        [Header("Distractor")]
        public Material[] distractorMats;
        public GameObject distractivePart;

        [Header("Events")]
        public UnityEvent onHit;
        public UnityEvent onOpen;
        public UnityEvent onClose;

        bool _isOpen;
        public bool isOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;

                if (_isOpen)
                {
                    pivot.Rotate(new Vector3(150.0f, 0.0f, 0.0f), Space.Self);

                    if (!content)
                    {
                        FillChest();
                        Manager.singleton.RandomizeLetter(content);

                        for (int i = 0; i < letterSigns.Length; i++)
                        {
                            letterSigns[i].text = content.letter.ToString();
                        }
                    }

                    onOpen.Invoke();
                }
                else
                {
                    pivot.Rotate(new Vector3(-150.0f, 0.0f, 0.0f), Space.Self);

                    onClose.Invoke();

                    if (content && interiorTrigger.bounds.Contains(content.transform.position))
                    {
                        Manager.singleton.Evaluate(content, false);
                    }
                }
            }
        }

        public void SetInteractable(bool b)
        {
            hitTrigger.enabled = !b;

            interiorTrigger.enabled = b;
            topTrigger.enabled = b;
        }

        public void ChestHit()
        {
            onHit.Invoke();

            hitsReceived++;

            healthBar.SetInstantFillAmount(1.0f - ((float)hitsReceived / (float)hitsToSteal));

            if (hitsReceived >= hitsToSteal)
                Service.singleton.StealChest(crab);
        }

        public void FillChest()
        {
            content = LetterService.singleton.SpawnLetter(contentContainer.position);
        }

        public void TogglePivot()
        {
            isOpen = !isOpen;
        }

    }
}
