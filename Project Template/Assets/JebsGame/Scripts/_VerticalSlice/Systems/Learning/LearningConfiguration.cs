using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Learning
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Learning Configuration Asset", order = 1)]
    public class LearningConfiguration : ScriptableObject
    {
        public int updateLearningScoreFrom = 15; // Total wins + total fails

        [Range(0.51f, 0.99f)]
        public float letterGroupMasteredOver = 0.9f;

        [Range(0.01f, 0.49f)]
        public float letterGroupForgottenUnder = 0.9f;
    }
}