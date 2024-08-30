using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class AlienMagEnemyMovement : MonoBehaviour
    {
        [SerializeField] private float stillDelay = 1f;
        [SerializeField] private float followDistance = 10f;
        [SerializeField] private AlienMagEnemy alienMagEnemy;
        public Animator alienAnimator;
        public float walkSpeed = 3.5f;
        public float runSpeed = 5f;
        public float dieSpeed = 0f;

        public bool spawnAnimIsOff = false;

        public Transform playerTransform;
        public NavMeshAgent Agent;
        private static NavMeshTriangulation Triangulation;


        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float projectileSpeed = 10f; 

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
            if (!alienMagEnemy.isDead && spawnAnimIsOff)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distanceToPlayer <= followDistance)
                {
                    StopAllCoroutines();
                    StartCoroutine(FollowPlayer());
                }

                if (distanceToPlayer <= Agent.stoppingDistance)
                {
                    AttackPlayer();
                }
            }
        }


        private IEnumerator Roam(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!alienMagEnemy.isDead && spawnAnimIsOff)
            {
                WaitForSeconds wait = new(stillDelay);

                while (enabled)
                {
                    Agent.speed = walkSpeed;
                    alienAnimator.SetTrigger("IDLE");
                    int index = Random.Range(0, Triangulation.vertices.Length);

                    Agent.SetDestination(
                        Vector3.Lerp(
                        Triangulation.vertices[index],
                         Triangulation.vertices[(index + 1) % Triangulation.vertices.Length],
                         Random.value
                          )
                         );

                    yield return new WaitUntil(() => Agent.remainingDistance <= Agent.stoppingDistance);

                    yield return wait;
                }
            }
        }

        private IEnumerator FollowPlayer()
        {
            if (!alienMagEnemy.isDead && spawnAnimIsOff)
            {
                alienAnimator.SetTrigger("IDLE");
                while (Vector3.Distance(transform.position, playerTransform.position) <= followDistance)
                {
                    Agent.SetDestination(playerTransform.position);
                    yield return null;
                }
            }
        }

        private void AttackPlayer()
        {
            alienAnimator.SetTrigger("ATTACK");
        }

        public void ShootProjectile()
        {
            Debug.Log("Shoot Projectile at player position");

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            Vector3 direction = (playerTransform.position - shootPoint.position).normalized;

            projectile.transform.forward = direction;

            if (projectile.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = direction * projectileSpeed;
            }
        }

        private IEnumerator DealySpawnAnimIsOff(float delay)
        {
            yield return new WaitForSeconds(delay);
            spawnAnimIsOff = true;
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            Agent.isStopped = true;
            Agent.speed = dieSpeed;
            alienMagEnemy.isDead = true;
        }
    }
}