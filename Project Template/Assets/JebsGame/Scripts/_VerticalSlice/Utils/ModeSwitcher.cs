using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Utils
{
    [Serializable]
    public class Mode
    {
        public GameObject root;

        public UnityEvent onStart;
        public UnityEvent onEnd;
    }

    public class ModeSwitcher : MonoBehaviour
    {
        public Mode[] modes;

        public int modeOnStart = 0;

        [Header("Updated by script")]
        public int currentMode = -1;

        private void Start()
        {
            if (modes.Length > 0)
                SetMode(modeOnStart);
        }

        public void SetMode(int newMode)
        {
            if (newMode < 0 || newMode > modes.Length - 1)
                return;

            if (currentMode >= 0)
            {
                modes[currentMode].onEnd.Invoke();
                modes[currentMode].root.SetActive(false);
            }

            currentMode = newMode;

            modes[currentMode].root.SetActive(true);
            modes[currentMode].onStart.Invoke();
        }
    }
}
