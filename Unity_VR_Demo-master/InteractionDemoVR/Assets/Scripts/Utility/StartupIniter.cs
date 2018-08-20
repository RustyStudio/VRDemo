using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractionDemo.Utility
{
    /// <summary>
    /// Class to enable first startup object (only used in empty main scene)
    /// </summary>
    class StartupIniter : MonoBehaviour
    {        
        public StartupScript StartupObject;

        void Start()
        {
            StartupObject.gameObject.SetActive(true);
        }
        
    }
}
