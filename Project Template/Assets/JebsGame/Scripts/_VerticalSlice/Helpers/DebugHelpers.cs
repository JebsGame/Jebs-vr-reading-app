using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Helpers
{
    public static class DebugHelpers
    {
        public static void Log(string newMessage, ref string inbox)
        {
            inbox = "[" + Time.time + "]" + newMessage;
            Debug.Log(newMessage);
        }

        public static void LogEvent(string newMessage, ref string inbox)
        {
            inbox = "[" + Time.time + "] EVENT: " + newMessage;
            Debug.Log(newMessage);
        }

        public static void LogError(string newMessage, ref string inbox)
        {
            inbox = "[" + Time.time + "] ERROR: " + newMessage;
            Debug.LogError(newMessage);
        }
    }
}
