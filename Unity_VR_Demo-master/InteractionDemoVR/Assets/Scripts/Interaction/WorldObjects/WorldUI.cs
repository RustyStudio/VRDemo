using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    /// <summary>
    /// Base class for worldUI elements
    /// </summary>
    public abstract class WorldUI : MonoBehaviour
    {
        public abstract void SetEnabled(bool enabled);

        protected bool IsEnabled = true;
       
    }
}
