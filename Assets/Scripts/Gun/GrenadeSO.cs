using FPS.Guns;
using UnityEngine;

[CreateAssetMenu(fileName = "Grenade", menuName = "Grenades/Grenade", order = 0)]
public class GrenadeSO : ScriptableObject
{
    public Sprite GrenadeIcon;
    public string Name;
    public GameObject ModelPrefab;
    public DamageGrenadeConfigScriptableObject DamageGrenadeConfig;

    public SphereCollider sphereCollider;
    public LayerMask enemyLayer;

    public float ExplosionDelay = 5f;
    public float DestroyDelay = 7f;
    public GameObject ExplosionParticleSystem;

    public void Explode(Vector3 explosionPosition)
    {
        if (sphereCollider != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, sphereCollider.radius, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    float distance = Vector3.Distance(explosionPosition, hitCollider.transform.position);
                    int damage = DamageGrenadeConfig.GetDamage(distance);
                    damageable.TakeDamage(damage);
                    string objectName = hitCollider.gameObject.name;
                    Debug.Log($"Zadano obra¿enia: {damage} obiektowi '{objectName}'");
                }
            }
        }

    }
}
