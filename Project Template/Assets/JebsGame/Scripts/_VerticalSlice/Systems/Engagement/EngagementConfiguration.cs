using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Engagement
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Engagement Configuration Asset", order = 1)]
    public class EngagementConfiguration : ScriptableObject
    {
        [Range(0.005f, 0.5f)]
        public float skillWinEffect = 0.01f;
        [Range(-0.005f, -0.5f)]
        public float skillFailEffect = -0.01f;
        [Range(0.005f, 0.5f)]
        public float streakDifficultyRate = 0.1f;
        [Range(0.1f, 5.0f)]
        public float maxLocomotiveSpeed = 3.0f;
        [Range(1.0f,30.0f)]
        public float minTimeBetweenDistractors = 5.0f;
        [Range(1.0f, 30.0f)]
        public float rangeForTimeBetweenDistractors = 60.0f;
    }
}
