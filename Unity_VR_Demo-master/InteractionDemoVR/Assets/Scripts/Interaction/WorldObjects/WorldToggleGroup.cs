using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    /// <summary>
    /// Class of worldUI buttons
    /// </summary>
    public class WorldToggleGroup : MonoBehaviour
    {
        public  WorldToggle[] Toggles;
        
        void Start()
        {
            for (int i = 0; i < Toggles.Length; i++)
            {
                int _i = i;
                Toggles[i].OnToggleOn.AddListener(()=> { ToggleOn(_i); });
            }
        }


        void ToggleOn(int z)
        {
            for (int i = 0; i < Toggles.Length; i++)
            {
                if(i != z)
                {
                    Toggles[i].IsOn = false;
                }
            }
        }

        public void SetControlsState(bool state)
        {
            for (int i = 0; i < Toggles.Length; i++)
            {
                Toggles[i].SetEnabled(state);
            }
        }

        public void UncheckAll()
        {
            for (int i = 0; i < Toggles.Length; i++)
            {
                Toggles[i].IsOn = false;
            }
        }
    }
}
