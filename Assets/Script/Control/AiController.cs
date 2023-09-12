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
        

        //                                          Awake
        // ########################################################################################
        private void Awake() 
        {   
            //untuk membuat cache dengan mengambil metode Fighter dan menamainya fighter
            fighter = GetComponent<Fighter>();
            
            //untuk membuat cache dengan mengambil Tag yang bernama "Player dan menamainya player
            player = GameObject.FindWithTag("Player");

            //untuk membuat cache dengan mengambil metode Health dan menamainya health
            health = GetComponent<Health>();

            //untuk membuat cache dengan mengambil metode Mover dan menamainya mover
            mover = GetComponent<Mover>();
        }

        //                                          Start
        // #################################################################################################
        private void Start() 
        {
            //untuk menginilisasi posisi awal dari npc.
            guardPosition = transform.position;
        }

        //                                          Update
        // ###################################################################################################
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
                AttackBehavior();
            }

            //agar terdapat delay saat musuh berhenti mengejar
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //method untuk mendelay musuh agar musuh tidak langsung kembali ke posisi awal saat tidak mengejar player
                SuspicionBehavior();
            }

            //agar musuh berhenti mengejar player
            else
            {
                //method untuk membuat enemy kembali ke posisi awal.
                PatrolBehavior();
            }

            //untuk mereset timer
            UpdateTimers();
        }


        //                                          Update Timer
        // ####################################################################################################
        private void UpdateTimers()
        {
            //untuk melacak berapa lama waktu yang telah berlalu sejak musuh terakhir kali melihat player.
            timeSinceLastSawPlayer += Time.deltaTime;
            //untuk melacak berapa lama waktu yang telah berlalu sejak musuh tiba di titik patroli.
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        //                           Untuk mengnendalikan perilaku patroli musuh
        // ######################################################################################################
        private void PatrolBehavior()
        {
            //untuk mencahce guardPosition menjadi nextPosition
            Vector3 nextPosition = guardPosition;

            //jika memiliki patrolPath
            if (patrolPath != null)
            {
                //untuk memeriksa jika npc mencapai suatu titik tertentu dalam jalur patroli
                if (AtWayPoint())
                {
                    //mereset timeSinceArrivedAtWaypoint
                    timeSinceArrivedAtWaypoint = 0f;

                    //memanggil metode CycleWayPoint untuk jalan ke posisi berikutnya
                    CycleWayPoint();
                }
                //untuk memperbarui variabel nextPositon menjadi dengan metode GetCurrentWayPoint
                nextPosition = GetCurrentWayPoint();
            }

            //kode ini berfungsi untuk membuat npc diam pada waypoint dalam kurun waktu tertentu.
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                //agar mush berjalan menuju posisi berikutnya
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        //                                           AtWayPoint
        // ######################################################################################################
        //method ini befungsi untuk membuat npc tau bahwa mereka berada di waypoint.
        private bool AtWayPoint()
        {
            //untuk memeriksa jarak antara npc dengan WayPoint.
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            //untuk mengecek jika distanceToWaypoint kurang dari wayPointTolerence, akan return true.
            return distanceToWaypoint < waypointTolerance;
        }

        //                                 Untuk menuju WayPoint berikutnya
        // ######################################################################################################
        private void CycleWayPoint()
        {
            
            // statement ini beguna untuk mengupdate value currenWayPointIndex dengan menggunakan patrolPath untuk - 
            // memanggil GetNextIndex pada currenWayPointIndex.
            currenWayPointIndex = patrolPath.GetNextIndex(currenWayPointIndex);
        }

        //                                 Untuk mengambil posisi patroli saat ini
        // ######################################################################################################
        private Vector3 GetCurrentWayPoint()
        {
            //fungsi ini berguna untuk memanggil patrolPath untuk mengambil WayPoint pada currenWayPointIndex.
            return patrolPath.GetWayPoint(currenWayPointIndex);
        }

        //                                          SuspicionBehavior
        // ######################################################################################################
        //agar musuh tidak langsung kembali ke posisi awal
        private void SuspicionBehavior()
        {
            //untuk memanggil metode CancelAction pada ActionScheduler
            GetComponent<ActionScheduler>().CancelAction();
        }

        //                                        Mengatur Perilaku attack npc
        // ######################################################################################################
        private void AttackBehavior()
        {
            //mereset timeSinceLastSawPlayer
            timeSinceLastSawPlayer = 0;

            //untuk menyerang player dengan memanggi metode Attack pada script fighter
            fighter.Attack(player);
        }

        //                                    Mengecek apakah musuh dalam jarak menyerang
        // ######################################################################################################
        private bool InAttackRange()
        {
            //untuk megatur jarak musuh dapat menemukan player
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            //untuk memeriksa apakah jarak dengan player sudah melebihi jarkak mengejar
            return distanceToPlayer < chaseDistance;
        }

        //                                    OnDrawGizmosSelected
        // ######################################################################################################
        //untuk menampilkan gizmos saat objek di select
        private void OnDrawGizmosSelected() 
        {
            //untuk membuat garis gizmo menjad warna cyan.
            Gizmos.color = Color.cyan;
            
            //untuk membuat gizmos yang berbentuk kubah sebagai penentu radius mengejar.
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

    }
}
