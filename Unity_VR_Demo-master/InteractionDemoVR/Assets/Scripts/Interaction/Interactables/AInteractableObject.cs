using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    public abstract class AInteractableObject : MonoBehaviour
    {
        public HandStateEnum HoverHandState;

        public HandStateEnum ActivationHandState;

        protected virtual void Reset()
        {
            HoverHandState = HandStateEnum.Free;
            ActivationHandState = HandStateEnum.Free;
        }
    }
}
