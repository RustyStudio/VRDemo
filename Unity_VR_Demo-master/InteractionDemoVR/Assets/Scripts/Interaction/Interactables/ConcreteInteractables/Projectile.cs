using InteractionDemo.Core;
using InteractionDemo.WorldObjects;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.Interaction
{
    class Projectile : GrabbableObject
    {
        public Rigidbody Body;

        public TrailRenderer Trail;

        void Awake()
        {
            Body.isKinematic = true;
            Body.detectCollisions = false;
        }

        public void Launch(float velocity, Vector3 direction)
        {
            transform.SetParent(null);
            Body.isKinematic = false;
            Body.AddForce(direction * velocity, ForceMode.Impulse);
            Body.detectCollisions = true;
            Trail.enabled = true;
        }

        void FixedUpdate()
        {
            CollisionHandled = false;
            if (!Body.isKinematic)
            {
                if (Body.velocity.sqrMagnitude > 30)
                {
                    transform.forward =
                    Vector3.Slerp(transform.forward, Body.velocity.normalized, 1f);
                }
            }
        }

        void OnCollisionEnter(Collision c)
        {
            if (CollisionHandled)
                return;
            CollisionHandled = true;
            var canStick = c.gameObject.GetComponentInParent<ArrowStickable>();
            
            if(canStick)
            {
                Body.isKinematic = true;
                Body.velocity = Vector3.zero;
                Body.angularVelocity = Vector3.zero;
            }
            OnProjectileHit.Invoke(c);
        }

        private bool CollisionHandled = false;

        public CollisionEvent OnProjectileHit;
    }
}
