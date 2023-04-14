using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {   
        GameObject player;
        private void Start() 
        {
            /* Attach an event listener to the "stopped" and "played" event of the 
            PlayableDirector component.*/
            /* Menambahkan pendengar acara pada event 
            "stopped" dan "played" dari komponen PlayableDirector.*/
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }
        void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelAction();
            player.GetComponent<PlayerController>().enabled =false;
        }
        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled =true;
        }
    }
}

