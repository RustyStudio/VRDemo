using System;
using InteractionDemo.Core;
using UnityEngine;


namespace InteractionDemo.Interaction
{
    class Grabber: MonoBehaviour, IHandModule
    {
        public float ReleaseDistance = 20f;

        private bool IsGrabbing = false;    

        private TrackedController _controller;

        public TrackedController Controller
        {
            get { return _controller; }
        }

        public Scanner Scanner;

        public GrabbableObject GrabbedObject;

        public ConfigurableJoint ConfigurableJoint;

        public SpringJoint SpringJoint;

        private Joint CurrentlyActiveJoint;

        private float _grabbedMagnitude;

        public void Setup(TrackedController controller)
        {
            _controller = controller;
            _controller.OnTriggerDown += TryGrab;
            _controller.OnTriggerUp += Release;
            CurrentlyActiveJoint = ConfigurableJoint;
        }        

        void Update()
        {
            if (IsGrabbing && ConfigurableJoint.connectedBody != null)
            {
                var currentDistance = Mathf.Abs(_grabbedMagnitude - (ConfigurableJoint.transform.position - ConfigurableJoint.connectedBody.transform.position).sqrMagnitude);

                if (currentDistance > ReleaseDistance)
                {                 
                    Release();
                }
            }
        }

        public void Release()
        {
            if (IsGrabbing)
            {
                IsGrabbing = false;
                Debug.Log("Released " + GrabbedObject.name);
                if (CurrentlyActiveJoint.connectedBody != null)
                {
                    CurrentlyActiveJoint.connectedBody.angularVelocity = _controller.Controller.angularVelocity;
                    CurrentlyActiveJoint.connectedBody.velocity = _controller.Controller.velocity;
                    CurrentlyActiveJoint.connectedBody = null;
                }
                GrabbedObject.Release(this);
                GrabbedObject = null;
                _grabbedMagnitude = 0;
                _controller.HandController.SetGrabState(false, HandStateEnum.Free);
            }
        }

        private void Release(TrackedController sender, float TriggerValue)
        {
            Release();
        }

        private void TryGrab(TrackedController sender, float TriggerValue)
        {
            if(IsGrabbing)
            {
                Debug.Log("Grabber is Grabbing already, something went wrong");
                return;
            }
          
            var toGrab = Scanner.GetClosestInterractableObject();
            if(toGrab != null)
            {              
                var grabbedObject = toGrab.GetComponentInParent<GrabbableObject>();
                if (grabbedObject != null && grabbedObject.CurrentGrabber ==null)
                {                    
                    GrabbedObject = grabbedObject;
                    IsGrabbing = true;
                    GrabbedObject.Grab(this);
                    Debug.Log("Grabbed " + GrabbedObject.name);
                    if (!grabbedObject.UsesFakeGrab)
                    {
                        if (grabbedObject.UseSpring)
                            CurrentlyActiveJoint = SpringJoint;
                        else
                            CurrentlyActiveJoint = ConfigurableJoint;
                        CurrentlyActiveJoint.connectedBody = GrabbedObject.GetComponentInParent<Rigidbody>();
                        _grabbedMagnitude = (CurrentlyActiveJoint.transform.position - CurrentlyActiveJoint.connectedBody.transform.position).sqrMagnitude;
                    }
                    _controller.HandController.SetGrabState(true, GrabbedObject.ActivationHandState);
                }
            }
           
        }

        public void Cleanup()
        {
            Release();
        }
    }
}
