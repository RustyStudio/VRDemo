using InteractionDemo.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    public class ElevatorDoor : MonoBehaviour
    {
        public bool LockDoor = false;

        public bool DoorOpening = false;

        public float DoorOpeningSpeed = 1f;

        public float OpenScale = 0;

        public float ClosedScale = 2f;

        public AudioSource OpenDoorAudio;

        public AudioSource CloseDoorAudio;

        private float _currentScale = 2f;

        public bool IsDoorClosed
        {
            get
            {
                return transform.localScale.y == ClosedScale;
            }
        }


        public bool IsDoorOpen
        {
            get
            {
                return transform.localScale.y == OpenScale;
            }
        }

        public void OpenDoor()
        {
            if (!LockDoor)
            {
                DoorOpening = true;
                CloseDoorAudio.Stop();
                OpenDoorAudio.Play();
            }
        }

        public void CloseDoor()
        {
            if (!LockDoor)
            {
                DoorOpening = false;
                OpenDoorAudio.Stop();
                CloseDoorAudio.Play();
            }
        }        

        void Update()
        {
            if (DoorOpening && _currentScale > OpenScale)
            {
                _currentScale -= Time.deltaTime * DoorOpeningSpeed;
                _currentScale = Mathf.Clamp(_currentScale, OpenScale, ClosedScale);
                var scale = new Vector3(transform.localScale.x, _currentScale, transform.localScale.z);
                transform.localScale = scale;
            }
            if (!DoorOpening && _currentScale < ClosedScale)
            {
                _currentScale += Time.deltaTime * DoorOpeningSpeed;
                _currentScale = Mathf.Clamp(_currentScale, OpenScale , ClosedScale);
                var scale = new Vector3(transform.localScale.x, _currentScale, transform.localScale.z);
                transform.localScale = scale;
            }
        }

    }
}
