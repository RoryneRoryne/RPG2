using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapons : ScriptableObject 
    {
        //AnimatorOverrideController untuk mengganti animator controller dengan animasi lain
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefabb = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isRightHand = true;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        const string weaponName = "Weapons";
        
        

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyCurrentWeapon(rightHand, leftHand);
            //if berfungsi untuk menghindari error null pada weapPrefab
            if (equippedPrefabb != null)
            {
                Transform handTransform = HandsTransform(rightHand, leftHand);
                //untuk memunculkan weap prefab pada handtransform.
                GameObject weapons = Instantiate(equippedPrefabb, handTransform);
                weapons.name = weaponName;
            }

            //mengkonversi runtimeAnimatorController objek 'animator' menjadi AnimatorOverrideController dan menetapkannya ke variabel 'overrideController'.
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            //if berfungsi untuk menghindari error null pada animator override.
            if (animatorOverride != null)
            {
                //untuk mengganti animasi yang sudah terdapat pada animator controller.
                animator.runtimeAnimatorController = animatorOverride;
            }
            //Jika animatorOverride tidak diberikan, AnimatorOverrideController akan memakai root animator.
            else if (overrideController != null)
            {
                // Mengatur runtimeAnimatorController dari overrideController sebagai runtimeAnimatorController baru.
                // agar menggunakan AnimatorOverrideController pada root animator.
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; 
            }
        }

        private void DestroyCurrentWeapon(Transform rightHand, Transform leftHand)
        {
            Transform currentWeapons = rightHand.Find(weaponName);
            if (currentWeapons == null)
            {
                currentWeapons = leftHand.Find(weaponName);
            }
            if (currentWeapons == null)
            {
                return;
            }
            currentWeapons.name = "OldWeapon";
            Destroy(currentWeapons.gameObject);
        }

        private Transform HandsTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            //jika isRightHand = true, handtransform akan berada pada tangan kanan
            if (isRightHand)
            {
                handTransform = rightHand;
            }
            //jika tidak, tangan kiri.
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileSpawn = Instantiate(projectile, HandsTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileSpawn.SetTarget(target, instigator, weaponDamage);
        }

        public float GetWeapDamage()
        {
            return weaponDamage;
        }
        public float GetWeapRange()
        {
            return weaponRange;
        }
    }
}