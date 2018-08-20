using UnityEngine;

namespace InteractionDemo.Utility
{
    [RequireComponent(typeof(Rigidbody))]
    class CenterOfMassManipulator : MonoBehaviour
    {
        public Vector3 CenterOfMass;
        void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        }
    }
}
