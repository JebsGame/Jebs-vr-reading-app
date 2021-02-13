using JebsReadingGame.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Log
{
    [Serializable]
    public class History
    {
        public string createdAt;
        public Log[] logs;

        public History(List<Log> logs)
        {
            this.createdAt = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            this.logs = logs.ToArray();
        }
    }

    [Serializable]
    public class Log
    {
        public string dateTime;
        public string gameplayTime;
        public string content;

        public Log(string content, float secondsElapsed, DateTime dateTime)
        {
            this.dateTime = dateTime.ToString("yyyy/MM/dd hh:mm:ss");
            this.gameplayTime = BasicHelpers.SecondsToClock(secondsElapsed);
            this.content = content;
        }
    }
}
