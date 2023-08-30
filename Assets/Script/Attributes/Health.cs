using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        // float deathTimer = 3f;
        float healthPoints = -1f;
        bool isDead = false;   

        private void Start() 
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            } 
        }

        public bool IsDead()
         {
            return isDead;
         }

        public void TakeDamage(GameObject instigator, float damage)
         {
            //berguna agar value dari health tidak akan berkurang dari 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if (healthPoints == 0f)
            {
                Die();
                ExperienceGain(instigator);
            }
        }

        public float GetPercentage()
        {
            // Menghitung persentase health objek.
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (isDead) return;
           
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            // Destroy(gameObject, deathTimer);
        }

        private void ExperienceGain(GameObject instigator)
        {
            // Mendapatkan komponen Experience dari GameObject instigator.
            Experience exp = instigator.GetComponent<Experience>();
            // Memeriksa jika exp null, kalau iya return.
            if (exp == null)
            {
                return;
            }
            // Memanggil metode GainExp() dari komponen Experience
            exp.GainExp(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        //untuk mensave state healthpoints
        public object CaptureState()
        {
            return healthPoints;
        }


        //restore state dari state yang telah tersave
        public void RestoreState(object state)
        {
            healthPoints = (float) state;

            //untuk mengecek jika hp 0 maka objek akan mati
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}