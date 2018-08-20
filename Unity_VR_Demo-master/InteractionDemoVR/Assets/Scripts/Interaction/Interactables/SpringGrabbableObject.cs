using System;
using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    class SpringGrabbableObject : GrabbableObject
    {
        public override bool UseSpring
        {
            get { return true; }
        }
    }
}
