using UnityEngine;

namespace FPS.Enemy
{
    public class SnakeSegment : MonoBehaviour
    {
        public SnakeBossHealth bossHealth;

        public void TakeDamage(int damage)
        {
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
        }
    }
}