using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Gamemode
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Gamemode Configuration Asset", order = 1)]
    public class GamemodeConfiguration : ScriptableObject
    {
        public int letterGroupStreakLength = 5; // How many letter group win/fails complete a positive/negative streak for the current letter group
    }
}
