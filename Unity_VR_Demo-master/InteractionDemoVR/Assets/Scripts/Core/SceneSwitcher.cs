using InteractionDemo.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionDemo.Core
{
    public class SceneSwitcher : MonoBehaviour
    {
        public WorldObjects.ElevatorDoor Door;

        public WorldObjects.WorldToggleGroup ElevatorButtonsGroup;

        public WorldObjects.ElevatorSpeaker Speaker;

        public AudioClip SceneSwitchingFailedSound;

        public PlayerScanner Scanner;

        public void SwitchScene(int newScene)
        {
            StartCoroutine(SceneSwitchingSequence((int)newScene));
        }

        private IEnumerator SceneSwitchingSequence(int newScene)
        {
            ElevatorButtonsGroup.SetControlsState(false);
            Door.CloseDoor();
            Door.LockDoor = true;
            while (!Door.IsDoorClosed)
            {
                yield return null;
            }
            if(!Scanner.IsPlayerInside())
            {
                Speaker.AbortAndPlayErrorClip(SceneSwitchingFailedSound);
                SceneIsLoaded();
                yield break;
            }
            Context.Instance.Cleanup();
            var sceneLoader = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
            sceneLoader.completed += SceneLoader_completed;
        }
        
        private void SceneLoader_completed(AsyncOperation obj)
        {
            SceneIsLoaded();
        }

        private void SceneIsLoaded()
        {
            Door.LockDoor = false;
            Door.OpenDoor();
            StartCoroutine(ActivateElevatorButtons());
        }

        IEnumerator ActivateElevatorButtons()
        {
            while (!Door.IsDoorOpen)
            {
                yield return null;
            }
            ElevatorButtonsGroup.SetControlsState(true);
            ElevatorButtonsGroup.UncheckAll();
        }
        
    }
}