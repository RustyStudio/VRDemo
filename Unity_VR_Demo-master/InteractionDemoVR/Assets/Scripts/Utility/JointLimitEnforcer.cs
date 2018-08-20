using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractionDemo.Utility
{
    /// <summary>
    /// Enforces visual object to joint limits, even if physical collider flies of to the moon
    /// </summary>
    class JointLimitEnforcer : MonoBehaviour
    {
        public ConfigurableJoint JointObject;

        private Vector3 _minOffset, _maxOffset;

        private Vector3 restPosition;

        private Vector3 zAngularLimits, xAngularLimits, yAngularLimits;

        void Awake()
        {
            var LinearLimit = JointObject.linearLimit.limit;
            restPosition = JointObject.transform.localPosition;
            _maxOffset.x = restPosition.x + LinearLimit;
            _maxOffset.y = restPosition.y + LinearLimit;
            _maxOffset.z = restPosition.z + LinearLimit;
            _minOffset.x = restPosition.x - LinearLimit;
            _minOffset.y = restPosition.y - LinearLimit;
            _minOffset.z = restPosition.z - LinearLimit;
            if (JointObject.xMotion == ConfigurableJointMotion.Locked)
            {
                _maxOffset.x = _minOffset.x = restPosition.x;
            }
            if (JointObject.yMotion == ConfigurableJointMotion.Locked)
            {
                _maxOffset.y = _minOffset.y = restPosition.y;
            }
            if (JointObject.zMotion == ConfigurableJointMotion.Locked)
            {
                _maxOffset.z = _minOffset.z = restPosition.z;
            }
        }

        void FixedUpdate()
        {
            var newPosition = JointObject.transform.localPosition ;
            newPosition.x = Mathf.Clamp(newPosition.x, _minOffset.x, _maxOffset.x);
            newPosition.y = Mathf.Clamp(newPosition.y, _minOffset.y, _maxOffset.y);
            newPosition.z = Mathf.Clamp(newPosition.z, _minOffset.z, _maxOffset.z);
            transform.localPosition = newPosition;
        }

    }
}
