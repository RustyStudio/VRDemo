using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Grabbable object that doesn't allow going through colliders
    /// </summary>
    class RubberBandGrabbableObject : GrabbableObject
    {
        private bool _armSpring = false;

        private bool _detectedCollision = false;

        public override void BeforeRelease(Grabber grabber)
        {
            TurnSpringOff();
        }

        void FixedUpdate()
        {
            if (_currentGrabber == null)
                return;
            if (_detectedCollision)
                _armSpring = true;
            
            if(_armSpring && _detectedCollision)
            {
                TurnSpringOn();
            }
            if(_armSpring && !_detectedCollision)
            {
                TurnSpringOff();
                _armSpring = false;
            }
            _detectedCollision = false;
        }        

        protected void TurnSpringOn()
        {
            if (_currentGrabber != null)
            {
                _currentGrabber.ConfigurableJoint.linearLimitSpring = new SoftJointLimitSpring() { spring = GetComponent<Rigidbody>().mass * 1000 };
                _currentGrabber.ConfigurableJoint.linearLimit = new SoftJointLimit() { limit = 0.001f };
            }
        }

        protected void TurnSpringOff()
        {
            if (_currentGrabber != null)
            {
                _currentGrabber.ConfigurableJoint.linearLimitSpring = new SoftJointLimitSpring();
                _currentGrabber.ConfigurableJoint.linearLimit = new SoftJointLimit();
            }
        }
        void OnCollisionEnter(Collision c)
        {
            _detectedCollision = true;
        }

        void OnCollisionStay(Collision c)
        {
            _detectedCollision = true;
        }

    }
}
