using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyDieConnect : MonoBehaviour
    {
        private PartHealth partHealth;
        private EnemyMovement enemyMovement;


        [SerializeField] private float stopingDistanceAfterFall = 3;

        private void Awake()
        {
            enemyMovement = GetComponentInParent<EnemyMovement>();
            partHealth = GetComponent<PartHealth>();
        }

        private void Start()
        {
            partHealth.ParticleOnDeath += Die;
            partHealth.DropOnDeath += Die;
        }

        private void Die(Vector3 Position)
        {
            if (partHealth.Name == "Head" || partHealth.Name == "Body")
            {
                enemyMovement.thisEnemyAnimator.SetTrigger("DIE");
                enemyMovement.StopMoving();
            }
            else if (!enemyMovement.isGrowl)
            {
                if (partHealth.Name == "LegLeft" || partHealth.Name == "LegRight")
                {
                    enemyMovement.thisEnemyAnimator.SetTrigger("FALL");
                    enemyMovement.Agent.stoppingDistance = stopingDistanceAfterFall;
                    enemyMovement.isGrowl = true;
                }

            }
        }
    }
}
