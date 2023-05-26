using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        Health target = null;
        float damage = 0f;
        
        // Update is called once per frame
        void Update()
        {
            //untuk menjaga kode agar jika target = null, akan langsung return.
            if (target == null)
            {
                return;
            }
            //membuat object mengarah pada target
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        //untuk menentukan target dengan damage
        public void SetTarget(Health target, float damage)
        {
            this.target  = target;
            this.damage = damage;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            //other akan mengecek jika Health tidak sama dengan targer maka return
            if (other.GetComponent<Health>() != target)
            {
                return;
            }
            //target akan memanggil take damage.
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

