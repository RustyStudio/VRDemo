using UnityEngine;

namespace InteractionDemo.Utility
{
    class PersistentObject : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }
}
