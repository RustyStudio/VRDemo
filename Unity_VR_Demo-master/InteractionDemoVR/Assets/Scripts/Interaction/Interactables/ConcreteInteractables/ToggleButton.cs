using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Class of buttons
    /// </summary>
    class ToggleButton : ASwitchable
    {
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    OnToggleOn.Invoke();
                }
            }
        }

        private bool _isOn = false;

        public UnityEvent OnToggleOn;

        public UnityEvent OnToggleOff;        
        
        public override void Switch()
        {
            IsOn = !IsOn;
        }
    }
}
