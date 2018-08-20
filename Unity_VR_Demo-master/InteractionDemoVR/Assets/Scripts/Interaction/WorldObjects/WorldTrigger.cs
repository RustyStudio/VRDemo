using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
   [RequireComponent(typeof(Collider))]
    class WorldTrigger:MonoBehaviour
    {
        public LayerMask TriggerLayers;

        public UnityEvent OnTriggerOn;

        public UnityEvent OnTriggerOff;

        void OnTriggerEnter(Collider other)
        {
            if(TriggerLayers == (TriggerLayers | (1 << other.gameObject.layer)))
            {
                OnTriggerOn.Invoke();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (TriggerLayers == (TriggerLayers | (1 << other.gameObject.layer)))
            {
                OnTriggerOff.Invoke();
            }
        }

    }
}
