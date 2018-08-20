using InteractionDemo.Interaction;
using UnityEngine;
using TMPro;


namespace InteractionDemo.Utility
{
    class TextCounter : MonoBehaviour
    {
        public TextMeshPro Text;

        private int i = 0;

        public void Bump(int z = 1)
        {
            i += z;
            Text.text =i.ToString(); ;
        }
    }
}
