using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics

{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool cmAlreadyPlay = false;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.CompareTag("Player") && !cmAlreadyPlay)
            {
                GetComponent<PlayableDirector>().Play();
                cmAlreadyPlay = true;
            } 
        }

        public object CaptureState()
        {
            return cmAlreadyPlay = true;
        }

        public void RestoreState(object state)
        {
            cmAlreadyPlay = (bool) state;
        }
    }
}

