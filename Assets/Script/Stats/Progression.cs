using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    // Membuat item menu baru untuk membuat instance dari scriptable object.
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject 
    {
        // Array data progression untuk berbagai kelas karakter.
        [SerializeField] ProgressionCharClass[] charClasses = null;

        Dictionary<CharClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharClass charClass, int level)
        {
            //Memastikan bahwa struktur data lookUpTable telah dibangun.
            BuildLookUp();

            // Mendapatkan array levels dari lookUpTable untuk kelas karakter dan statistik tertentu.
            float[] levels = lookUpTable[charClass][stat];

            // Memeriksa apakah level yang diminta berada dalam rentang array levels.
            if (levels.Length < level)
            {
                // Mengembalikan nilai 0 jika level melebihi batas progresi yang ada.
                return 0;
            }

            // Mengembalikan nilai statistik dari array levels sesuai dengan level yang diminta.
            return levels[level - 1];

        }

        public int GetLevels(Stat stat, CharClass charClass)
        {
            BuildLookUp();
            
            float[] levels = lookUpTable[charClass][stat];
            return levels.Length;
        }


        private void BuildLookUp()
        {
            // Memeriksa apakah lookUpTable sudah dibangun sebelumnya.
            if (lookUpTable != null)
            {
                return;
            }
            
            // Inisialisasi struktur data lookUpTable.
            lookUpTable = new Dictionary<CharClass, Dictionary<Stat, float[]>>();

            // Membangun lookUpTable berdasarkan charClasses.
            foreach (ProgressionCharClass progressionClass in charClasses)
            {
                // Inisialisasi Dictionary untuk menyimpan data statistik per kelas karakter.
                var statLookUpTable = new Dictionary<Stat, float[]>();

                // Menambahkan data statistik ke dalam statLookUpTable.
                foreach (ProgressionStat progressionStat in progressionClass.stat)
                {
                    // Menyimpan array levels progresi statistik dalam statLookUpTable.
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                // Menambahkan data statistik per kelas karakter ke dalam lookUpTable.
                lookUpTable[progressionClass.charClass] = statLookUpTable;
            }
        }

        //untuk menserialize dan menampilkan progression class di inspector Unity.
        [System.Serializable]
        class ProgressionCharClass
        {
            //Variabel untuk menyimpan informasi kelas karakter.
            public CharClass charClass;
            //Array yang menyimpan data progresi statistik untuk kelas karakter.
            public ProgressionStat[] stat;
            
        }

        [System.Serializable]
        class ProgressionStat
        {
            //Variabel yang menyimpan informasi tentang stats.
            public Stat stat;
            //Array yang menyimpan data progresi level untuk statistik tertentu.
            public float[] levels;
        }
    }
}