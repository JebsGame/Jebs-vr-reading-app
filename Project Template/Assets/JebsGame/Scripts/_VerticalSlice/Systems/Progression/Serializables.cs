using JebsReadingGame.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Systems.Progression
{
    [Serializable]
    public class Level
    {
        public LetterGroup letterGroup;
        public bool unlocked;

        public Level(LetterGroup letterGroup, bool unlocked)
        {
            this.letterGroup = letterGroup;
            this.unlocked = unlocked;
        }

        public static Level[] DeepCopy(Level[] levels)
        {
            List<Level> cpLevels = new List<Level>();

            for (int i = 0; i < levels.Length; i++)
            {
                cpLevels.Add(new Level(levels[i].letterGroup,levels[i].unlocked));
            }

            return cpLevels.ToArray();
        }
    }

    [Serializable]
    public class Gamemode
    {
        public Activity activity;
        public Level[] levels;
        public bool unlocked;

        public bool tutorialCompleted = false;
        public bool broken = false;

        public Gamemode(Activity activity, Level[] levels, bool unlocked)
        {
            this.activity = activity;
            this.levels = levels;
            this.unlocked = unlocked;
        }
    }

    [Serializable]
    public class GamemodeGroup
    {
        public Gamemode[] gamemodes;
        public bool unlocked;

        public GamemodeGroup(Gamemode[] gamemodes, bool unlocked)
        {
            this.gamemodes = gamemodes;
            this.unlocked = unlocked;
        }
    }

    [Serializable]
    public class ProgressionState
    {
        public GamemodeGroup[] gamemodeGroups;

        public ProgressionState(bool firstLevelUnlocked)
        {
            // Lists init
            List<Level> levels = new List<Level>();
            List<Gamemode> gamemodes = new List<Gamemode>();
            List<GamemodeGroup> gamemodeGroups = new List<GamemodeGroup>();

            // Levels
            levels.Add(new Level(LetterGroup.AtoG, false)); // (!)
            levels.Add(new Level(LetterGroup.HtoM, false));
            levels.Add(new Level(LetterGroup.NtoT, false));
            levels.Add(new Level(LetterGroup.UtoZ, false));

            // Gamemodes
            gamemodes.Add(new Gamemode(Activity.LetterRecognition, Level.DeepCopy(levels.ToArray()), false)); // (!)
            gamemodes.Add(new Gamemode(Activity.LetterSequencing, Level.DeepCopy(levels.ToArray()), false));
            gamemodes.Add(new Gamemode(Activity.LetterMissing, Level.DeepCopy(levels.ToArray()), false));
            gamemodes.Add(new Gamemode(Activity.LetterPairing, Level.DeepCopy(levels.ToArray()), false));

            // Gamemode groups
            gamemodeGroups.Add(new GamemodeGroup(gamemodes.ToArray(), false)); // (!)

            // Unlock if needed
            if (firstLevelUnlocked)
            {
                gamemodeGroups[0].unlocked = true;
                gamemodeGroups[0].gamemodes[0].unlocked = true;
                gamemodeGroups[0].gamemodes[0].levels[0].unlocked = true;
            }

            // 
            this.gamemodeGroups = gamemodeGroups.ToArray();
        }
    }
}
