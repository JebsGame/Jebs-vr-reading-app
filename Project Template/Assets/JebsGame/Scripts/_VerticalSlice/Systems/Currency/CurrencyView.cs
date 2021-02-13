using JebsReadingGame.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Systems.Currency
{
    public class CurrencyView : MonoBehaviour
    {
        // Singleton
        static CurrencyView _singleton;
        public static CurrencyView singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<CurrencyView>();

                return _singleton;
            }
        }

        // ViewModel
        public sealed class CurrencyViewModel
        {
            CurrencyModel model;

            // Properties
            public int totalCoins { get { return model.persistent.totalCoins; } }

            // Constructor
            public CurrencyViewModel(CurrencyModel model)
            {
                this.model = model;
            }
        }
        public CurrencyViewModel viewModel;

        // Events
        public IntEvent onCoinsEarned;
    }
}
