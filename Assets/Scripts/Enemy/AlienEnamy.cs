using System.Collections.Generic;
using UnityEngine;

namespace FPS.Enemy
{
    public class AlienEnamy : EnemyMovement
    {
        public bool isDead;
        public List<PartHealth> partHealths = new();
        public List<Enemy> enemys = new();
        public List<DoAfterEnemyDeath> doAfterEnemyDeaths = new();
        public List<MeshCollider> meshColliders = new();

        public void DisablePartHealths()
        {
            foreach (var partHealth in partHealths)
            {
                partHealth.enabled = false;
            }
        }

        public void DisableEnemys()
        {
            foreach (var enemy in enemys)
            {
                enemy.enabled = false;
            }
        }

        public void DisableDoAfterEnemyDeaths()
        {
            foreach (var doAfterEnemyDeath in doAfterEnemyDeaths)
            {
                doAfterEnemyDeath.enabled = false;
            }
        }

        public void DisableMeshColliders()
        {
            foreach (var meshCollider in meshColliders)
            {
                meshCollider.enabled = false;
            }
        }
    }
}
