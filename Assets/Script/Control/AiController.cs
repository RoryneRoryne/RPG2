using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AiController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        
        private void Start() 
        {
            fighter = GetComponent<Fighter>();
            //berguna untuk menemukan player menggunakan tag 
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }
        private void Update()
        {
            //jika player mati, player tidak dapat menggerakkan karakternya.
            if (health.IsDead())
            {
                return;
            }
            //agar disaat player mendekati musuh, player akan dikejar.
           if (InAttackRange() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }

            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                //method agar musuh disaat tidak mengejar player, musuh-
                //tidak akan langsung kembali ke posisi awal.
                SuspicionBehavior();
            }

            //agar musuh berhenti mengejar player
            else
            {
                //method untuk membuat enemy kembali ke posisi awal
                
                GuardBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }


        //memakai mover.startmoveaction karena setiap startmoveaction dipanggil-
        //semua kegiatan yang sedang dilakukan akan dibatalkan.
        private void GuardBehavior()
        {
            mover.StartMoveAction(guardPosition);
        }

        //agar musuh tidak langsung kembali ke posisi awal
        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        //method Attack dari fighter, agar musuh memanggil dapat menyerang player.
        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            //untuk megatur jarak musuh dapat menemukan player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //dipanggil dengan unity
        //untuk menampilkan gizmos saat objek di select
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}
