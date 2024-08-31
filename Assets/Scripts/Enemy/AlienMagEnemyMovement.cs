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
        private Collider thisCollider;
        private SphereCollider thisSphereCollider;
        public Animator alienAnimator;
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float runSpeed = 5f;
        [SerializeField] private float dieSpeed = 0f;

        public bool spawnAnimIsOff = false;

        private Transform playerTransform;
        public NavMeshAgent agent;
        private static NavMeshTriangulation Triangulation;


        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private bool canShoot;
        [SerializeField] private ParticleSystem projectilePS;
        [SerializeField] private GameObject floatingPS;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            thisCollider = GetComponent<Collider>();
            thisSphereCollider = GetComponent<SphereCollider>();

            playerTransform = PlayerSingleton.Instance.transform;

            agent.speed = walkSpeed;

            if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
            {
                Triangulation = NavMesh.CalculateTriangulation();
            }

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

                if (distanceToPlayer <= agent.stoppingDistance)
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
                    agent.speed = walkSpeed;
                    alienAnimator.SetTrigger("IDLE");
                    int index = Random.Range(0, Triangulation.vertices.Length);

                    agent.SetDestination(
                        Vector3.Lerp(
                        Triangulation.vertices[index],
                         Triangulation.vertices[(index + 1) % Triangulation.vertices.Length],
                         Random.value
                          )
                         );

                    yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

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
                    agent.SetDestination(playerTransform.position);
                    yield return null;
                }
            }
        }

        private void AttackPlayer()
        {
            if (!canShoot)
            {
                alienAnimator.SetTrigger("ATTACK");
                canShoot = true;
            }

        }

        public void ActiveProjectile()
        {
            projectilePS.gameObject.SetActive(true);
            projectilePS.Play();
        }

        public void ShootProjectile()
        {
            Debug.Log("Shoot Projectile at player position");

            canShoot = false;
            projectilePS.gameObject.SetActive(false);
            projectilePS.Stop();

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


        public void StartFollowPlayerAFterHit()
        {
            followDistance = 100;
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            agent.isStopped = true;
            agent.speed = dieSpeed;
            alienMagEnemy.isDead = true;
            projectilePS.gameObject.SetActive(false);
            projectilePS.Stop();
            floatingPS.SetActive(false);    
            thisCollider.enabled = false;
            thisSphereCollider.enabled = false;
        }
    }
}