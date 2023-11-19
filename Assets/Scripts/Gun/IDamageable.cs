using UnityEngine;

namespace FPS.Enemy
{
    public interface IDamageable
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }

        public delegate void TakeDamageEvent(int Damage);
        public event TakeDamageEvent OnTakeDamage;

        public delegate void ParticleDeathEvent(Vector3 Position);
        public event ParticleDeathEvent ParticleOnDeath;

        public delegate void DropDeathEvent(Vector3 Position);
        public event DropDeathEvent DropOnDeath;

        public void TakeDamage(int Damage);
    }
}