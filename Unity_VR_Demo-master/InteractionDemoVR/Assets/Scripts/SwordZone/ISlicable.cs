using InteractionDemo.Core;
using InteractionDemo.Interaction;
using InteractionDemo.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace InteractionDemo.SwordZone
{
    public interface ISlicable
    {
        void Slice(Plane p);
    }
}
