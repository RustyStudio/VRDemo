using InteractionDemo.Interaction;
using UnityEngine;

namespace InteractionDemo.Utility
{
    class TestingLogger : MonoBehaviour
    {
        public Lever Lever;

        void Start()
        {
            Lever.onLeverValueChanged.AddListener((val) => { Lever_onLeverValueChanged(val); });
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Lever.Value = 0.5f;
        }

        private void Lever_onLeverValueChanged(float newValue)
        {
            Debug.Log(newValue + " - new lever value");
        }
    }
}
