using InteractionDemo.Interaction;
using UnityEngine;
using TMPro;


namespace InteractionDemo.Utility
{
    public class PlayerScanner : MonoBehaviour
    {
        public LayerMask PlayerMask;

        
        public bool IsPlayerInside()
        {
           var result =  Physics.OverlapBox(transform.position, transform.localScale/2, Quaternion.identity, PlayerMask.value);
            return result.Length > 0;
        }
    }
}
