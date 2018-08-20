using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteractionDemo.Core
{
    public static class Extensions
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }
    }
}
