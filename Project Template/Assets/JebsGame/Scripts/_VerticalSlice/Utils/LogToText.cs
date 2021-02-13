using TMPro;
using UnityEngine;

namespace HPTK.Utils
{
    public class LogToText : MonoBehaviour
    {
        static string myLog = "";
        static string latestTrace = "";

        public TextMeshPro logTmpro;
        public TextMeshPro traceTmpro;

        private string output;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        private void Update()
        {
            logTmpro.text = myLog;
            traceTmpro.text = latestTrace;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;

            latestTrace = stackTrace;

            myLog = myLog + "\n\n" + output;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
            }
        }
    }
}
