using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationId
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationId destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other) 
        {
            // Cek apakah objek tersebut memiliki tag "Player"
            if (other.tag == "Player")
            {
                // Jika ya, panggil method Transition() sebagai coroutine
                StartCoroutine(Transition());
            }
            
        }

        // Method ini akan memulai sebuah coroutine untuk memuat scene baru
        private IEnumerator Transition()
        {
            //jika scene yang akan di load kurang dari 0
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene belum ke set");
                //hentikan ienumerator
                yield break;
            }
            // Jangan hancurkan game object ini saat memuat scene baru
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            //menyimpan level saat ini
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            //Muat scene baru secara asynchronous
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //memuat level saat ini
            wrapper.Load();

            /*Mendapatkan portal lain yang terhubung dengan 
            portal saat ini dan menyimpannya dalam variabel otherPortal.*/
            Portal otherPortal = GetOtherPortal();

            /*Memindahkan pemain ke portal lain yang terhubung dengan portal saat ini, 
            menggunakan informasi portal yang disimpan dalam variabel otherPortal.*/
            UpdatePlayer(otherPortal);

            //agar state akan disave saat pindah melewait portal (checkpoint).
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            Destroy(gameObject); 
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            // Cari objek dengan tag "Player" dalam scene
            GameObject player = GameObject.FindWithTag("Player");

            //mematikan navmesh agent untuk sementara agar menghindari bug 
            player.GetComponent<NavMeshAgent>().enabled = false;

            // Set posisi dan rotasi player ke posisi spawn point portal lainnya
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;

            //menyalakan ulang navmesh agent
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            // Loop melalui semua objek yang memiliki komponen Portal dalam scene
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                // Jika objek saat ini sama dengan objek ini (this), lanjutkan ke iterasi berikutnya
                if (portal == this) 
                {
                    continue;
                }

                if (portal.destination != destination)
                {
                    continue;
                }

                // Jika objek saat ini bukan sama dengan objek ini (this), return objek tersebut
                return portal;          
            } 
            // Jika tidak ada objek yang ditemukan, kembalikan null
            return null;
        }
    }
}

