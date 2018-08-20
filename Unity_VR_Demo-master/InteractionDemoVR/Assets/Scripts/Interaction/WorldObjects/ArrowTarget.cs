using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    public class ArrowTarget : ArrowStickable
    {
        public float MaximumDistance = 0.5f;

        public float MaxScore = 100;

        public Transform Bullseye;

        public float GetScore(Vector3 positionFrom)
        {
            var scoreMagnitude = (Bullseye.position - positionFrom).magnitude;
            if (scoreMagnitude <= MaximumDistance)
            {
                var scoreRemover = Mathf.InverseLerp(0, MaximumDistance, scoreMagnitude) * MaxScore;
                return MaxScore - scoreRemover;
            }
            return 0;
        }
    }
}
