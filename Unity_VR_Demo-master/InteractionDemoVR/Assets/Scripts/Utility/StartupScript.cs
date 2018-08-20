using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractionDemo.Utility
{
    /// <summary>
    /// Script to load Foyer and transport player to startpoint (requires activation from other script)
    /// </summary>
    class StartupScript : MonoBehaviour
    {
        public Transform MovingTransform;

        public int FirstScene = 1;

        IEnumerator Start()
        {
            yield return null;
            SceneManager.LoadScene(FirstScene);
            yield return null;
            var startpoint = GameObject.Find("{STARTPOINT}");
            if (startpoint != null)
            {
                MovingTransform.position = startpoint.transform.position;
            }
            GameObject.Destroy(gameObject);
        }
        
    }
}
