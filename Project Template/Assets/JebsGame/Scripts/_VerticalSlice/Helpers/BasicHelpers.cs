using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JebsReadingGame.Helpers
{
    public static class BasicHelpers
    {
        public static void CopyTransform(Transform from, Transform to)
        {
            to.position = from.position;
            to.rotation = from.rotation;
            to.localScale = from.localScale;
        }

        public static Color RandomSaturatedColor()
        {
            float[] rgb = new float[3];
            rgb[0] = UnityEngine.Random.value;  // red
            rgb[1] = UnityEngine.Random.value;  // green
            rgb[2] = UnityEngine.Random.value;  // blue

            int max, min;

            if (rgb[0] > rgb[1])
            {
                max = (rgb[0] > rgb[2]) ? 0 : 2;
                min = (rgb[1] < rgb[2]) ? 1 : 2;
            }
            else
            {
                max = (rgb[1] > rgb[2]) ? 1 : 2;
                int notmax = 1 + max % 2;
                min = (rgb[0] < rgb[notmax]) ? 0 : notmax;
            }
            rgb[max] = 1.0f;
            rgb[min] = 0.0f;

            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public static string Shuffle(this string str)
        {
            return new string(str.ToCharArray().OrderBy(s => (Random.Range(0,2) % 2) == 0).ToArray());
        }

        public static string SecondsToClock(float totalSeconds)
        {
            float t = totalSeconds;

            int sec = (int)(t % 60);
            t /= 60;
            int minutes = (int)(t % 60);
            t /= 60;
            int hours = (int)(t % 24);

            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + sec.ToString("00");
        }
    }
}
