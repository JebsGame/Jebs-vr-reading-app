using JebsReadingGame.Systems.Currency;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JebsReadingGame.Utils
{
    public class CoinsCounter : MonoBehaviour
    {
        public TextMeshPro tmpro;

        int _coins = 0;
        public int coins
        {
            get { return _coins; }
            set
            {
                _coins = value;

                tmpro.text = _coins.ToString();
            }
        }

        private void Start()
        {
            coins = CurrencyView.singleton.viewModel.totalCoins;

            CurrencyView.singleton.onCoinsEarned.AddListener(OnCoinsEarned);
        }

        void OnCoinsEarned(int earnedCoins)
        {
            coins += earnedCoins;
        }
    }
}
