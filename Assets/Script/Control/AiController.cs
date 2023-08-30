using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;

namespace RPG.Control
{
    public class AiController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float  waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currenWayPointIndex = 0;
        
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
            //when player is near enemy,the enemy will chase player.
            //agar disaat player mendekati musuh, player akan dikejar.
            if (InAttackRange() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }

            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //a method to delay enemy from going back to it original position whenever it not chasing  the player
                //method untuk mendelay musuh agar musuh tidak langsung kembali ke posisi awal saat tidak mengejar player
                SuspicionBehavior();
            }

            //agar musuh berhenti mengejar player
            else
            {
                //a method to make enemy go back to it original position.
                //method untuk membuat enemy kembali ke posisi awal.
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }


        //memakai mover.startmoveaction karena setiap startmoveaction dipanggil-
        //semua kegiatan yang sedang dilakukan akan dibatalkan.
        private void PatrolBehavior()
        {
            //this code to make
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            //this code will make the npc to stay at waypoint within amount certain time.
            //kode ini berfungsi untuk membuat npc diam pada waypoint dalam kurun waktu tertentu.
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        //this method is to make an object know they're at the waypoint.
        //method ini befungsi untuk membuat suatu object tau bahwa mereka berada di waypoint.
        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            //this is to check if the distanceToWayPoint is less than wayPointTolerance, it will return true.
            //untuk mengecek jika distanceToWaypoint kurang dari wayPointTolerence, akan return true.
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWayPoint()
        {
            //this statement is to updates the value of currenWayPointIndex by using patrolPath to call GetNextIndex on currenWayPointIndex.
            // statement ini beguna untuk mengupdate value currenWayPointIndex dengan menggunakan patrolPath untuk memanggil GetNextIndex pada currenWayPointIndex.
            currenWayPointIndex = patrolPath.GetNextIndex(currenWayPointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            //this function is to call patrolPath to retrieve the WayPoint at the currenWayPointIndex.
            //fungsi ini berguna untuk memanggil patrolPath untuk mengambil WayPoint pada currenWayPointIndex.
            return patrolPath.GetWayPoint(currenWayPointIndex);
        }

        //agar musuh tidak langsung kembali ke posisi awal
        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelAction();
        }

        //method Attack dari fighter, agar musuh memanggil dapat menyerang player.
        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
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
