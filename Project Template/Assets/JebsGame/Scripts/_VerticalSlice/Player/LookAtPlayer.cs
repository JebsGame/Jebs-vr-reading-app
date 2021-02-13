using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace JebsReadingGame.Player
{
    [RequireComponent(typeof(LookAtConstraint))]
    public class LookAtPlayer : MonoBehaviour
    {
        LookAtConstraint lookAtConstraint;

        private void Start()
        {
            lookAtConstraint = GetComponent<LookAtConstraint>();

            ConstraintSource source = new ConstraintSource();
            source.weight = 1.0f;
            source.sourceTransform = PlayerService.singleton.head.transform;

            List<ConstraintSource> sources = new List<ConstraintSource>();
            sources.Add(source);

            lookAtConstraint.SetSources(sources);
        }
    }
}
