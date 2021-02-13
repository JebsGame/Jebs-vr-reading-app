using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Currency
{
    // Scene-specific model
    public class CurrencyModel : MonoBehaviour
    {
        [HideInInspector]
        public CurrencyView view;

        public CurrencyConfiguration asset;

        public CurrencyPersistent persistent = new CurrencyPersistent();
    }

    // Persistent model: Persistent between scenes and launches
    public class CurrencyPersistent
    {
        string totalCoinsKey = "totalCoins";

        public int totalCoins;

        internal void Load()
        {
            totalCoins = PlayerPrefs.GetInt(totalCoinsKey);
        }
        internal void Save()
        {
            PlayerPrefs.SetInt(totalCoinsKey, totalCoins);
        }
    }

    

}
