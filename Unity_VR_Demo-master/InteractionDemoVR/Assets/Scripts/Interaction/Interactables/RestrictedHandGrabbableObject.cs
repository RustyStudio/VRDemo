using System;
using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// GrabbableObject that restricts movement of hand
    /// </summary>
    class RestrictedHandGrabbableObject : SpringGrabbableObject
    {
        public Transform RestrictionTransform;       

        public override void AfterGrab(Grabber newGrabber)
        {
            newGrabber.Controller.RestrictHandMovement(RestrictionTransform);
        }

        public override void BeforeRelease(Grabber grabber)
        {
            _currentGrabber.Controller.FreeHandMovement();
        }
        

        public virtual void Activate()
        {

        }   

        public virtual void Deactivate()
        {

        }
            
    }
}
