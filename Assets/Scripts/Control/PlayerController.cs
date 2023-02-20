using Attributes;
using Combat;
using Movement;
using UnityEngine;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        private Mover _mover;
        private Fighter _fighter;
        private Health _health;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (!target || !Fighter.CanAttack(target.gameObject)) continue;
                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            var ray = GetMouseRay();
            var hasHit = Physics.Raycast(ray, out var hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private Ray GetMouseRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}