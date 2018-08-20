using InteractionDemo.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionDemo.Core
{
    public class HandStateController : MonoBehaviour
    {
        private const string HAND_STATE_PARAMETER = "HandState";

        public Animator HandAnimator;

        private bool _stateLocked;

        public void SetGrabState(bool isGrabbing, HandStateEnum grabState)
        {
            HandAnimator.SetInteger(HAND_STATE_PARAMETER, (int)grabState);
            _stateLocked = isGrabbing;
        }

        public void SetHandState(HandStateEnum newState)
        {
            if (!_stateLocked)
            {
                HandAnimator.SetInteger(HAND_STATE_PARAMETER, (int)newState);
            }
        }
    }
}
