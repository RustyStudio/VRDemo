using InteractionDemo.Core;

namespace InteractionDemo.Interaction
{
    abstract class AGrabbable : AInteractableObject
    {
        protected override void Reset()
        {
            HoverHandState = HandStateEnum.CanGrab;
            ActivationHandState = HandStateEnum.Grab;
        }
        
    }
}
