using UnityEngine;

namespace InteractionDemo.Utility
{
      /// <summary>
      /// Special class to push objects to initial position
      /// </summary>
     [RequireComponent(typeof(Rigidbody))]
    class Pusher : MonoBehaviour
    {
        public Vector3 PushDirection = new Vector3(0,0,1);

        public float PushPower = 0;

        void Start()
        {
            GetComponent<Rigidbody>().AddRelativeForce(PushDirection * PushPower);
        }
    }
}
