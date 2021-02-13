using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JebsReadingGame.Utils
{
    public class LerpService: MonoBehaviour
    {
        public sealed class Animation
        {
            public bool playing = false;
            public Transform receiver;
            public Transform start;
            public Transform destination;
            public float duration = 1.0f;
            public bool lerpPos = true;
            public bool lerpRot = true;
            public bool lerpScale = true;
            public bool destroyStart = false;
            public bool destroyDestination = false;

            public Animation(Transform receiver, Transform start, Transform destination, float duration, bool lerpPos, bool lerpRot, bool lerpScale, bool destroyStart, bool destroyDestination)
            {
                this.receiver = receiver;
                this.start = start;
                this.destination = destination;
                this.duration = duration;
                this.lerpPos = lerpPos;
                this.lerpRot = lerpRot;
                this.lerpScale = lerpScale;
                this.destroyStart = destroyStart;
                this.destroyDestination = destroyDestination;
            }
        }

        // Singleton
        static LerpService _singleton;
        public static LerpService singleton
        {
            get
            {
                if (_singleton == null)
                    _singleton = GameObject.FindObjectOfType<LerpService>();

                return _singleton;
            }
        }

        public void Play(Animation animation, UnityAction onCompleted)
        {
            animation.playing = true;
            StartCoroutine(Lerp(animation, onCompleted));
        }

        IEnumerator Lerp(Animation animation, UnityAction onCompleted)
        {
            float time = 0;

            float lerp;

            while (time < animation.duration)
            {
                if (!animation.playing)
                {
                    yield return null;
                    continue;
                }

                lerp = Mathf.SmoothStep(0.0f, 1.0f, time / animation.duration);

                if (animation.lerpPos)
                    animation.receiver.position = Vector3.Lerp(animation.start.position, animation.destination.position, lerp);
                if (animation.lerpRot)
                    animation.receiver.rotation = Quaternion.Lerp(animation.start.rotation, animation.destination.rotation, lerp);
                if (animation.lerpScale)
                    animation.receiver.localScale = Vector3.Lerp(animation.start.localScale, animation.destination.localScale, lerp);

                time += Time.deltaTime;
                yield return null;
            }

            if (animation.lerpPos)
                animation.receiver.position = animation.destination.position;
            if (animation.lerpRot)
                animation.receiver.rotation = animation.destination.rotation;
            if (animation.lerpScale)
                animation.receiver.localScale = animation.destination.localScale;

            if (animation.destroyStart)
                Destroy(animation.start.gameObject);
            if (animation.destroyDestination)
                Destroy(animation.destination.gameObject);

            onCompleted.Invoke();
        }
    }
}
