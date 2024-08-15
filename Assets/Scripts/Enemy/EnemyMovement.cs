using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float stillDelay = 1f;
        [SerializeField] private Animator alienAnimator;
        private NavMeshAgent Agent;
        public float baseSpeed;

        private static NavMeshTriangulation Triangulation;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
            {
                Triangulation = NavMesh.CalculateTriangulation();
            }

            baseSpeed = Agent.speed;
        }

        private void Start()
        {
            StartCoroutine(Roam());
            baseSpeed = Agent.speed;
            alienAnimator.SetTrigger("IDLE");
        }

        private IEnumerator Roam()
        {
            WaitForSeconds wait = new(stillDelay);

            while (enabled)
            {
                alienAnimator.ResetTrigger("IDLE");
                alienAnimator.SetTrigger("WALK");
                int index = Random.Range(0, Triangulation.vertices.Length);

                Agent.SetDestination(
                    Vector3.Lerp(
                    Triangulation.vertices[index],
                     Triangulation.vertices[(index + 1) % Triangulation.vertices.Length],
                     Random.value
                      )
                     );

                yield return new WaitUntil(() => Agent.remainingDistance <= Agent.stoppingDistance);

                alienAnimator.ResetTrigger("WALK");
                alienAnimator.SetTrigger("IDLE");

                yield return wait;
            }
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            alienAnimator.SetTrigger("IDLE");
            alienAnimator.ResetTrigger("WALK");
            alienAnimator.ResetTrigger("RUN");
            Agent.isStopped = true;
            Agent.enabled = false;
        }
      
    }
}