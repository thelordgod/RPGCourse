using Attributes;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject[] destroyOnCollision = null;
        [SerializeField, Min(0f)] private float lifeAfterImpact;

        private Health _target;
        private float _damage;
        private float _targetHeight;
        private string _shooterTag;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, 60f / speed);
        }

        private void Update()
        {
            if (!_target) return;
            if (isHoming && !_target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        }

        public void SetTarget(Health target, float damage, string shooterTag, float targetHeight = 1f)
        {
            _damage = damage;
            _target = target;
            _shooterTag = shooterTag;
        }

        private Vector3 GetAimLocation()
        {
            return _target.transform.position + Vector3.up * (_targetHeight + 0.6f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_shooterTag)) return;
            var colliderHealth = other.GetComponent<Health>();
            if (colliderHealth == null) return;
            if (colliderHealth.IsDead()) return;
            if (!colliderHealth.IsDead()) colliderHealth.TakeDamage(_damage);
            speed = 0;
            if (hitEffect)
            {
                var projectileTransform = transform;
                var currentHitEffect = Instantiate(hitEffect, projectileTransform.position, projectileTransform.rotation);
                Destroy(currentHitEffect, 1f);
            }
            
            foreach (var toDestroy in destroyOnCollision)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}