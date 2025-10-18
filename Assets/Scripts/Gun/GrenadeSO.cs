 using System;
using FPS.Enemy;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Grenade", menuName = "Grenades/Grenade", order = 0)]
public class GrenadeSO : ScriptableObject
{
    public Sprite GrenadeIcon;

    public string Name;

    public LocalizedString localizeStringGrenadeName;
    public GameObject ModelPrefab;
    public DamageGrenadeConfigScriptableObject DamageGrenadeConfig;

    public SphereCollider sphereCollider;
    public LayerMask enemyLayer;

    public float ExplosionDelay = 5f;
    public float DestroyDelay = 7f;
    public GameObject ExplosionParticleSystem;

    public string GetLocalizedName()
    {
        string localized = null;
        try
        {
            localized = localizeStringGrenadeName != null ? localizeStringGrenadeName.GetLocalizedString() : null;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[GrenadeSO] B³¹d pobierania zlokalizowanej nazwy: {e.Message}");
        }

        if (!string.IsNullOrEmpty(localized)) return localized;
        return name; 
    }

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
