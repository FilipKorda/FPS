using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    public Transform[] segments;
    public Transform[] waypoints;
    public float moveSpeed = 8f;
    public float followDistance = 0.5f;
    public float rotationSpeed = 5f;

    private List<Vector3> positions = new List<Vector3>();
    private int currentWaypointIndex = 0;

    public bool canMove = false;

    void Start()
    {
        positions.Add(segments[0].position);
    }

    void Update()
    {
        if (canMove)
        {
            MoveHeadToWaypoint();
            MoveSegments();
        }
    }

    void MoveHeadToWaypoint()
    {
        if (waypoints.Length == 0) return;

        Vector3 target = waypoints[currentWaypointIndex].position;
        Vector3 direction = (target - segments[0].position).normalized;

        segments[0].position = Vector3.MoveTowards(
            segments[0].position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            segments[0].rotation = Quaternion.Slerp(
                segments[0].rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(segments[0].position, target) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        if (Vector3.Distance(positions[0], segments[0].position) > followDistance)
        {
            positions.Insert(0, segments[0].position);
        }
    }

    void MoveSegments()
    {
        for (int i = 1; i < segments.Length; i++)
        {
            int index = Mathf.Min(i * 2, positions.Count - 1);
            Vector3 targetPos = positions[index];
            Vector3 direction = targetPos - segments[i].position;

            segments[i].position = Vector3.MoveTowards(
                segments[i].position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                segments[i].rotation = Quaternion.Slerp(
                    segments[i].rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        if (positions.Count > segments.Length * 10)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        if (waypoints[0] != null && waypoints[waypoints.Length - 1] != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        if (positions != null && positions.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                Gizmos.DrawLine(positions[i], positions[i + 1]);
            }
        }
    }
}
