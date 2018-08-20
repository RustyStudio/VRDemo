using InteractionDemo.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionDemo.Core
{
    public interface IHandModule
    {
        void Setup(TrackedController controller);

        void Cleanup();
    }
}
