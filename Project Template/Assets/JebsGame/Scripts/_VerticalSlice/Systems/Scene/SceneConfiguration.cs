using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Scene
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Scene Configuration Asset", order = 1)]
    public class SceneConfiguration : ScriptableObject
    {
        public int configurationValue = 0;
    }
}