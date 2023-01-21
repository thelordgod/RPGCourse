using Core;
using Movement;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Weapon defaultWeapon;

        private Health _target;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private Weapon _currentWeapon;
        private float _height;

        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int StopAttackTrigger = Animator.StringToHash("stopAttack");
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");


        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (!_target) return;
            if (_target.IsDead())
            {
                Cancel();
                return;
            }

            if (_target && !GetIsInRange())
                _mover.MoveTo(_target.transform.position, 0.8f);
            else
                AttackBehaviour();
        }

        public static bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget)
            {
                return false;
            }

            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            if (combatTarget.CompareTag(tag))
            {
                return;
            }

            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (!weapon) return;
            _animator.SetFloat(AttackSpeed, 1f / weapon.GetTimeBetweenAttacks());
            _currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > _currentWeapon.GetTimeBetweenAttacks())
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _mover.Cancel();
            _animator.ResetTrigger(StopAttackTrigger);
            _animator.SetTrigger(AttackTrigger);
        }

        // Animation Event
        // ReSharper disable once MemberCanBePrivate.Global
        public void Hit()
        {
            if (!_target) return;
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, tag, _height);
            }
            else
            {
                _target.TakeDamage(_currentWeapon.GetDamage());
            }
        }

        //Animation Event
        public void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(_target.transform.position, _mover.transform.position) < _currentWeapon.GetRange();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(AttackTrigger);
            _animator.SetTrigger(StopAttackTrigger);
        }
    }
}