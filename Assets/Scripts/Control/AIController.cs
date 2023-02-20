using Attributes;
using Combat;
using Core;
using Movement;
using UnityEngine;

namespace Control
{
    [RequireComponent(typeof(Fighter), typeof(Health))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float dwellingTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField, Range(0,1)] private float patrolSpeedFraction = 0.2f;
        

        private GameObject _player;
        private Health _health;
        private Fighter _fighter;
        private Mover _mover;
        private ActionScheduler _actionScheduler;

        private Vector3 _guardPosition;
        private float _timeSinceLastSawPLayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int _currentWaypointIndex = 0;
        private bool _skipWaypoint;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if (Fighter.CanAttack(_player) && InAttackRange())
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPLayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceLastSawPLayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = _guardPosition;
            if (patrolPath)
            {
                if (AtWayPoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                
                nextPosition = GetCurrentWaypoint();
            }

            if (patrolPath && patrolPath.skipWaypoints.Length > 0)
            {
                if (_skipWaypoint)
                {
                    _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
                }
                else
                    DwellAndMove(nextPosition);
            }
            else 
                DwellAndMove(nextPosition);
        }

        private void DwellAndMove(Vector3 nextPosition)
        {
            if (_timeSinceArrivedAtWaypoint > dwellingTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            if (patrolPath && patrolPath.skipWaypoints.Length > 0)
            {
                _skipWaypoint = patrolPath.skipWaypoints[_currentWaypointIndex];
            }
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            var distance = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distance <= waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPLayer = 0;
            _fighter.Attack(_player);
        }

        private bool InAttackRange()
        {
            var distance = Vector3.Distance(transform.position, _player.transform.position);
            return distance <= chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}