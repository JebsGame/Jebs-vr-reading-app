using JebsReadingGame.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Globals
{
    public static class Environment
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static string FromLetterGroupToString(LetterGroup letterGroup)
        {
            switch(letterGroup)
            {
                case LetterGroup.AtoG:
                    return "abcdefg";
                case LetterGroup.HtoM:
                    return "hijklm";
                case LetterGroup.NtoT:
                    return "nopqrst";
                case LetterGroup.UtoZ:
                    return "uvwxyz";
                case LetterGroup.All:
                    return alphabet;
                case LetterGroup.None:
                default:
                    return "";
            }
        }

        public static string GetRandomizedLetterGroup(LetterGroup letterGroup)
        {
            return FromLetterGroupToString(letterGroup).Shuffle();
        }

        public static LetterGroup FromCharToLetterGroup(char c)
        {
            switch (c)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                    return LetterGroup.AtoG;
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                    return LetterGroup.HtoM;
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                    return LetterGroup.NtoT;
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    return LetterGroup.UtoZ;
                default:
                    return LetterGroup.None;
            }
        }
    } 

    public enum Activity
    {
        None,
        LetterRecognition,
        LetterSequencing,
        LetterMissing,
        LetterPairing,
        All
    }

    public enum LetterGroup
    {
        None,
        AtoG,
        HtoM,
        NtoT,
        UtoZ,
        All
    }

    public enum SceneState
    {
        None,
        Loading,
        Tutorial,
        Gameplay,
        Minigame,
        Paused
    }

    public enum GamemodeUnlockingStrategy
    {
        UnlockAllLevelsToUnlockGamemode,
        UnlockFirstLevelToUnlockGamemode
    }

    public enum LevelUnlockingStrategy
    {
        UnlockNextFromCurrent,
        UnlockNextFromLearning
    }
}
