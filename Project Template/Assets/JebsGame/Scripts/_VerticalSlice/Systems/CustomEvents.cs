using JebsReadingGame.Globals;
using JebsReadingGame.Systems.Progression;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Events
{
    // Basic
    [Serializable]
    public class IntEvent : UnityEvent<int> { }
    [Serializable]
    public class CharEvent : UnityEvent<char> { }
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    // Physics
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    [Serializable]
    public class RigidbodyEvent : UnityEvent<Rigidbody> { }

    // Composed
    [Serializable]
    public class IntIntEvent : UnityEvent<int,int> { }
    [Serializable]
    public class IntCharEvent : UnityEvent<int, char> { }

    // Learning system
    [Serializable]
    public class ActivityEvent : UnityEvent<Activity> { }
    [Serializable]
    public class ActivityLetterGroupEvent : UnityEvent<Activity, LetterGroup> { }
    [Serializable]
    public class ActivityLetterEvent : UnityEvent<Activity, char> { }

    // Progression system
    [Serializable]
    public class GamemodeGroupEvent : UnityEvent<GamemodeGroup> { }
    [Serializable]
    public class GamemodeEvent : UnityEvent<GamemodeGroup,Gamemode> { }
    [Serializable]
    public class LevelEvent : UnityEvent<GamemodeGroup,Gamemode,Level> { }

    // Scene system
    [Serializable]
    public class SceneStateEvent : UnityEvent<SceneState> { }
}