using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
{

    const string defaultSaveFile = "Save";
    [SerializeField] float fadeInTime = 0.2f;

    private void Awake() 
    {
        StartCoroutine(LoadLastScene());
    }

    //agar start mereturn ienumarator, dengan ini start menjadi courtine
    private IEnumerator LoadLastScene() 
    {
        //untuk meload scene yang sebelumnya tersave kemudian laod kondisi pada secene itu.
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        
        yield return fader.FadeIn(fadeInTime);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
        }

    }

        public void Save()
        {
            //memanggil method save dari saving system
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            //memanggil method load dari saving system
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
