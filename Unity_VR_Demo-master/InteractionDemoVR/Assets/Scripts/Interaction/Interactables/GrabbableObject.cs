using System;
using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    class GrabbableObject : AGrabbable
    {
        protected Grabber _currentGrabber;

        public Grabber CurrentGrabber
        {
            get
            {
                return _currentGrabber;
            }
        }

        /// <summary>
        /// The object will not actualy create joint, but will classify as grabbed. Useful if you don't want to actualy move object
        /// </summary>
        public virtual bool UsesFakeGrab
        {
            get { return false; }
        }

        public virtual bool UseSpring
        {
            get { return false; }
        }

        public void Grab(Grabber newGrabber)
        {
            BeforeGrab(newGrabber);
            if(_currentGrabber != null)
                _currentGrabber.Release();
            _currentGrabber = newGrabber;
            AfterGrab(newGrabber);
        }

        public virtual void BeforeGrab(Grabber newGrabber)
        {

        }

        public virtual void AfterGrab(Grabber newGrabber)
        {

        }

        public virtual void BeforeRelease(Grabber newGrabber)
        {

        }

        public virtual void AfterRelease(Grabber newGrabber)
        {

        }

        public void Release(Grabber grabber)
        {
            BeforeRelease(grabber);
            if(_currentGrabber == grabber)
            {                
                _currentGrabber = null;
            }
            AfterRelease(grabber);
        }

        void OnDestroy()
        {
            if (_currentGrabber != null)
                _currentGrabber.Release();
        }
    }
}
