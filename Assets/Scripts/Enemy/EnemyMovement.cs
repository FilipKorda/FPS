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
        [SerializeField] private float followDistance = 10f;
        public Animator alienAnimator;
        public float walkSpeed = 3.5f;
        public float grawlSpeed = 3f;
        public float grawlSpeedWithoutHands = 1f;
        public float runSpeed = 4;

        private Transform playerTransform;
        public NavMeshAgent Agent;
        private static NavMeshTriangulation Triangulation;

        [SerializeField] private PartHealth legLeft;
        [SerializeField] private PartHealth legRight;
        [SerializeField] private PartHealth armLeft;
        [SerializeField] private PartHealth armRight;


        private void Awake()
        {
            playerTransform = PlayerSingleton.Instance.transform;

            Agent = GetComponent<NavMeshAgent>();

            if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
            {
                Triangulation = NavMesh.CalculateTriangulation();
            }

            Agent.speed = walkSpeed;
        }

        private void Start()
        {
            StartCoroutine(Roam());
            alienAnimator.SetTrigger("IDLE");
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= followDistance)
            {
                StopAllCoroutines();
                StartCoroutine(FollowPlayer());
            }
        }

        private IEnumerator Roam()
        {
            WaitForSeconds wait = new(stillDelay);

            while (enabled)
            {
                Agent.speed = walkSpeed;
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

        private IEnumerator FollowPlayer()
        {
            alienAnimator.ResetTrigger("IDLE");
            alienAnimator.ResetTrigger("WALK");
            alienAnimator.SetTrigger("RUN");

            if (legLeft.isActiveAndEnabled && legRight.isActiveAndEnabled)
            {
                Agent.speed = runSpeed;
            }
            else if (!legLeft.isActiveAndEnabled || !legRight.isActiveAndEnabled)
            {
                if (!armLeft.isActiveAndEnabled && !armRight.isActiveAndEnabled)
                {
                    Agent.speed = grawlSpeedWithoutHands;
                }
                else
                {
                    Agent.speed = grawlSpeed;
                }
            }

            while (Vector3.Distance(transform.position, playerTransform.position) <= followDistance)
            {
                Agent.SetDestination(playerTransform.position);
                yield return null;
            }
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            Agent.isStopped = true;
            Agent.enabled = false;
        }

    }
}