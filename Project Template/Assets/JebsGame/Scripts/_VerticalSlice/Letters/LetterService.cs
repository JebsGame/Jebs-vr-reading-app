using JebsReadingGame.Helpers;
using JebsReadingGame.Player;
using JebsReadingGame.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JebsReadingGame.Letters
{
    public class LetterService : MonoBehaviour
    {
        // Singleton
        static LetterService _singleton;
        public static LetterService singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<LetterService>();

                return _singleton;
            }
        }

        public string poolTag = "letters";

        [Header("Fonts")]
        public Font[] fonts;

        [Header("Default values")]
        public Color defaultColor = Color.white;
        public int defaultFontSize = 7;

        public Letter SpawnLetter(Vector3 worldPos)
        {
            Letter newLetter = PoolService.singleton.SpawnFromPool(poolTag, worldPos, Quaternion.identity).GetComponent<Letter>();

            newLetter.lookAt = PlayerService.singleton.head.transform;
            newLetter.letter = 'a';
            newLetter.color = defaultColor;
            newLetter.font = fonts[0];
            newLetter.fontSize = defaultFontSize;

            return newLetter;
        }

        public void RandomizeStyle(Letter letter)
        {
            letter.color = BasicHelpers.RandomSaturatedColor();
            letter.font = fonts[UnityEngine.Random.Range(0, fonts.Length - 1)];
        }

    }
}
