using InteractionDemo.Core;
using System;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Class of hinged switches
    /// </summary>
    class ConcreteSwitch : ASwitchable
    {
        public float OnPosition = 0;

        public float OffPosition = 0;

        public bool InitialyOn = false;

        public HingeJoint Joint;        

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
                    ConfigureSpring();
                    OnSwitchEvent.Invoke(IsOn);
                }
            }
        }

        private bool _isOn = true;

        public SwitcherEvent OnSwitchEvent;

        void Start()
        {
            IsOn = InitialyOn;
        }

        private void ConfigureSpring()
        {
            Joint.spring = new JointSpring() { spring = 1, targetPosition = _isOn ?  OnPosition :OffPosition};           
        }

        public override void Switch()
        {
            IsOn = !IsOn;
        }
    }
}
