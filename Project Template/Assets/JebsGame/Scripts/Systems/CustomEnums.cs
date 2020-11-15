﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Enums
{
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
}
