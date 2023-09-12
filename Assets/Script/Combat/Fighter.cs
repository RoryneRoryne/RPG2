using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapons currentWeapons = null;
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Weapons defaultWeapons = null;
        [SerializeField] Transform rightHandTransfrom = null;
        [SerializeField] Transform leftHandTransfrom = null;
        
        //                              Start
        // ########################################################################################
        private void Start() 
        {
            if (currentWeapons == null)
            {
                EquipWeapon(defaultWeapons);
            }
        }

        //                              Update
        // ########################################################################################
        private void Update()
        {
            //agak timeSinceLastAttack memakai waktu dalam waktu detik bukan perframe.
            timeSinceLastAttack += Time.deltaTime;

            //jika target null (tidak memiliki target) akan langsung return (berhenti menyerang).
            if (target == null) 
            {
                return;
            }

            //jika target mati, maka akan langsung return (berhenti menyerang).
            if (target.IsDead()) 
            {
                return;
            }

            //jika objek tidak berada dalam jarak serang.
            if (!GetIsInRange())
            {
                //akan mendekati objek yang ingin diserang
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }

            else
            {
                //jika objek yang ingin diserang berada dalam jarak, maka akan berhenti berjalan.
                GetComponent<Mover>().Cancel();
                //dan memanggil AttackBehavior untuk menyerang.
                AttackBehavior();
            }
        }

        //                              Memakai Senjata
        // ########################################################################################
        public void EquipWeapon(Weapons weapons)
        {
            //weapons yang diangkat akan menjadi currentWeapons.
            currentWeapons = weapons;
            //untuk memanggil animator.
            Animator animator = GetComponent<Animator>();
            //untuk menentukan weapons yang diangkat akan berada di tangan kanan atau kiri dan memanggil animator - 
            //berdasarkan senjata yang telah diambil.
            weapons.Spawn(rightHandTransfrom, leftHandTransfrom, animator);
        }

        //                              Menentukan Target
        // ########################################################################################
        //GetTarget akan mengambil parameter dari Health.
        public Health GetTarget()
        {
            //objek yang memiliki health akan return dan menjadi target.
            return target;
        }

        //                              Perilaku Saat Menyerang
        // ########################################################################################
        private void AttackBehavior()
        {
            //agark objek akan melihat ke arah target yang akan diserang.
            transform.LookAt(target.transform);
            //untuk menentukan waktu menyerang, waktu menyerang akan ditentukan jika waktu serangan terakhir -
            //sudah melewati batas cooldown attack yang telah ditentukan.
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //memanggil TriggerAttack untuk menyerang.
                TriggerAttack();
                //mengembalikan 
                timeSinceLastAttack = 0;
            }
        }

        //                              Untuk Mengendalikan Animasi Attack
        // ########################################################################################
        private void TriggerAttack()
        {
            //untuk menghentikan animasi attack
            GetComponent<Animator>().ResetTrigger("stopAttack");

            //untuk mengaktifkan animasi attack
            GetComponent<Animator>().SetTrigger("attack");
        }
        
        //                              Mengatur Logika Saat Menyerang
        // ########################################################################################
        void Hit()
        {
            //alasan kenapa script untuk menerima damage berada dalam Hit
            //agar musuh akan terkena damage saat animasi menyerang menyentuh target

            //untuk memeriksa jika tidak ada target atau target mati, akan return tanpa melakukan apa - apa.
            if (target == null || target.IsDead())
            {
                return;
            }

            //untuk memanggil fungsi damagee dari basestats.
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            //memeriksa jika objek yang memiliki script ini memiliki projectile atau tidak.
             if (currentWeapons.HasProjectile())
            {
                //jika iya, maka akan memanggil metode LaunchProjectile.
                currentWeapons.LaunchProjectile(rightHandTransfrom, leftHandTransfrom, target, gameObject, damage);
            }

            //jika tidak
            else
            {
                //akan memanggil metode TakeDamage
                target.TakeDamage(gameObject, damage);
            }   
        }
        
        //                                      Shoot
        // ########################################################################################
        void Shoot()
        {
            //memanggil Hit
            Hit();
        }
        
        //                             Menghitung jarak serangan dengan target
        // ########################################################################################
        private bool GetIsInRange()
        {
            //untuk menentukan apakah target berada dalam jangkauan dengan menghitung jarak antara target dengan -
            //penyerang lebih kecil dari jarak serangan senjata yang berada dalam GetWeapRange.
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapons.GetWeapRange();
        }

        //                            Memerika apakah target dapat diserang
        // ########################################################################################
        //metode ini mengembalikan parameter GameObject yang memiliki combatTarget
        public bool CanAttack(GameObject combatTarget)
        {
            //jika combatTarget null, akan return false yang membuat tidak dapat diserang.
            if (combatTarget == null)
            {
                return false;
            }

            //untuk mengambil komponen health yang terdapat pada combat.
            Health targetToAttack = combatTarget.GetComponent<Health>();
            //untuk memeriksa jika target masih memiliki health dan belum mati.
            return targetToAttack != null && !targetToAttack.IsDead();
        }

        //                                     Untuk Menyerang
        // ######################################################################################## 
        //metode ini mengembalikan parameter GameObject yang memiliki combatTarget.
        public void Attack(GameObject combatTarget)
        {
            //untuk memulai menyerang dengan memanggil metode StartAction yang berada dalam ActionScheduler.
            GetComponent<ActionScheduler>().StartAction(this);

            //untuk mengambil komponen Health dari combatTarget untuk ditetapkan dalam variabel target.
            target = combatTarget.GetComponent<Health>();
        }

        //                                     Untuk Membatalkan serangan
        // ######################################################################################## 
        public void Cancel()
        {
            //animator yang ini untuk menghentikan animasi memukul saat berhenti mengincar target
            StopAttack();
            //membuat target menjadi null agar serangan berhenti.
            target = null;
        }

        //                             Untuk Menghentikan animasi menyerang.
        // ######################################################################################## 
        private void StopAttack()
        {
            //untuk mereset trigger "attack" yang berada dalam animator.
            GetComponent<Animator>().ResetTrigger("attack");
            //untuk memanggil trigger "stopAttack" yang berada dalam animator.
            GetComponent<Animator>().SetTrigger(name: "stopAttack");
        }

        //                          Menambahkan Damage Senjata Pada Player
        // ######################################################################################## 
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            //untuk memeriksa damage player yang terdapat pada stat
            if (stat == Stat.Damage)
            {
                //untuk megembailkan damage senjata dari senjata yang sedang dipakai.
                yield return currentWeapons.GetWeapDamage();
            }
        }
        
        //                          Menambahkan Bonus Persen Damage Pada Player
        // ########################################################################################
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            //untuk memeriksa damage player yang terdapat pada stat
            if (stat == Stat.Damage)
            {
                //untuk megembailkan bonus persen dari senjata yang sedang dipakai.
                yield return currentWeapons.GetPercentageBonus();
            }
        }

        //                                       Mensave state 
        // ########################################################################################
        public object CaptureState()
        {
            //mengembalikan nama senjata yang sedang digunakan.
            return currentWeapons.name;
        }

        //                                       Mereload state 
        // ########################################################################################
        public void RestoreState(object state)
        {
            // Mengonversi state menjadi string dan menetapkannya ke weaponsName.
            string weaponsName = (string) state;
            // Memuat objek senjata dari folder Resources menggunakan weaponsName.
            Weapons weapons = Resources.Load<Weapons>(weaponsName);
            //equip senjata.
            EquipWeapon(weapons);
        }
    }

}