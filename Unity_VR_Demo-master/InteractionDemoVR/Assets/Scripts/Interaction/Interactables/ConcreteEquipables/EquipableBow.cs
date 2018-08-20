using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    /// <summary>
    /// Grabbable object that is grabbed in specific way
    /// </summary>
    class EquipableBow : EquipableGrabbableObject
    {
        public BowString BowstringPoint;

        protected override void SetEquipedState(bool state)
        {
            base.SetEquipedState(state);

            int layer = state ? LayerMask.NameToLayer(Layers.EQUIPMENT_LAYER) : LayerMask.NameToLayer(Layers.INTERACTABLE_LAYER);
            EquipmentTransform.gameObject.SetLayerRecursively(layer);

            if (state)
                BowstringPoint.Activate();
            else
                BowstringPoint.Deactivate();            
        }
      
    }
}
