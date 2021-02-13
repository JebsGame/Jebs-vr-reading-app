using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AkTimelineEventPlayable;

namespace JebsReadingGame.Games.Chests
{
    public class Debug : MonoBehaviour
    {
        public GameObject[] debugPanels;

        [Header("Button")]
        public MeshRenderer buttonMR;
        public Material onMaterial;
        public Material offMaterial;

        bool _debugMode = false;
        public bool debugMode
        {
            get { return _debugMode; }
            set
            {
                _debugMode = value;

                for (int i = 0; i < debugPanels.Length; i++)
                {
                    debugPanels[i].SetActive(_debugMode);
                }

                if (_debugMode) buttonMR.material = onMaterial;
                else buttonMR.material = offMaterial;
            }
        }

        public void ToggleDebugMode() { debugMode = !debugMode; }
    }
}




