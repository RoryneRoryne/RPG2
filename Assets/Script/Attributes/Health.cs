using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        // [SerializeField] float regenPercentage = 65;
        // float deathTimer = 3f;
        float healthPoints = -1f;
        bool isDead = false;   

        //                                          Start
        // ########################################################################################
        private void Start() 
        {
            //untuk mengecek jika hp < 0. Berguna agar hp tidak bisa kurang dari 0.
            if (healthPoints < 0)
            {
                //maka akan mengambil nilai maksimum hp dari BaseStats
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            } 
        }

        //                           Untuk mensubsribe suatu metode dalam event
        // ########################################################################################
        //metode unity yang berguna untuk mensubscribe suatu event
        private void OnEnable() 
        {
            //untuk mensubsribe event onLevelUp yang berada pada BaseStats yang akan memanggil metode RegenHealth.
            GetComponent<BaseStats>().onLevelUp += RegenHealth;
        }

        //                           Untuk unsubsribe suatu metode dalam event
        // ########################################################################################
        //metode unity yang berguna untuk unnsubscribe suatu event
        private void OnDisable() 
        {
            //untuk unsubscribe event onLevelUp yang berada pada BaseStats yang tidak akan memanggil -
            //metode RegenHealth.
            GetComponent<BaseStats>().onLevelUp -= RegenHealth;
        }

        //                                 Mengecek apakah objek mati
        // ########################################################################################
        public bool IsDead()
        {
            //mengembailkan nilai variabel isDead.
            return isDead;
        }

        //                                 Menerima Damage
        // ########################################################################################
        //metode ini menerima dua parameter yaitu instigator (penyerang) dan damage.
        public void TakeDamage(GameObject instigator, float damage)
        {
            //untuk menampilkan pesan yan berisi objek yang menerima damage dan damage yang diterima.
            Debug.Log(gameObject.name + "took damage" + damage);

            //berguna agar value dari health tidak akan berkurang dari 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            //untuk menampilkan pesan yan berisi sisa healthpoints.
            Debug.Log("Hp left" + healthPoints);

            //jika healthpoints sama dengan 0
            if (healthPoints == 0f)
            {
                //target akan mati
                Die();

                //penyerang mendapat exp.
                ExperienceGain(instigator);
            }
        }

        //                          Mengambil healthPoints
        // ########################################################################################
        public float GetHealthPoints()
        {
            //untuk mengembalikan nilai healthPoints.
            return healthPoints;
        }

        //                          Mengambil maksimum healthPoints
        // ########################################################################################
        public float GetMaxHealthPoints()
        {
            //mengambil poin kesehatan maksimum dari BaseStats.
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        //                          Mengambil persentase Hp
        // ########################################################################################
        public float GetPercentage()
        {
            // Menghitung persentase health objek.
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        //                          Mengatur objek yang mati
        // ########################################################################################
        private void Die()
        {
            //memeriksa apakah isDead, jika true akan return tanpa melakukan apapun.
            if (isDead) return;

            //untuk membuat variabel isDead menjadi true.
            isDead = true;

            //menjalankan animasi "die"
            GetComponent<Animator>().SetTrigger("die");

            //untuk mengentikan kegiatan objek.
            GetComponent<ActionScheduler>().CancelAction();

            //untuk menghancurkan objek jika sudah mati tergantung dari deathTimer.
            // Destroy(gameObject, deathTimer);
        }

        //                                    Menambah Xp
        // ######################################################################################################
        private void ExperienceGain(GameObject instigator)
        {
            // Mendapatkan komponen Experience dari GameObject instigator.
            Experience exp = instigator.GetComponent<Experience>();
            // Memeriksa jika objek memiliki komponen exp. Jika tidak, maka akan return tanpa melakukan apapun.
            if (exp == null)
            {
                return;
            }
            // J ika objek memiliki komponen exp, maka akan Memanggil metode GainExp() dari komponen Experience
            exp.GainExp(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        //                                             RegenHealth
        // ######################################################################################################
        //untuk meregen health kembali ke max.
        private void RegenHealth()
        {
            //untuk mengembalikan Hp ke nilai maksimum.
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        //                                             Save
        // ######################################################################################################
        //untuk mensave state healthpoints
        public object CaptureState()
        {
            //akan mengCapture(Menyimpan) nilai Hp saat ini.
            return healthPoints;
        }

        //                                             Load
        // ######################################################################################################
        //restore state dari state yang telah tersave
        public void RestoreState(object state)
        {
            //memuat nilai Hp dari CaptureState
            healthPoints = (float) state;

            //untuk mengecek jika hp 0 maka objek akan mati
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}