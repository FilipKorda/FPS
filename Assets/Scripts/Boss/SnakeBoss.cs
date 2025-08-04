using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    [SerializeField] private enum BossState { Idle, Patrol, Attack }
    [SerializeField] private BossState currentState = BossState.Idle;
    [SerializeField] private BossRaycastHit bossRaycastHit;

    [SerializeField] private Transform[] segments;
    [SerializeField] private Transform[] wallWaypoints;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float followDistance = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;

    private List<Vector3> positions = new List<Vector3>();

    [Header("Attack Paths")]
    [SerializeField] private Transform[] attackWaypointsPath;
    [SerializeField] private Transform[] secondAttackWaypointsPath;
    [SerializeField] private Transform[] thirdAttackWaypointsPath;
    [SerializeField] private Transform[] fourthAttackWaypointsPath;
    [SerializeField] private Transform[] fifthAttackWaypointsPath;
    [SerializeField] private Transform[] sixthAttackWaypointsPath;
    [SerializeField] private Transform[] seventhAttackWaypointsPath;
    [SerializeField] private Transform[] eighthAttackWaypointsPath;

    private Transform[][] attackPaths;
    private Transform[] currentAttackPath;
    private int patrolIndex = 0;
    private int attackIndex = 0;

    [SerializeField] private float moveAttackSpeed = 27f;
    [SerializeField] private float followAttackDistance = 1f;
    [SerializeField] private float rotationAttackSpeed = 8f;
    [SerializeField] private float attackPathWaitTime = 2f;

    public bool canMove = false;
    private bool waitingForNextAttack = false;
    private float waitTimer = 0f;

    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform[] allAttackPathMainTransform;
    private Transform currentAttackRoot;


  

    void Start()
    {
        positions.Add(segments[0].position);

        attackPaths = new Transform[][]
        {
            attackWaypointsPath,
            secondAttackWaypointsPath,
            thirdAttackWaypointsPath,
            fourthAttackWaypointsPath,
            fifthAttackWaypointsPath,
            sixthAttackWaypointsPath,
            seventhAttackWaypointsPath,
            eighthAttackWaypointsPath
        };
    }

    void Update()
    {     
        if (!canMove) return;

        switch (currentState)
        {
            case BossState.Idle:
                bossRaycastHit.shouldUseRaycast = false;
                break;

            case BossState.Patrol:
                bossRaycastHit.shouldUseRaycast = false;
                MoveHeadToWaypoint(wallWaypoints, ref patrolIndex, moveSpeed, followDistance, rotationSpeed, loop: true);
                MoveSegments(moveSpeed, followDistance, rotationSpeed);
                break;

            case BossState.Attack:
                bossRaycastHit.shouldUseRaycast = true;

                if (currentAttackPath == null)
                {
                    PickRandomAttackPath();
                }

                if (waitingForNextAttack)
                {
                    waitTimer -= Time.deltaTime;

                    if (currentAttackRoot != null && playerPosition != null)
                    {
                        Vector3 targetXZ = new Vector3(playerPosition.position.x, currentAttackRoot.position.y, playerPosition.position.z);
                        currentAttackRoot.position = targetXZ;
                    }

                    if (waitTimer <= 0f)
                    {
                        waitingForNextAttack = false;
                        attackIndex = 0;
                    }
                }
                else
                {
                    MoveHeadToWaypoint(currentAttackPath, ref attackIndex, moveAttackSpeed, followAttackDistance, rotationAttackSpeed, loop: false);
                    MoveSegments(moveAttackSpeed, followAttackDistance, rotationAttackSpeed);
                }
                break;
        }
    }

    void MoveHeadToWaypoint(Transform[] path, ref int index, float speed, float followDist, float rotSpeed, bool loop)
    {
        if (path == null || path.Length == 0) return;

        Vector3 target = path[index].position;
        Vector3 direction = (target - segments[0].position).normalized;

        segments[0].position = Vector3.MoveTowards(segments[0].position, target, speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            segments[0].rotation = Quaternion.Slerp(segments[0].rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(segments[0].position, target) < 0.2f)
        {
            index++;

            if (loop)
            {
                index %= path.Length;
            }
            else if (index >= path.Length)
            {
                PickRandomAttackPath();
            }
        }

        if (Vector3.Distance(positions[0], segments[0].position) > followDist)
        {
            positions.Insert(0, segments[0].position);
        }
    }

    void MoveSegments(float speed, float followDist, float rotSpeed)
    {
        for (int i = 1; i < segments.Length; i++)
        {
            int posIndex = Mathf.Min(i * 2, positions.Count - 1);
            Vector3 targetPos = positions[posIndex];
            Vector3 direction = targetPos - segments[i].position;

            segments[i].position = Vector3.MoveTowards(segments[i].position, targetPos, speed * Time.deltaTime);

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                segments[i].rotation = Quaternion.Slerp(segments[i].rotation, targetRotation, rotSpeed * Time.deltaTime);
            }
        }

        if (positions.Count > segments.Length * 10)
        {
            positions.RemoveAt(positions.Count - 1);
        }
    }

    private void PickRandomAttackPath()
    {
        List<Transform[]> validPaths = attackPaths.Where(path => path != null && path.Length > 0).ToList();
        if (validPaths.Count == 0)
        {
            Debug.LogWarning("Brak dostêpnych œcie¿ek ataku!");
            return;
        }

        int randomIndex = Random.Range(0, validPaths.Count);
        currentAttackPath = validPaths[randomIndex];
        attackIndex = 0;

        currentAttackRoot = allAttackPathMainTransform[randomIndex];

        Vector3 startPos = currentAttackPath[0].position;
        segments[0].position = startPos;
        positions.Clear();
        positions.Add(startPos);

        for (int i = 1; i < segments.Length; i++)
        {
            segments[i].position = startPos;
        }

        waitingForNextAttack = true;
        waitTimer = attackPathWaitTime;
    }

    public void SetMove(bool value) => canMove = value;

    void OnDrawGizmos()
    {
        if (wallWaypoints != null && wallWaypoints.Length > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < wallWaypoints.Length - 1; i++)
            {
                if (wallWaypoints[i] != null && wallWaypoints[i + 1] != null)
                {
                    Gizmos.DrawLine(wallWaypoints[i].position, wallWaypoints[i + 1].position);
                }
            }
        }

        Color[] attackColors = { Color.red, Color.magenta, Color.cyan, Color.yellow, Color.blue, Color.gray, Color.white, new Color(1f, 0.5f, 0f) };

        Transform[][] paths = {
            attackWaypointsPath,
            secondAttackWaypointsPath,
            thirdAttackWaypointsPath,
            fourthAttackWaypointsPath,
            fifthAttackWaypointsPath,
            sixthAttackWaypointsPath,
            seventhAttackWaypointsPath,
            eighthAttackWaypointsPath
        };

        for (int p = 0; p < paths.Length; p++)
        {
            Transform[] path = paths[p];
            if (path == null || path.Length < 2) continue;

            Gizmos.color = attackColors[p % attackColors.Length];
            for (int i = 0; i < path.Length - 1; i++)
            {
                if (path[i] != null && path[i + 1] != null)
                {
                    Gizmos.DrawLine(path[i].position, path[i + 1].position);
                    Gizmos.DrawSphere(path[i].position, 0.2f);
                }
            }

            if (path[path.Length - 1] != null)
                Gizmos.DrawSphere(path[path.Length - 1].position, 0.2f);
        }
    }

}
