using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
        public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        IEnumerator FadeOutIn()
            {
                //Memulai Coroutine untuk fade out selama 3 detik.
                yield return FadeOut(3f);
                Debug.Log("FadeOut");
                //Memulai Coroutine untuk fade in selama 2 detik.
                yield return FadeIn(2f);
                Debug.Log("FadeIn");
            }    
        public IEnumerator FadeOut(float time)
            {
                //Looping selama nilai alpha pada canvasGroup kurang dari 1.
                while (canvasGroup.alpha < 1) 
                {
                    //Meningkatkan alpha berdasarkan waktu delta dan waktu yang dibutuhkan untuk fade out.
                    canvasGroup.alpha += Time.deltaTime/time;
                    yield return null;
                }
            }
            public IEnumerator FadeIn(float time)
            {
                //Looping selama nilai alpha pada canvasGroup lebih besar dari 0.
                while (canvasGroup.alpha > 0 && canvasGroup != null) 
                {
                    //Mengurangi alpha berdasarkan waktu delta dan waktu yang dibutuhkan untuk fade in.
                    canvasGroup.alpha -= Time.deltaTime/time;
                    yield return null;
                }
            }    
    }
}

