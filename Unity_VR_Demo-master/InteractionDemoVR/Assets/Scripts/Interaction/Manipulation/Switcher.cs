using System;
using InteractionDemo.Core;
using UnityEngine;


namespace InteractionDemo.Interaction
{
    [RequireComponent(typeof(Scanner))]
    class Switcher : MonoBehaviour, IHandModule
    {
        private Scanner _scanner;

        private TrackedController _controller;

        public void Setup(TrackedController controller)
        {
            _controller = controller;
            _controller.OnTriggerDown += TryPush;
            _scanner = GetComponent<Scanner>();
        }
        
        private void TryPush(TrackedController sender, float TriggerValue)
        {          
            var toPush = _scanner.GetClosestInterractableObject();
            if(toPush != null)
            {              
                var pushedObject = toPush.GetComponentInParent<ASwitchable>();
                if (pushedObject != null)
                {
                    pushedObject.Switch();
                    Debug.Log("Pushed " + toPush.name);
                }
            }
           
        }

        public void Cleanup()
        {

        }
    }
}
