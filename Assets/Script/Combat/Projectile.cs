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

        public void SetTarget(Health target)
        {
            this.target  = target;
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
    }
}

