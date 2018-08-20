using System;
using InteractionDemo.Core;
using UnityEngine;

namespace InteractionDemo.Interaction
{
    class BowString : RestrictedHandGrabbableObject
    {
        public Collider BowstringCollider;

        public GameObject BowstringHintObject;

        public Animator BowstringAnimator;

        public Transform BowCheckPosition;

        public Projectile ProjectilePrefab;

        public AudioSource ProjectileLaunchSound;

        public float MaxOffset = 1;

        public float MaxForce = 100;

        private float _currentOffset = 0;

        public string AnimatorParameter = "Blend";

        private Projectile _currentProjectile;

        public override bool UsesFakeGrab
        {
            get
            {
                return true;
            }
        }

        public override void Activate()
        {
            BowstringCollider.enabled = true;
            BowstringHintObject.SetActive(true);
        }

        public override void Deactivate()
        {
            BowstringCollider.enabled = false;
            BowstringHintObject.SetActive(false);
            if (_currentGrabber != null)
            {
                _currentGrabber.Release();
            }
        }

        public override void AfterGrab(Grabber newGrabber)
        {
            base.AfterGrab(newGrabber);
            _currentProjectile =  GameObject.Instantiate(ProjectilePrefab);
            _currentProjectile.transform.SetParent(transform, false);
            _currentOffset = 0;
            ArcheryZone.ArcheryHallManager.Instance.RegisterProjectile(_currentProjectile);
        }

        public override void AfterRelease(Grabber newGrabber)
        {
            base.AfterRelease(newGrabber);
            BowstringAnimator.SetFloat(AnimatorParameter, 0);
            LaunchProjectile();
        }

        void LaunchProjectile()
        {
            _currentProjectile.Launch(_currentOffset * MaxForce, BowCheckPosition.up * -1);
            ProjectileLaunchSound.Play();
            _currentProjectile = null;
        }

        void Update()
        {
            if (_currentGrabber != null)
            {
                var difference = BowCheckPosition.position - _currentGrabber.Controller.transform.position;
                var projected = Vector3.Project(difference, BowCheckPosition.up);

                _currentOffset = Mathf.Clamp01(Mathf.InverseLerp(0, MaxOffset, projected.magnitude));

                BowstringAnimator.SetFloat(AnimatorParameter, _currentOffset);
                _currentGrabber.Controller.Controller.TriggerHapticPulse((ushort)(_currentOffset * 1000));
            }
        }
    }
}
