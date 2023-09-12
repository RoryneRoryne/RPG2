using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
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
        private void Awake() 
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
            // Perform a raycast from the mouse pointer to the scene, and check if it hits something.
            // Melakukan raycast dari titik pointer mouse ke scene, dan periksa apakah ada yang terkena.
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                // If there is a hit and the left mouse button is pressed, start moving the object to the hit point.
                // Jika ada yang terkena dan tombol kiri mouse ditekan, mulai gerakan objek ke titik yang terkena tersebut.
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            // If there is no hit, or the left mouse button is not pressed, return false to indicate no interaction occurred.
            // Jika tidak ada yang terkena, atau tombol kiri mouse tidak ditekan, kembalikan false untuk menunjukkan tidak ada interaksi yang terjadi.
            return false;
        }

        private static Ray GetMouseRay()
        {
            // Calculate a ray from the camera through the mouse pointer on the screen.
            // Hitung sebuah ray dari kamera melalui pointer mouse pada layar.
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

