using InteractionDemo.Core;

namespace InteractionDemo.Interaction
{
    abstract class ASwitchable: AInteractableObject
    {
        protected override void Reset()
        {
            HoverHandState = HandStateEnum.Point;
        }

        public abstract void Switch();
    }
}
