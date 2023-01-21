using UnityEngine;

namespace Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] public bool[] skipWaypoints;
        
        private const float WaypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.color = lineColor;
                Gizmos.DrawSphere(GetWaypoint(i), WaypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            return i == transform.childCount - 1 ? 0 : i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
