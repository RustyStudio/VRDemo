using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{ 
    /// <summary>
    /// Class of worldUI buttons
    /// </summary>
    class WorldButton : WorldUI
    {
        private bool _isOn = false;

        public AudioSource Clicker;

        public Transform ButtonTransform;

        public float PressedOffset;

        public UnityEvent OnButtonPress;

        void FixedUpdate()
        {
            if (!IsEnabled)
                return;
            if(ButtonTransform.localPosition.z <= PressedOffset)
            {
                if(!_isOn)
                {
                    _isOn = true;
                    OnButtonPress.Invoke();
                    Clicker.Play();
                }
            }
            if (ButtonTransform.localPosition.z >= PressedOffset)
            {
                if (_isOn)
                {
                    _isOn = false;
                }
            }
        }

        public override void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
