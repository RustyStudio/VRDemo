using InteractionDemo.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionDemo.WorldObjects
{
    public class ElevatorSpeaker : MonoBehaviour
    {
        public AudioSource Speaker;

        private Coroutine SpeakerRoutine;

        public void AnounceNextScene(AudioClip nextSceneClip)
        {
            Speaker.Play();
            if (SpeakerRoutine != null)
                StopCoroutine(SpeakerRoutine);
            SpeakerRoutine = StartCoroutine(NextSceneRoutine(nextSceneClip));
          
        }

        public void AbortAndPlayErrorClip(AudioClip errorClip)
        {
            if (SpeakerRoutine != null)
                StopCoroutine(SpeakerRoutine);
            Speaker.Stop();
            Speaker.PlayOneShot(errorClip);
        }

        IEnumerator NextSceneRoutine(AudioClip nextSceneClip)
        {
            while (Speaker.isPlaying)
            {
                yield return null;
            }
            Speaker.PlayOneShot(nextSceneClip);
        }
    }
}
