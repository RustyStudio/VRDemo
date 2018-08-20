using InteractionDemo.Core;
using InteractionDemo.Interaction;
using InteractionDemo.WorldObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.SwordZone
{    
    public class Slicer:MonoBehaviour
    {
        private float Cooldown = 2f;

        public float SliceVelocity = 20;

        public Rigidbody BladeBody;

        public Collider BladeCollider;

        public TrailRenderer BladeTrail;

        public UnityEvent OnSlice;

        private Dictionary<Collider, Vector3> SlicerVectors;

        private Coroutine TrailStopper;

        public bool IsSlicing
        {
            get
            {
                return SlicerVectors.Count > 0;
            }
        }

        void OnCollisionEnter(Collision c)
        {
        
        }

        void Start()
        {
            SlicerVectors = new Dictionary<Collider, Vector3>();
        }

        void StartTrail()
        {
            if(TrailStopper != null)
            {
                StopCoroutine(TrailStopper);
                TrailStopper = null;
            }
            BladeTrail.time = 0.3f;
        }

        void StopTrail()
        {
            if (TrailStopper != null)
            {
                StopCoroutine(TrailStopper);
                TrailStopper = null;
            }
            StartCoroutine(StopTrailRoutine());
        }

        IEnumerator StopTrailRoutine()
        {
            float curSpeed = BladeTrail.time;
            while (curSpeed > 0)
            {
                curSpeed -= Time.deltaTime;
                BladeTrail.time = curSpeed;
                yield return null;
            }
            BladeTrail.time = 0;
        }

        void FixedUpdate()
        {
            if(!BladeCollider.isTrigger)
                SlicerVectors.Clear();
            if (BladeBody.velocity.sqrMagnitude > SliceVelocity)
            {
                StartTrail();
                BladeCollider.isTrigger = true;
            }
            else
            {
                StopTrail();
                BladeCollider.isTrigger = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var slicable = other.GetComponentInParent<ISlicable>();
            if (slicable != null)
            {
                if (!SlicerVectors.ContainsKey(other))
                {
                    SlicerVectors[other] = transform.position ;
             
                }
            }
        }


        void OnTriggerExit(Collider other)
        {
            if (SlicerVectors.ContainsKey(other))
            {
                var slicable = other.GetComponentInParent<ISlicable>();
                if (slicable != null)
                {
                    var centerPoint = (transform.position - SlicerVectors[other]) / 2;
                    var cross = Vector3.Cross(transform.up, centerPoint);
                    Plane p = new Plane(other.transform.InverseTransformDirection(cross), other.transform.InverseTransformPoint(transform.position + centerPoint));
                    slicable.Slice(p);
                    OnSlice.Invoke();
                }
                SlicerVectors.Remove(other);
            }
        }
    }
}
