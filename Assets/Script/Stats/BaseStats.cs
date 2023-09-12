using System;
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
        [SerializeField] GameObject levelUpParticleFx = null;
        [SerializeField] bool useModifier = false;
        public event Action onLevelUp;

        int currentLevel = 0;

        Experience experience;

        //                                      Awake
        // ########################################################################################
        private void Awake() 
        {
            //membuat local variabel bernama experience yang direferensi dari script Experience
            experience = GetComponent<Experience>(); 
        }

        //                                      Start
        // ########################################################################################
        private void Start() 
        {
            //untuk memasukkan hasil dari CaculateLevel ke variabel currentLevel
            currentLevel = CalculateLevel();
        }

        //                           Untuk mensubsribe suatu metode dalam event
        // ########################################################################################
        private void OnEnable() 
        {
            //Mengecek apakah objek memiliki komponen experience
            if (experience != null)
            {
                //jika memiliki komponen experience, maka akan mensubsrcibe event onExpGained yang -
                //pada experience dan akan memanggil method UpdateLevel
                experience.onExpGained += UpdateLevel;
            }
        }

        //                           Untuk unnsubsribe suatu metode dalam event
        // ########################################################################################
        private void OnDisable() 
        {
            //Mengecek apakah objek memiliki komponen experience
            if (experience != null)
            {
                //jika memiliki komponen experience, maka akan unnsubsrcibe event onExpGained yang -
                //pada experience dan akan menghapus method UpdateLevel
                experience.onExpGained -= UpdateLevel;
            }
        }

        //                              Update level player
        // ######################################################################################## 
        private void UpdateLevel() 
        {
            // hasil dari level baru berasal dari calculate level
            int newLevel = CalculateLevel();

            // untuk mengubah level saat ini dengan level baru
            if (newLevel > currentLevel)
            {
                // level saat ini akan menjadi level baru
                currentLevel = newLevel;

                //efek saat level naik
                LevelUpEffect();

                //memanggil event onLevelUp
                onLevelUp();
            }
        }

        //                              Efek Saat Naik Level
        // ########################################################################################
        private void LevelUpEffect()
        {
            //memuncul fx pada player
            Instantiate(levelUpParticleFx, transform);
        }

        //                              Mengambil Stat yang telah dimodify.
        // ########################################################################################
        public float GetStat(Stat stat)
        {
            //Menghitung nilai damage akhir.
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        //                              Mengambil Stats awal.
        // ########################################################################################
        private float GetBaseStat(Stat stat)
        {
            //Memanggil metode 'GetStat' dari objek 'progression' untuk mengambil nilai 'stat' yang diinginkan berdasarkan level karakter.
            return progression.GetStat(stat, charClass, GetLevel());
        }

        //                              Mengambil Level
        // ########################################################################################
        public int GetLevel()
        {

            //agar disaat terdapat bug level -1 maka akan segerea memanggil calculate level
            if (currentLevel < 1)
            {
                //membuat hasil currentLevel berdasarkan CalculateLevel
                currentLevel = CalculateLevel();
            }
            //Jika currentLevel sudah lebih besar dari atau sama dengan 1 maka akan langsung return tanpa menjalankan if.
            return currentLevel;
        }
        
        //                              Modify weap damage
        // ########################################################################################
        private float GetAdditiveModifier(Stat stat)
        {
            //untuk memeriksa useModifier false (atau tidak menggunakan useModifier).
            if (!useModifier)
            {
                //jika tidak maka modifier tidak akan diterapkan.
                return 0;
            }

            // Inisialisasi variabel total dengan nilai awal 0. Ini akan digunakan untuk menjumlahkan semua modifikasi.
            float total = 0;

            // Loop melalui semua komponen yang mengimplementasikan interface IModifierProvider pada objek ini.
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                // Panggil metode GetAdditiveModifier(stat) pada objek provider. Metode ini mengembalikan daftar modifiier (float).
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    // Tambahkan setiap modifikasi ke nilai total.
                    total += modifier;
                }
            }
            // Kembalikan nilai total jika tidak memanggil foreach.
            return total;
        }

        //                              Modify Bonus Persen Damage
        // ########################################################################################
        private float GetPercentageModifier(Stat stat)
        {
            //untuk memeriksa useModifier false (atau tidak menggunakan useModifier).
            if (!useModifier)
            {
                //jika tidak maka modifier tidak akan diterapkan.
                return 0;
            }

            // Inisialisasi variabel total dengan nilai awal 0. Ini akan digunakan untuk menjumlahkan semua modifikasi.
            float total = 0;

            // Loop melalui semua komponen yang mengimplementasikan interface IModifierProvider pada objek ini.
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                // Panggil metode GetPercentageModifiers(stat) pada objek provider. Metode ini mengembalikan daftar modifiier (float).
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    // Tambahkan setiap modifikasi ke nilai total.
                    total += modifier;
                }
            }
            // Kembalikan nilai total jika tidak memanggil foreach.
            return total;
        }


        //                              Mengkalkulasi Level
        // ########################################################################################
        private int CalculateLevel()
        {
            // Menginisialisasi variabel lokal experience dengan cara mendapatkan komponen Experience.
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