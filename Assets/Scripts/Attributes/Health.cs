using Core;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = 100f;

        private bool _isDead;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private float _maxHealth;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
            _maxHealth = healthPoints;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print($"{gameObject.name} receives {damage} damage! {healthPoints} health left!");
            if (healthPoints == 0) Die();
        }

        public float GetPercentage()
        {
            return healthPoints / _maxHealth * 100;
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;
            _animator.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            if (healthPoints <= 0) Die();
        }
    }
}