using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private PartHealth Health;
        [SerializeField] private EnemyMovement EnemyMovement;

        private void Start()
        {
            Health.ParticleOnDeath += Die;
            Health.DropOnDeath += Die;
        }

        private void Die(Vector3 Position)
        {
            if (Health.Name == "Head" || Health.Name == "Body")
            {
                EnemyMovement.alienAnimator.SetTrigger("DIE");
                EnemyMovement.StopMoving();
            }
            else if (Health.Name == "LegLeft" || Health.Name == "LegRight")
            {
                EnemyMovement.alienAnimator.SetTrigger("CRAWL");
            }          

        }
    }
}