using RPG.Core;
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
        
        

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handTransform = HandsTransform(rightHand, leftHand);
            //if berfungsi untuk menghindari error null pada weapPrefab
            if (equippedPrefabb != null)
            {
                //untuk memunculkan weap prefab pada handtransform.
                Instantiate(equippedPrefabb, handTransform);
            }
            //if berfungsi untuk menghindari error null pada animator override.
            if (animatorOverride != null)
            {
                //untuk mengganti animasi yang sudah terdapat pada animator controller.
                animator.runtimeAnimatorController = animatorOverride;
            }
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileSpawn = Instantiate(projectile, HandsTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileSpawn.SetTarget(target, weaponDamage);
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