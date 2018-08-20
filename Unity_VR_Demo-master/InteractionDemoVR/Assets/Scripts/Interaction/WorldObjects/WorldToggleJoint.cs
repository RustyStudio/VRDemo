using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    /// <summary>
    /// Class of worldUI buttons
    /// </summary>
    public class WorldToggleJoint : MonoBehaviour
    {
        public ConfigurableJoint ButtonJoint;

        public float OnLimit;

        public float OffLimit;

        public GameObject ButtonOnVisual;

        public GameObject ButtonOffVisual;

        public void SetState(bool On)
        {
            ButtonJoint.linearLimit = new SoftJointLimit() { limit = On ? OnLimit : OffLimit };
            ButtonOnVisual.SetActive(On);
            ButtonOffVisual.SetActive(!On);
            ButtonJoint.GetComponent<Rigidbody>().WakeUp();

        }

      

    }
}
