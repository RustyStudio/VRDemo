using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    /// <summary>
    /// Class of worldUI buttons
    /// </summary>
    public class WorldToggle : WorldUI
    {
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                if (_isOn != value) {
                    _isOn = value;

                    ButtonJoint.SetState(IsOn);
                    if (_isOn)
                    {
                        OnToggleOn.Invoke();  
                    }
                    else
                    {
                        OnToggleOff.Invoke();
                    }
                }
            }
        }

        private bool _isOn = false;

        private bool _isPressed = false;

        public AudioSource Clicker;

        public AudioClip ToggleOnPress;

        public AudioClip ToggleOnRelease;

        public AudioClip ToggleOffPress;

        public AudioClip ToggleOffRelease;

        public WorldToggleJoint ButtonJoint;

        public float PressedOffset;

        public UnityEvent OnToggleOn;

        public UnityEvent OnToggleOff;

        void FixedUpdate()
        {
  
            if(ButtonJoint.transform.localPosition.z <= PressedOffset && !_isPressed)
            {
                if (!IsEnabled)
                    return;
                _isPressed = true;
                IsOn = !IsOn;
                Clicker.PlayOneShot(IsOn ? ToggleOnPress : ToggleOffPress);       
            }
            if (ButtonJoint.transform.localPosition.z >= PressedOffset && _isPressed)
            {
                _isPressed = false;
                Clicker.PlayOneShot(IsOn ? ToggleOnRelease : ToggleOffRelease);
            }
        }

        public override void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
