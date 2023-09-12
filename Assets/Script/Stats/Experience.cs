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

        public event Action onExpGained;

        //                                      Menambah Exp
        // ########################################################################################
        public void GainExp(float exp)
        {
            //menambahkan expPoints setiap exp dipanggil.
            expPoints += exp;

            //memanggil event onExpGained
            onExpGained();
        }

        //                                      Mendaptkan Exp
        // ########################################################################################
        public float GetExp()
        {
            //mengembalikan nilai expPoints
            return expPoints;
        }

        //                                          Save
        // ########################################################################################
        public object CaptureState()
        {
            //mengembalikan nilai expPoints
            return expPoints;
        }

        //                                          Load
        // ########################################################################################
        public void RestoreState(object state)
        {
            //memuat nilai expPoints dari CaptureState
            expPoints = (float) state;
        }
        
    }
}
