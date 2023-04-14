using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics

{
    public class CinematicTrigger : MonoBehaviour
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
    }
}

