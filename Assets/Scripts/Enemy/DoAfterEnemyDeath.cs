using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDamageable))]
    public class DoAfterEnemyDeath : MonoBehaviour
    {    
        [Header("= Only For Head And Body Component =")]
        [SerializeField] private PartHealth bodyHealth;
        [SerializeField] private PartHealth headHealth;
        [Header("===========================")]
        [SerializeField] private ParticleSystem DeathSystem;
        [SerializeField] private GameObject droppedBodyPart;
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
            Damageable.ParticleOnDeath += Damageable_OnDeath_SpawnParticle;
            Damageable.DropOnDeath += Damageable_OnDeath_DropObject;
        }

        private void Damageable_OnDeath_SpawnParticle(Vector3 Position)
        {
            SpawnDeathParticleSystem(Position);
            if (droppedBodyPart != null)
                SpawnBodyPart(Position);
            if (bodyHealth != null && bodyHealth.Name == "Body")
            {
                gameObject.SetActive(true);
            }
            else if(headHealth != null && headHealth.Name == "Head")
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

        private void Damageable_OnDeath_DropObject(Vector3 Position)
        {
            SpawnObjectOnDeath();
        }

        private void SpawnObjectOnDeath()
        {
            if (spawnObjectPrefab != null)
            {
                GameObject spawnedObject = Instantiate(spawnObjectPrefab, transform.position, Quaternion.identity);
                Rigidbody spawnedRigidbody = spawnedObject.GetComponent<Rigidbody>();

                Vector3 randomForce = new(Random.Range(minForce, maxForce), Random.Range(minForce, maxForce), Random.Range(minForce, maxForce));
                spawnedRigidbody.AddForce(randomForce, ForceMode.Impulse);

                SphereCollider spawnedSphereCollider = spawnedObject.GetComponent<SphereCollider>();
                spawnedSphereCollider.isTrigger = true;
            }
        }

        private void SpawnDeathParticleSystem(Vector3 position)
        {
            Instantiate(DeathSystem, position, Quaternion.identity);
        }

        public void SpawnBodyPart(Vector3 position)
        {
            Instantiate(droppedBodyPart, position, Quaternion.identity);
        }
    }
}