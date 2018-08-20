using System;
using UnityEngine;

namespace InteractionDemo.Core
{
    /// <summary>
    /// Class for moving around
    /// </summary>
    public class Locomotion : MonoBehaviour
    {
        public SteamVR_TrackedObject Camera;

        public SteamVR_ControllerManager Rig;

        public TrackedController LeftController;

        public TrackedController RightController;

        public float SpeedMultiplier;

        public float MaxVelocity = 10;

        private bool[] _locomotionReady = new bool[] { false, false };

        private Rigidbody _rigidbody;

        void Start()
        {
            LeftController.OnGripDown += LeftController_OnGripDown;
            LeftController.OnGripUp += LeftController_OnGripUp;
            RightController.OnGripDown += RightController_OnGripDown;
            RightController.OnGripUp += RightController_OnGripUp;
            _rigidbody = Rig.GetComponent<Rigidbody>();
        }

        private void RightController_OnGripUp(TrackedController sender, bool Value)
        {
            _locomotionReady[1] = Value;
        }

        private void RightController_OnGripDown(TrackedController sender, bool Value)
        {
            _locomotionReady[1] = Value;
            _rightControllerPosition = RightController.transform.localPosition;
        }

        private void LeftController_OnGripUp(TrackedController sender, bool Value)
        {
            _locomotionReady[0] = Value;
        }

        private void LeftController_OnGripDown(TrackedController sender, bool Value)
        {
            _locomotionReady[0] = Value;
            _leftControllerPosition = LeftController.transform.localPosition;
        }

        private Vector3 _leftControllerPosition, _rightControllerPosition;

        void Update()
        {          

            if (_locomotionReady[0] && _locomotionReady[1])
            {
                //ENGAGE LOCOMOTION
                var magnitudeLeft = (_leftControllerPosition - LeftController.transform.localPosition).sqrMagnitude;

                var magnitudeRight = (_rightControllerPosition - RightController.transform.localPosition).sqrMagnitude;

                var velocity = magnitudeLeft + magnitudeRight;

                var direction = new Vector3(Camera.transform.forward.x, 0, Camera.transform.forward.z);

                if (_rigidbody.velocity.sqrMagnitude < MaxVelocity)
                {
                    _rigidbody.velocity += (direction * Mathf.Min(MaxVelocity, velocity * SpeedMultiplier));
                }

            }
            _leftControllerPosition = LeftController.transform.localPosition;
            _rightControllerPosition = RightController.transform.localPosition;
        }
    }
}
