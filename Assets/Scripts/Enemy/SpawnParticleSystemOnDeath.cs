using UnityEngine;
using FPS.Guns;

namespace FPS.Guns.Demo
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDamageable))]
    public class SpawnParticleSystemOnDeath : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem DeathSystem;
        public IDamageable Damageable;

        [SerializeField] private GameObject spawnObjectPrefab;
        [SerializeField] private float minForce = 0.1f;
        [SerializeField] private float maxForce = 0.2f;

        private void Awake()
        {
            Damageable = GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            Damageable.OnDeath += Damageable_OnDeath;
        }

        private void Damageable_OnDeath(Vector3 Position)
        {
            DropOnDeath();
            Instantiate(DeathSystem, Position, Quaternion.identity);
            gameObject.SetActive(false);
        }

        private void DropOnDeath()
        {
            if (spawnObjectPrefab != null)
            {
                //int numberOfObjectsToSpawn = Random.Range(1, 3);

                //  for (int i = 0; i < numberOfObjectsToSpawn; i++)
                //  {
                GameObject spawnedObject = Instantiate(spawnObjectPrefab, transform.position, Quaternion.identity);
                Rigidbody spawnedRigidbody = spawnedObject.GetComponent<Rigidbody>();

                Vector3 randomForce = new Vector3(Random.Range(minForce, maxForce), Random.Range(minForce, maxForce), Random.Range(minForce, maxForce));
                spawnedRigidbody.AddForce(randomForce, ForceMode.Impulse);

                SphereCollider spawnedSphereCollider = spawnedObject.GetComponent<SphereCollider>();
                spawnedSphereCollider.isTrigger = true;
                //     }

            }
        }
    }
}