using Core;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject equippedPrefab;
        [SerializeField, Min(0)] private float range = 2f;
        [SerializeField, Min(0)] private float timeBetweenAttacks = 1f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile;
        [SerializeField, Min(1)] private int projectiles;

        private const string WeaponName = "Weapon";
        private string _weaponUser;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab)
            {
                var weapon = Instantiate(equippedPrefab, GetTransform(rightHand, leftHand));
                weapon.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private static void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null) oldWeapon = leftHand.Find(WeaponName);
            if (oldWeapon == null) return;
            oldWeapon.name = "Destroyed";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, string shooterTag,
            float targetHeight)
        {
            var offsetAngleStep = Mathf.PI * 2 / projectiles;
            var originHand = GetTransform(rightHand, leftHand);
            var scale = 2f;
            for (var i = 1; i <= projectiles; i++)
            {
                var offset = new Vector3();
                if (projectiles > 1)
                {
                    offset = new Vector3(Mathf.Sin(i * offsetAngleStep), 0, Mathf.Cos(i * offsetAngleStep));
                    offset *= scale;
                }

                var projectilePosition = originHand.position + offset;
                var projectileInstance = Instantiate(projectile, projectilePosition, Quaternion.identity);
                projectileInstance.SetTarget(target, damage, shooterTag, targetHeight);
            }
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }

        public float GetRange()
        {
            return range;
        }
    }
}