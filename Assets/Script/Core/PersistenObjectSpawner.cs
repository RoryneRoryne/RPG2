using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
        public class PersistenObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject PersistentObjectPrefab;
        static bool hasSpawned = false;
        private void Awake() 
        {
            if (hasSpawned)
            {
                return;
            }

            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(PersistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
