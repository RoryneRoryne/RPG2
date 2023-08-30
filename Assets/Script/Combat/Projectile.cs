using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject effectWhenHit = null;
        [SerializeField] float maxLifeTime = 5f;
        // [SerializeField] GameObject[] destroyOnCollision = null;
        Health target = null;
        GameObject instigator = null;
        float damage = 0f;

        private void Start() 
        {
            //membuat object mengarah pada target dan tidak update perframe.
            transform.LookAt(GetAimLocation());
        }
        // Update is called once per frame
        void Update()
        {
            //untuk menjaga kode agar jika target = null, akan langsung return.
            if (target == null)
            {
                return;
            }

            if (isHoming && !target.IsDead())
            {
               //membuat object mengarah pada target
                transform.LookAt(GetAimLocation()); 
            }
            transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }

        //untuk menentukan target dengan damage
        public void SetTarget(Health target,GameObject instigator, float damage)
        {
            this.target  = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            // Dapatkan komponen CapsuleCollider yang terpasang pada GameObject target
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            // Jika target tidak memiliki komponen CapsuleCollider, kembalikan posisinya secara langsung
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            // Hitung lokasi aim dengan menambahkan offset pada posisi target
            // Offset ditentukan oleh setengah tinggi target di sepanjang sumbu Y
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other) 
        {
            //other akan mengecek jika Health tidak sama dengan targer maka return
            if (other.GetComponent<Health>() != target)
            {
                return;
            }
            if (target.IsDead())
            {
                return;
            }
            //target akan memanggil take damage.
            target.TakeDamage(instigator, damage);

            if (effectWhenHit != null)
            {
                Instantiate(effectWhenHit, GetAimLocation(), transform.rotation);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}

