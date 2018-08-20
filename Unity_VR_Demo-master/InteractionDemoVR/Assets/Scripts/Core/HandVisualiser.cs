using UnityEngine;

namespace InteractionDemo.Core
{
    public class HandVisualiser : MonoBehaviour
    {
        public void SetTrackedTransform(Transform t)
        {
            transform.SetParent(t, true);           
        }      

        public void ResetPositionAndRotation()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
