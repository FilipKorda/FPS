using FPS.Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private Animator thisNpcAnimator;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float dealyTimeToRoam = 1.1f;
    [SerializeField] private float stillDelay = 10f;
    [SerializeField] private NavMeshAgent Agent;
    private static NavMeshTriangulation Triangulation;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
        {
            Triangulation = NavMesh.CalculateTriangulation();
        }

        Agent.speed = walkSpeed;
    }

    private void Start()
    {
        StartCoroutine(Roam(dealyTimeToRoam));
    }

    private IEnumerator Roam(float delay)
    {
        yield return new WaitForSeconds(delay);
        WaitForSeconds wait = new WaitForSeconds(stillDelay);

        while (enabled)
        {
            Agent.speed = walkSpeed;
            thisNpcAnimator.ResetTrigger("IDLE");
            thisNpcAnimator.SetTrigger("WALK");

            int index = Random.Range(0, Triangulation.vertices.Length);
            Vector3 destination = Vector3.Lerp(
                Triangulation.vertices[index],
                Triangulation.vertices[(index + 1) % Triangulation.vertices.Length],
                Random.value
            );

            Agent.SetDestination(destination);

            yield return new WaitUntil(() => Agent.remainingDistance <= Agent.stoppingDistance);

            thisNpcAnimator.ResetTrigger("WALK");
            thisNpcAnimator.SetTrigger("IDLE");

            Agent.isStopped = true;
            yield return wait;

            Agent.isStopped = false;
        }
    }

    private void StopMoving()
    {
        StopAllCoroutines();
        Agent.isStopped = true;
    }
}
