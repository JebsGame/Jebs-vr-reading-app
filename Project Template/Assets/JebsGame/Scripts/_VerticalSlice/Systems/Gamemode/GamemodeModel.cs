using JebsReadingGame.Skeletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JebsReadingGame.Globals;

namespace JebsReadingGame.Systems.Gamemode
{
    public class GamemodeModel : GlobalModel
    {
        [HideInInspector]
        public GamemodeView view; // Optional

        public GamemodeConfiguration asset;

        public GamemodePersistent persistent = new GamemodePersistent();
 
        public Activity activity; // Scene-specific

        [Header("Updated by Controller")]
        public int gameplayLetterWins;
        public int gameplayLetterFails;
        public int currentLetterGroupCombo;
        public int gameplayLetterGroupWins;
        public int gameplayLetterGroupFails;
        // public Item nextEquipableItem;        -> Store system is not implemented yet
    }

    // Persistent model: Persistent between scenes
    public class GamemodePersistent
    {
        string persistentValueKey = "key";

        public int persistentValue
        {
            get { return PlayerPrefs.GetInt(persistentValueKey); }
            set { PlayerPrefs.SetInt(persistentValueKey, value); }
        }
    }
}
