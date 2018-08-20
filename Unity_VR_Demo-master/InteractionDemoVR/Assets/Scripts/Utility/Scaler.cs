using UnityEngine;

namespace InteractionDemo.Utility
{
    class Scaler : MonoBehaviour
    {
        public Vector3 ScaleAxis;

        public void SetScale(float value)
        {
            transform.localScale = Vector3.one - ScaleAxis * value;
        }


    }
}
