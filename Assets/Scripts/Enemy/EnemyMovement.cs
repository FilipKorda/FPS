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
        [SerializeField] private MainEnemyBehaviour mainEnemyBehaviour;
        public Animator thisEnemyAnimator;
        public float walkSpeed = 3.5f;
        public float grawlSpeed = 3f;
        public float grawlSpeedWithoutHands = 1f;
        public float runSpeed = 4;
        public float dieSpeed = 0;
        public bool isGrowl = false;
        public bool spawnAnimIsOff = false;
        [SerializeField] private bool canAttack = true;
        public float dealySpawnTiameAnimToOff = 1;
        public float dealyTimeToRoam = 1.1f;

        private Transform playerTransform;
        public NavMeshAgent Agent;
        private static NavMeshTriangulation Triangulation;

        [SerializeField] private PartHealth legLeft;
        [SerializeField] private PartHealth legRight;
        [SerializeField] private PartHealth armLeft;
        [SerializeField] private PartHealth armRight;

        //Shooting
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private bool canShoot;
        [SerializeField] private ParticleSystem projectilePS;


        [SerializeField] private AudioClip respawnSound;
        [SerializeField] private AudioClip loopIdleSound;
        [SerializeField] private AudioClip shootSound;

        private AudioSource loopIdleAudioSource;

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
            AudioManager.Instance.PlayClip(respawnSound, transform.position, 0.1f, true, 1, 500, 1, false, null);
            StartCoroutine(DealySpawnAnimIsOff(dealySpawnTiameAnimToOff));
            StartCoroutine(Roam(dealyTimeToRoam));
            thisEnemyAnimator.SetTrigger("IDLE");
        }

        private void Update()
        {
            if (!mainEnemyBehaviour.isDead && spawnAnimIsOff)
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

            loopIdleAudioSource = AudioManager.Instance.PlayClip(loopIdleSound, transform.position, 0.1f, true, 1, 500, 1, true, transform);
        }

        public void ResetAttackBool()
        {
            canAttack = true;
        }

        private IEnumerator Roam(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!mainEnemyBehaviour.isDead && spawnAnimIsOff)
            {
                WaitForSeconds wait = new(stillDelay);

                while (enabled)
                {
                    Agent.speed = walkSpeed;
                    thisEnemyAnimator.ResetTrigger("IDLE");
                    thisEnemyAnimator.SetTrigger("WALK");
                    int index = Random.Range(0, Triangulation.vertices.Length);

                    Agent.SetDestination(
                        Vector3.Lerp(
                        Triangulation.vertices[index],
                         Triangulation.vertices[(index + 1) % Triangulation.vertices.Length],
                         Random.value
                          )
                         );

                    yield return new WaitUntil(() => Agent.remainingDistance <= Agent.stoppingDistance);

                    thisEnemyAnimator.ResetTrigger("WALK");
                    thisEnemyAnimator.SetTrigger("IDLE");

                    yield return wait;
                }
            }
        }

        private IEnumerator FollowPlayer()
        {
            if (!mainEnemyBehaviour.isDead && spawnAnimIsOff)
            {
                thisEnemyAnimator.ResetTrigger("IDLE");
                thisEnemyAnimator.ResetTrigger("WALK");
                thisEnemyAnimator.SetTrigger("RUN");

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
            foreach (var partHealth in mainEnemyBehaviour.partHealths)
            {
                if (index == 0 || index == 1)
                {
                    if (partHealth.isActiveAndEnabled)
                    {
                        if (canAttack && mainEnemyBehaviour.enemyAlien)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_LEFT_ARM");
                            canAttack = false;
                        }
                        else if (canAttack && mainEnemyBehaviour.enemyOrc)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK");
                            canAttack = false;
                        }
                        else if (canShoot && mainEnemyBehaviour.enemyRobot)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK");
                            canShoot = false;
                        }

                    }
                    else if (canAttack || canShoot)
                    {
                        if (mainEnemyBehaviour.enemyAlien)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_HEAD");
                            canAttack = false;
                        }
                        else if (mainEnemyBehaviour.enemyOrc)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_HEAD");
                            canAttack = false;
                        }
                        else if (mainEnemyBehaviour.enemyRobot)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK");
                            canShoot = false;
                        }
                    }

                    return;
                }
                index++;
            }
        }

        private void AttackPlayerGrawl()
        {
            int index = 0;
            foreach (var partHealth in mainEnemyBehaviour.partHealths)
            {
                if (index == 0 || index == 1)
                {
                    if (partHealth.isActiveAndEnabled)
                    {
                        if (canAttack && mainEnemyBehaviour.enemyAlien)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_CRAWL_RIGHTARM");
                            canAttack = false;
                        }
                        else if (canAttack && mainEnemyBehaviour.enemyOrc)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_CRAWL_RIGHTARM");
                            canAttack = false;
                        }
                        else if (canAttack && mainEnemyBehaviour.enemyRobot)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_ARMS");
                            canAttack = false;
                        }
                    }
                    else if (canAttack)
                    {
                        if (mainEnemyBehaviour.enemyAlien)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_CRAWL_HEAD");
                            canAttack = false;
                        }
                        else if (mainEnemyBehaviour.enemyOrc)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_CRAWL_HEAD");
                            canAttack = false;
                        }
                        else if (mainEnemyBehaviour.enemyRobot)
                        {
                            thisEnemyAnimator.SetTrigger("ATTACK_ARMS");
                            canAttack = false;
                        }
                    }
                    return;
                }
                index++;
            }
        }


        public void ShootProjectile()
        {
            Debug.Log("Shoot Projectile at player position");

            AudioManager.Instance.PlayClip(shootSound, transform.position, 0.05f, false, 1, 500, 1, false, null);

            canShoot = true;
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

        public void ActiveProjectile()
        {
            projectilePS.gameObject.SetActive(true);
            projectilePS.Play();
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
            mainEnemyBehaviour.isDead = true;

            if (loopIdleAudioSource != null)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Stop(loopIdleAudioSource);
                }
                else
                {
                    loopIdleAudioSource.Stop();
                }
                loopIdleAudioSource = null;
            }

            mainEnemyBehaviour.DisablePartHealths();
            mainEnemyBehaviour.DisableEnemys();
            mainEnemyBehaviour.DisableDoAfterEnemyDeaths();
            mainEnemyBehaviour.DisableMeshColliders();
        }
    }
}