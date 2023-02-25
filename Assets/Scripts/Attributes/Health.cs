using Core;
using Saving;
using Stats;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private float _healthPoints;
        private bool _isDead;
        private float _maxHealth;
        private float _xpReward;

        private Animator _animator;
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            var stats = GetComponent<BaseStats>();
            _healthPoints = stats.GetHealth();
            _maxHealth = _healthPoints;
            _xpReward = stats.GetExperienceReward();
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if (_healthPoints != 0) return;
            Die();
            var instigatorExp = instigator.GetComponent<Experience>();
            if (!instigatorExp) return;
            instigatorExp.GainExperience(_xpReward);
        }

        public float GetPercentage()
        {
            return _healthPoints / _maxHealth * 100;
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
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float) state;
            if (_healthPoints <= 0) Die();
        }
    }
}