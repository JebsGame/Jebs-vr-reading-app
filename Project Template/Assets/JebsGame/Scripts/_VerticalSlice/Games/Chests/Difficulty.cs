using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Games.Chests
{
    [CreateAssetMenu(menuName = "JesbReadingGame/Chests/Difficulty Asset", order = 1)]
    public class Difficulty : ScriptableObject
    {
        [Header("Service")]
        [Range(1.0f, 10.0f)]
        public float crabsSpeed = 2.0f;
        [Range(0.0f, 10.0f)]
        public float crabsPerSecond = 2.0f;
        [Range(0.0f, 1.0f)]
        public float chestProbability = 0.5f;
        [Range(1, 10)]
        public int hitsToSteal = 3;

        [Header("Manager")]
        [Range(0, 10)]
        public int maxIncorrectLetters = 3;
    }
}
