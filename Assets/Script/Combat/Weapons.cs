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
        [SerializeField] float percentageBonus = 0;
        [SerializeField] float weaponRange = 2f;
        const string weaponName = "Weapons";
        
        
        //                          Memunculkan weapon pada tangan objek yang memiliki script
        // ######################################################################################## 
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            //untuk menghancurkan senjata lama jika player memiliki senjata baru
            DestroyCurrentWeapon(rightHand, leftHand);
            //if berfungsi untuk menghindari error null pada weapPrefab
            if (equippedPrefabb != null)
            {
                //untuk memastikan di mana senjata akan dipasang.
                Transform handTransform = HandsTransform(rightHand, leftHand);
                
                //untuk memunculkan weap prefab pada handtransform.
                GameObject weapons = Instantiate(equippedPrefabb, handTransform);
                
                //nama weapon
                weapons.name = weaponName;
            }

            //mengkonversi runtimeAnimatorController objek 'animator' menjadi AnimatorOverrideController dan - 
            //menetapkannya ke variabel 'overrideController'. Berguna untuk mengganti animasi.
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

        //                          Menghancurkan weapon lama
        // ######################################################################################## 
        private void DestroyCurrentWeapon(Transform rightHand, Transform leftHand)
        {
            //untuk mencari weapon yang sedang dipakai
            Transform currentWeapons = rightHand.Find(weaponName);

            //untuk memeriksa jika tidak ada weapon pada tangan kanan.
            if (currentWeapons == null)
            {
                //akan memeriksa tangan kiri
                currentWeapons = leftHand.Find(weaponName);
            }

            ////untuk memeriksa jika tidak ada weapon sama sekali.
            if (currentWeapons == null)
            {
                //akan return tanpa melakukan apapun
                return;
            }

            //mengubah nama currentWeapon yang ditemukan menjadi OldWeapon
            currentWeapons.name = "OldWeapon";

            //menghancurkan currentWeapon
            Destroy(currentWeapons.gameObject);
        }

        //                       Menentukan tangan mana yang akan memegang senjata
        // ######################################################################################## 
        private Transform HandsTransform(Transform rightHand, Transform leftHand)
        {
            //lokal varibel
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

            //returh variabel handTransform.
            return handTransform;
        }

        //                          Memeriksa projectile
        // ######################################################################################## 
        public bool HasProjectile()
        {
            //untuk memeriksa apakah ada projectile atau tidak, jika ada akan mengembalikan nilai true
            return projectile != null;
        }

        //                          Meluncurkan projectile
        // ######################################################################################## 
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            //menginisialisasi prefab projectile
            //Quaternion.identity mengatur rotasi dari projectile
            Projectile projectileSpawn = Instantiate(projectile, HandsTransform(rightHand, leftHand).position, Quaternion.identity);
            //mengatur projectile untuk mengejar target
            projectileSpawn.SetTarget(target, instigator, calculatedDamage);
        }

        //                          Mengambil damage weapon
        // ######################################################################################## 
        public float GetWeapDamage()
        {
            //untuk mengembailkan nilai weaponDamage
            return weaponDamage;
        }

        //                          Mengambil bonus persen damage
        // ######################################################################################## 
        public float GetPercentageBonus()
        {
            //untuk mengembailkan nilai percentageBonus
            return percentageBonus;
        }

        //                          Mengambil jarak weapon
        // ######################################################################################## 
        public float GetWeapRange()
        {
            //untuk mengembailkan nilai weaponRange
            return weaponRange;
        }
    }
}