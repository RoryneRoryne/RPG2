using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapons currentWeapons = null;
        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Weapons defaultWeapons = null;
        [SerializeField] Transform rightHandTransfrom = null;
        [SerializeField] Transform leftHandTransfrom = null;
        

        private void Start() 
        {
            if (currentWeapons == null)
            {
                EquipWeapon(defaultWeapons);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        //untuk memunculkan weapon
        public void EquipWeapon(Weapons weapons)
        {
            currentWeapons = weapons;
            Animator animator = GetComponent<Animator>();
            weapons.Spawn(rightHandTransfrom, leftHandTransfrom, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // ini akan trigger event Hit()
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //event animasi
        void Hit()
        {
            //alasan kenapa script untuk menerima damage berada dalam Hit
            //agar musuh akan terkena damage saat animasi menyerang menyentuh target
            if (target == null || target.IsDead())
            {
                return;
            }
             if (currentWeapons.HasProjectile())
            {
                currentWeapons.LaunchProjectile(rightHandTransfrom, leftHandTransfrom, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject,currentWeapons.GetWeapDamage());
            }   
        }

        void Shoot()
        {
            Hit();
        }
        
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapons.GetWeapRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToAttack = combatTarget.GetComponent<Health>();
            return targetToAttack != null && !targetToAttack.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            //animator yang ini untuk menghentikan animasi memukul saat berhenti mengincar target
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger(name: "stopAttack");
        }

        public object CaptureState()
        {
            return currentWeapons.name;
        }

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