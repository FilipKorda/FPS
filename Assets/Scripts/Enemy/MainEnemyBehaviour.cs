using System.Collections.Generic;
using UnityEngine;

namespace FPS.Enemy
{
    public class MainEnemyBehaviour : EnemyMovement
    {
        public bool enemyAlien;
        public bool enemyOrc;
        public bool isDead;
        public List<PartHealth> partHealths = new();
        public List<EnemyDieConnect> enemyDiesConnects = new();
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
            foreach (var enemy in enemyDiesConnects)
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
