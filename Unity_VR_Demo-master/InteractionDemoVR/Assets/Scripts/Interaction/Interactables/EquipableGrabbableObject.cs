using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Grabbable object that is grabbed in specific way
    /// </summary>
    class EquipableGrabbableObject : GrabbableObject
    {
        public Transform EquipPoint;

        public Transform EquipmentTransform;

        protected override void Reset()
        {
            HoverHandState = HandStateEnum.Grab;
            ActivationHandState = HandStateEnum.EquipPole;
        }

        public override void AfterRelease(Grabber grabber)
        {
            SetEquipedState(false);
        }

        public override void AfterGrab(Grabber grabber)
        {                       
            EquipmentTransform.rotation = grabber.ConfigurableJoint.transform.rotation * EquipPoint.transform.localRotation;
            EquipmentTransform.position += grabber.ConfigurableJoint.transform.position - EquipPoint.transform.position;
            SetEquipedState(true);           
        }

        protected virtual void SetEquipedState(bool state)
        {
            int layer = state ? LayerMask.NameToLayer(Layers.EQUIPMENT_LAYER) : LayerMask.NameToLayer(Layers.INTERACTABLE_LAYER);
            EquipmentTransform.gameObject.SetLayerRecursively(layer);
        }
      
    }
}
