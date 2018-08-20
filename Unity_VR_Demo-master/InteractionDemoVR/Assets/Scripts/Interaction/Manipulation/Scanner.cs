using InteractionDemo.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace InteractionDemo.Interaction
{
    [RequireComponent(typeof(Collider))]
    class Scanner: MonoBehaviour, IHandModule
    {
        private HashSet<Transform> InteractableObjects;

        private TrackedController _controller;

        private Collider _collider;

        void Awake()
        {
            InteractableObjects = new HashSet<Transform>();
            _collider = GetComponent<Collider>();
        }

        public void Setup(TrackedController controller)
        {
            _controller = controller;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == Strings.TAG_INTERACTABLE)
            {
                InteractableObjects.Add(other.GetComponent<Transform>());
                SwitchHandState();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.transform.tag == Strings.TAG_INTERACTABLE)
            {
                InteractableObjects.Remove(other.GetComponent<Transform>());
                SwitchHandState();
            }
        }

        private void SwitchHandState()
        {
            var t = GetClosestInterractableObject();
            if (t != null)
            {
                _controller.HandController.SetHandState(t.HoverHandState);
            }
            else
            {
                _controller.HandController.SetHandState(HandStateEnum.Free);
            }
        }

        public AInteractableObject GetClosestInterractableObject()
        {
            float magnitude = float.MaxValue;
            AInteractableObject closest = null;
            var filteredObjs = new HashSet<Transform>();
            foreach (var item in InteractableObjects)
            {
                if (item != null)
                    filteredObjs.Add(item);
            }
            InteractableObjects = filteredObjs;
            foreach (var item in InteractableObjects)
            {
                var colliders = item.GetComponentsInChildren<Collider>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (!colliders[i].enabled)
                        continue;
                    if ((colliders[i].bounds.center - transform.position).sqrMagnitude < magnitude)
                    {
                        var component = item.GetComponentInParent<AInteractableObject>();
                        if (component != null)
                            closest = component;
                    }
                }            
              
            }
            return closest;
        }

        public void Cleanup()
        {
            InteractableObjects.Clear();
        }
    }
}
