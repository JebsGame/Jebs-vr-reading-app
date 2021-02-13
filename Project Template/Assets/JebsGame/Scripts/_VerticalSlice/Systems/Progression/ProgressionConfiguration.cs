using JebsReadingGame.Globals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Progression
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Progression Configuration Asset", order = 1)]
    public class ProgressionConfiguration : ScriptableObject
    {
        public GamemodeUnlockingStrategy howToUnlockGamemodes;
        public LevelUnlockingStrategy howToUnlockLevels;
    }
}
