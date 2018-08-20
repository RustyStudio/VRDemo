using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteractionDemo.Core
{    
    class Context : SingletonBehaviour<Context>
    {
        protected Context() {}

        public TrackedController[] Controllers;

        void Start()
        {
            
        }       

        void Update()
        {

        }

        public void Cleanup()
        {
            for (int i = 0; i < Controllers.Length; i++)
            {
                Controllers[i].Cleanup();
            }
        }
    }
}
