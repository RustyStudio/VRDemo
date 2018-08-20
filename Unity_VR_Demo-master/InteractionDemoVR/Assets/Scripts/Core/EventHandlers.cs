using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.Core
{
    public delegate void TriggerEventHandler(TrackedController sender, float TriggerValue);

    public delegate void ButtonEventHandler(TrackedController sender, bool Value);

    [System.Serializable]
    public class SwitcherEvent : UnityEvent<bool> { }

    [System.Serializable]
    public class LeverEvent : UnityEvent<float> { }
    
    [System.Serializable]
    public class CollisionEvent:UnityEvent<Collision> { }   
}
