using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    class Lever : MonoBehaviour
    {
        public HingeJoint LeverJoint;

        public Transform LeverHandle;

        private float _value, _minValue, _maxValue;        

        [Range(0, 1)]
        public float TargetValue;

        private float targetValue;

        public float TargetValueChanger
        {
            set
            {
                if(targetValue != value)
                {
                    targetValue = value;
                    Value = targetValue;
                }
            }
        }

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Mathf.Clamp01(value);
                LeverHandle.GetComponent<Rigidbody>().isKinematic = true;

                var newValue = (_maxValue - _minValue) * _value;

                LeverHandle.localRotation = Quaternion.Euler(0, 0, newValue);

                LeverHandle.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
     
        public delegate void LeverValueChangedEventHandler(float newValue);

        public LeverEvent onLeverValueChanged;

        void Awake()
        {
        }

        void Start()
        {
            _minValue = LeverJoint.limits.min;
            _maxValue = LeverJoint.limits.max;
        }

        void FixedUpdate()
        {
            TargetValueChanger = TargetValue;
            if (LeverHandle.hasChanged)
            {
                var newValue = GetValue();
             
                if (_value != newValue)
                {  
                    _value = newValue;
                    onLeverValueChanged.Invoke(Value);
                }
              
                LeverHandle.hasChanged = false;
            }
        }

        private float GetValue()
        {
            return Mathf.InverseLerp(_minValue, _maxValue, LeverHandle.localRotation.eulerAngles.z);
           
        }
    }

}
