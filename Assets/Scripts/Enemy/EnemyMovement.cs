using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private float StillDelay = 1f;
        private NavMeshAgent Agent;

        private Coroutine SlowCoroutine;
        private float BaseSpeed;

        private static NavMeshTriangulation Triangulation;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            if (Triangulation.vertices == null || Triangulation.vertices.Length == 0)
            {
                Triangulation = NavMesh.CalculateTriangulation();
            }

            BaseSpeed = Agent.speed;
        }

        private void Start()
        {
            StartCoroutine(Roam());
            BaseSpeed = Agent.speed;
        }


        private IEnumerator Roam()
        {
            WaitForSeconds wait = new(StillDelay);

            while (enabled)
            {
                int index = Random.Range(1, Triangulation.vertices.Length);
                Agent.SetDestination(
                    Vector3.Lerp(
                        Triangulation.vertices[index - 1],
                        Triangulation.vertices[index],
                        Random.value
                    )
                );
                yield return new WaitUntil(() => Agent.remainingDistance <= Agent.stoppingDistance);
                yield return wait;
            }
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            Agent.isStopped = true;
            Agent.enabled = false;
        }

        public void Slow(AnimationCurve SlowCurve)
        {
            if (SlowCoroutine != null)
            {
                StopCoroutine(SlowCoroutine);
            }
            SlowCoroutine = StartCoroutine(SlowDown(SlowCurve));
        }

        private IEnumerator SlowDown(AnimationCurve SlowCurve)
        {
            float time = 0;

            while (time < SlowCurve.keys[^1].time)
            {
                Agent.speed = BaseSpeed * SlowCurve.Evaluate(time);
                time += Time.deltaTime;
                yield return null;
            }

            Agent.speed = BaseSpeed;
        }
    }
}