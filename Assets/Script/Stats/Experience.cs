using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float expPoints = 0f;

        public void GainExp(float exp)
        {
            //menambahkan expPoints setiap exp dipanggil.
            expPoints += exp;
        }

        public float GetExp()
        {
            return expPoints;
        }

        public object CaptureState()
        {
            return expPoints;
        }

        public void RestoreState(object state)
        {
            expPoints = (float) state;
        }
        
    }
}
