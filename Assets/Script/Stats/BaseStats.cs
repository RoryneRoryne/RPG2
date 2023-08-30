using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        //untuk mengatur batas pada level
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharClass charClass;
        [SerializeField] Progression progression = null;


        private void Update() 
        {
            if (gameObject.tag == "Player")
            {
                Debug.Log(GetLevel());
            }
        }

        //Mengambil nilai dari sebuah statistik karakter tertentu berdasarkan parameter 'stat' yang diberikan.
        public float GetStat(Stat stat)
        {
            //Memanggil metode 'GetStat' dari objek 'progression' untuk mengambil nilai 'stat' yang diinginkan berdasarkan level karakter.
            return progression.GetStat(stat, charClass, GetLevel());
        }


        // Fungsi ini mengambil level karakter berdasarkan 'experience' yang diperoleh.
        public int GetLevel()
        {
            // Mendapatkan komponen 'Experience' dari objek ini.
            Experience experience = GetComponent<Experience>();

            // Jika komponen 'experience' tidak ditemukan, maka level awal ('startingLevel') dikembalikan.
            if (experience == null)
            {
                return startingLevel;
            }

            // Mendapatkan jumlah 'experience' saat ini.
            float currentExp = experience.GetExp();

            // Mendapatkan level maksimal yang bisa dicapai berdasarkan statistik kelas karakter.
            int maxLevel = progression.GetLevels(Stat.ExpToLevelUp, charClass);
            
             // Loop untuk memeriksa setiap level yang dapat diperoleh.
            for (int level = 1; level <= maxLevel; level++)
            {
                // Mendapatkan jumlah 'experience' yang dibutuhkan untuk naik level.
                float ExpToLevelUp = progression.GetStat(Stat.ExpToLevelUp, charClass, level);
                
                 // Jika jumlah 'exp' yang dibutuhkan untuk naik level lebih besar dari 'exp' saat ini, maka level saat ini dikembalikan.
                if (ExpToLevelUp > currentExp)
                {
                    return level;
                }
            }
            
            // Jika total 'experience' belum cukup untuk naik ke level berikutnya , maka level maksimal + 1 dikembalikan.
            return maxLevel + 1;
        }
    }

}