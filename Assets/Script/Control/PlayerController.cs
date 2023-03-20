using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
// using UnityEngine.InputSystem;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        private void Start() 
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            //whenvener IsDead method is true, it will stop all function and call IsDead method.
            //setiap IsDead method memiliki nilai true, kode ini akan menghentikan fungsi lain dan menajalankan method.
            if (health.IsDead())
            {
                return;
            }
            //whenver InteractWithCombat method is true, it will stop the other fucntion and do the method.
            //setiap InteractWithCombat method memiliki nilai true, kode ini akan menghentikan fungsi lain dan menajalankan method.
            if (InteractWithCombat()) return;
            
            //whenver InteractWithMovement method is true, it will stop the other fucntion and do the method.
            //setiap InteractWithMovement method memiliki nilai true, kode ini akan menghentikan fungsi lain dan menajalankan method.
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
               CombatTarget target = hit.transform.GetComponent<CombatTarget>();
               if (target == null)
               {
                    continue;
               }
               if (!GetComponent<Fighter>().CanAttack(target.gameObject))
               {
                    continue;
               }
               if (Input.GetMouseButtonDown(0))
               {
                    GetComponent<Fighter>().Attack(target.gameObject);
               }
               return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

