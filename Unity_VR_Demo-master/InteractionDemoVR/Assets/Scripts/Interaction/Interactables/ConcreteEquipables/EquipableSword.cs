using InteractionDemo.Core;
using InteractionDemo.SwordZone;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Grabbable object that is grabbed in specific way
    /// </summary>
    class EquipableSword : EquipableGrabbableObject
    {
        public Slicer SwordBlade;

        void Update()
        {
            if(_currentGrabber != null && SwordBlade.IsSlicing)
            {
                _currentGrabber.Controller.Controller.TriggerHapticPulse();
            }
        }
      
    }
}
