using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private PartHealth Health;
        [SerializeField] private EnemyPainResponse PainResponse;
        [SerializeField] private EnemyMovement EnemyMovement;

        private void Start()
        {
            Health.OnTakeDamage += PainResponse.HandlePain;
            Health.ParticleOnDeath += Die;
            Health.DropOnDeath += Die;
        }

        private void Die(Vector3 Position)
        {
            if (Health.Name == "Head" || Health.Name == "Body")
            {
                PainResponse.HandleAllPartDeath();
            }
            else if (Health.Name == "LegLeft" || Health.Name == "LegRight")
            {
                EnemyMovement.alienAnimator.SetTrigger("CRAWL");
                PainResponse.HandleDeath();
            }          
            else
            {
                PainResponse.HandleDeath();
            }
        }
    }
}