using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class Enemy : MonoBehaviour
    {
        public PartHealth Health;
        public EnemyPainResponse PainResponse;

        private void Start()
        {
            Health.OnTakeDamage += PainResponse.HandlePain;
            Health.ParticleOnDeath += Die;
            Health.DropOnDeath += Die;
        }

        private void Die(Vector3 Position)
        {
            if (EnemyMovementController.Instance.Movement != null)
            {
                EnemyMovementController.Instance.Movement.StopMoving();
            }

            if (Health.Name == "Head" || Health.Name == "Body")
            {
                PainResponse.HandleAllPartDeath();
            }

            PainResponse.HandleDeath();
        }
    }
}