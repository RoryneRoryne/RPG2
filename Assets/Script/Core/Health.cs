using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public bool IsDead()
         {
            return isDead;
         }

        public void TakeDamage(float damage)
         {
            //berguna agar value dari health tidak akan berkurang dari 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if (healthPoints == 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
           
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            Destroy(GameObject.FindGameObjectWithTag("Enemy"), 10);
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
            if (healthPoints == 0f)
            {
                Die();
            }
        }
    }
}