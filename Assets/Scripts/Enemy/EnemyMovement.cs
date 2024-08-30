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
        [SerializeField] private AlienEnamy alienEnemy;
        public Animator alienAnimator;
        public float walkSpeed = 3.5f;
        public float grawlSpeed = 3f;
        public float grawlSpeedWithoutHands = 1f;
        public float runSpeed = 4;
        public float dieSpeed = 0;
        public bool isGrowl = false;
        public bool spawnAnimIsOff = false;

        private Transform playerTransform;
        public NavMeshAgent Agent;
        private static NavMeshTriangulation Triangulation;

        [SerializeField] private PartHealth legLeft;
        [SerializeField] private PartHealth legRight;
        [SerializeField] private PartHealth armLeft;
        [SerializeField] private PartHealth armRight;


        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();

            if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
            {
                Triangulation = NavMesh.CalculateTriangulation();
            }

            playerTransform = PlayerSingleton.Instance.transform;

            Agent.speed = walkSpeed;
        }

        private void Start()
        {
            StartCoroutine(DealySpawnAnimIsOff(1f));
            StartCoroutine(Roam(1.1f));
            alienAnimator.SetTrigger("IDLE");
        }

        private void Update()
        {
            if (!alienEnemy.isDead && spawnAnimIsOff)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distanceToPlayer <= followDistance)
                {
                    StopAllCoroutines();
                    StartCoroutine(FollowPlayer());
                }

                if (distanceToPlayer <= Agent.stoppingDistance)
                {
                    if (isGrowl)
                    {
                        AttackPlayerGrawl();
                    }
                    else
                    {
                        AttackPlayer();
                    }

                }
            }
        }

        private IEnumerator DealySpawnAnimIsOff(float delay)
        {
            yield return new WaitForSeconds(delay);
            spawnAnimIsOff = true;
        }

        private IEnumerator Roam(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!alienEnemy.isDead && spawnAnimIsOff)
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
        }

        private IEnumerator FollowPlayer()
        {
            if (!alienEnemy.isDead && spawnAnimIsOff)
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
        }

        private void AttackPlayer()
        {
            int index = 0;
            foreach (var partHealth in alienEnemy.partHealths)
            {
                if (index == 0 || index == 1)
                {
                    if (partHealth.isActiveAndEnabled)
                    {
                        alienAnimator.SetTrigger("ATTACK_LEFT_ARM");
                    }
                    else
                    {
                        alienAnimator.SetTrigger("ATTACK_HEAD");
                    }

                    return;
                }
                index++;
            }
        }

        private void AttackPlayerGrawl()
        {
            int index = 0;
            foreach (var partHealth in alienEnemy.partHealths)
            {
                if (index == 0 || index == 1)
                {
                    if (partHealth.isActiveAndEnabled)
                    {
                        alienAnimator.SetTrigger("ATTACK_CRAWL_RIGHTARM");
                    }
                    else
                    {
                        alienAnimator.SetTrigger("ATTACK_CRAWL_HEAD");
                    }
                    return;
                }
                index++;
            }
        }



        public void StartFollowPlayerAFterHit()
        {
            followDistance = 100;
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            Agent.isStopped = true;
            Agent.speed = dieSpeed;
            alienEnemy.isDead = true;

            alienEnemy.DisablePartHealths();
            alienEnemy.DisableEnemys();
            alienEnemy.DisableDoAfterEnemyDeaths();
            alienEnemy.DisableMeshColliders();
        }
    }
}