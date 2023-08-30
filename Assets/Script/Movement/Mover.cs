using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Start() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            //this code is to make navMeshAgent enalbed whenever the player isn't dead, and when the player is dead it will disable navMeshAgent.
            //kode ini befungsi untuk mengaktifkan navMeshAgent disaat player tidak mati, tetapi akan menonaktifkan navMeshAgent disaat player mati.
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction (Vector3 destination, float speedFraction)
        {
            // Get the ActionScheduler component from the current game object, and call the StartAction method on it, passing in 'this' as the argument.
            // Ambil komponen ActionScheduler dari game object saat ini, dan panggil metode StartAction di dalamnya, dengan 'this' sebagai argumennya.
            GetComponent<ActionScheduler>().StartAction(this);

            // Call the MoveTo method with the given destination and speedFraction arguments.
            // Panggil metode MoveTo dengan argumen tujuan dan fraksi kecepatan yang diberikan.
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            // Set the destination of the NavMeshAgent component attached to this game object to the given destination.
            //Setel tujuan NavMeshAgent yang terpasang pada game object ini menjadi tujuan yang diberikan.
            navMeshAgent.destination = destination;

            // Set the speed of the NavMeshAgent to the maximum speed (stored in the maxSpeed field) multiplied by the given speedFraction (clamped between 0 and 1 using Mathf.Clamp01).
            // Setel kecepatan NavMeshAgent menjadi kecepatan maksimum (yang disimpan dalam variabel maxSpeed) dikali dengan fraksi kecepatan yang diberikan (diklem antara 0 dan 1 menggunakan Mathf.Clamp01).
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);

            // Set isStopped to false to allow the NavMeshAgent to start moving towards the destination.
            // Setel isStopped menjadi false untuk mengizinkan NavMeshAgent mulai bergerak menuju tujuan.
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }
        //fungsi ini menreturn kondisi object yang memiliki script sebagai new SerializableVector3
        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3 (transform.position);
            data.rotation = new SerializableVector3 (transform.eulerAngles);
            return data;
        }

        //mengembalikan kondisi object
        public void RestoreState(object state)
        {
            //mengcasting object yang diberikan ke dalam dictionary
            MoverSaveData data = (MoverSaveData)state;

            //mematikan navmesh agent agar tidak bermasalah
            GetComponent<NavMeshAgent>().enabled = false;

            //mengatur posisi objek ke posisi sbelumnya
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
