using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Core
{
    public class Health : MonoBehaviour
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
        }
    }
}