using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Currency
{
    // Configuration model: Can be shared between models. Cannot be modified in build
    [CreateAssetMenu(menuName = "JesbReadingGame/Currency Configuration Asset", order = 1)]
    public class CurrencyConfiguration : ScriptableObject
    {
        public int letterWinPrice = 5;
        public int letterGroupWinPrice = 5;
        public int positiveStreakPrice = 100;
        public int tipPrice = 3;
    }
}
