using FPS.ImpactSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using FPS.Guns.ImpactEffects;
using FPS.Enemy;

namespace FPS.Guns
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject, System.ICloneable
    {
        public ImpactType ImpactType;
        public GunType Type;
        public Sprite GunIcon;
        public string Name;
        public GameObject ModelPrefab;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        public DamageConfigScriptableObject DamageConfig;
        public ShootConfigurationScriptableObject ShootConfig;
        public AmmoConfigScriptableObject AmmoConfig;
        public TrailConfigurationScriptableObject TrailConfig;
        public AudioConfigScriptableObject AudioConfig;
        public BulletPenetrationConfigScriptableObject BulletPenConfig;

        public ICollisionHandler[] BulletImpactEffects = new ICollisionHandler[0];

        private MonoBehaviour ActiveMonoBehaviour;
        private AudioSource ShootingAudioSource;
        private GameObject Model;
        private Camera ActiveCamera;
        private float LastShootTime;
        private float InitialClickTime;
        private float StopShootingTime;

        private ParticleSystem ShootSystem;
        private ObjectPool<TrailRenderer> TrailPool;
        private ObjectPool<Bullet> BulletPool;
        private bool LastFrameWantedToShoot;

        public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour, Camera Camera = null)
        {
            this.ActiveMonoBehaviour = ActiveMonoBehaviour;

            TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
            if (!ShootConfig.IsHitscan)
            {
                BulletPool = new ObjectPool<Bullet>(CreateBullet);
            }

            Model = Instantiate(ModelPrefab);
            Model.transform.SetParent(Parent, false);

            Model.transform.SetLocalPositionAndRotation(SpawnPoint, Quaternion.Euler(SpawnRotation));
            ActiveCamera = Camera;

            ShootingAudioSource = Model.GetComponent<AudioSource>();
            ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        }

        public void Despawn()
        {
            Model.SetActive(false);
            Destroy(Model);
            TrailPool.Clear();
            BulletPool?.Clear();

            ShootingAudioSource = null;
            ShootSystem = null;
        }

        public void UpdateCamera(Camera Camera)
        {
            ActiveCamera = Camera;
        }

        public void Tick(bool WantsToShoot)
        {
            Model.transform.localRotation = Quaternion.Lerp(
            Model.transform.localRotation,
            Quaternion.Euler(SpawnRotation),
            Time.deltaTime * ShootConfig.RecoilRecoverySpeed
        );

            if (WantsToShoot)
            {
                LastFrameWantedToShoot = true;
                TryToShoot();
            }

            if (!WantsToShoot && LastFrameWantedToShoot)
            {
                StopShootingTime = Time.time;
                LastFrameWantedToShoot = false;
            }

        }

        public void StartReloading()
        {
            Debug.Log("Start Reloading");
            AudioConfig.PlayReloadClip(ShootingAudioSource);
        }

        public void EndReload()
        {
            Debug.Log("Reloading complete");
            AmmoConfig.Reload();
        }

        public bool CanReload()
        {
            return AmmoConfig.CanReload();
        }

        private void TryToShoot()
        {
            if (PlayerSingleton.Instance.canShoot && !DialogueManager.Instance.isTalking && !MainInventory.Instance.isPanelActive)
            {
                if (Time.time - LastShootTime - ShootConfig.FireRate > Time.deltaTime)
                {
                    float lastDuration = Mathf.Clamp(
                        0,
                        StopShootingTime - InitialClickTime,
                        ShootConfig.MaxSpreadTime
                    );
                    float lerpTime = (ShootConfig.RecoilRecoverySpeed - (Time.time - StopShootingTime))
                                     / ShootConfig.RecoilRecoverySpeed;

                    InitialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
                }

                if (Time.time > ShootConfig.FireRate + LastShootTime)
                {
                    LastShootTime = Time.time;
                    if (AmmoConfig.CurrentClipAmmo == 0)
                    {
                        AudioConfig.PlayOutOfAmmoClip(ShootingAudioSource);
                        return;
                    }

                    ShootSystem.Play();
                    AudioConfig.PlayShootingClip(ShootingAudioSource, AmmoConfig.CurrentClipAmmo == 1);

                    Vector3 spreadAmount = ShootConfig.GetSpread(Time.time - InitialClickTime);
                    _ = Vector3.zero;
                    Model.transform.forward += Model.transform.TransformDirection(spreadAmount);
                    Vector3 shootDirection;
                    if (ShootConfig.ShootType == ShootType.FromGun)
                    {
                        shootDirection = ShootSystem.transform.forward;
                    }
                    else
                    {
                        shootDirection = ActiveCamera.transform.forward +
                                         ActiveCamera.transform.TransformDirection(spreadAmount);
                    }

                    AmmoConfig.CurrentClipAmmo--;

                    StatisticsCollector.AddAmmoShot();

                    if (ShootConfig.IsHitscan)
                    {
                        DoHitscanShoot(shootDirection, GetRaycastOrigin(), ShootSystem.transform.position);
                    }
                    else
                    {
                        DoProjectileShoot(shootDirection);
                    }
                }

            }
        }

        private void DoProjectileShoot(Vector3 ShootDirection)
        {
            Bullet bullet = BulletPool.Get();
            bullet.gameObject.SetActive(true);
            bullet.OnCollision += HandleBulletCollision;

            if (ShootConfig.ShootType == ShootType.FromCamera
                && Physics.Raycast(
                    GetRaycastOrigin(),
                    ShootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                Vector3 directionToHit = (hit.point - ShootSystem.transform.position).normalized;
                Model.transform.forward = directionToHit;
                ShootDirection = directionToHit;
            }

            bullet.transform.position = ShootSystem.transform.position;
            bullet.Spawn(ShootDirection * ShootConfig.BulletSpawnForce);

            TrailRenderer trail = TrailPool.Get();
            if (trail != null)
            {
                trail.transform.SetParent(bullet.transform, false);
                trail.transform.localPosition = Vector3.zero;
                trail.emitting = true;
                trail.gameObject.SetActive(true);
            }
        }

        private void DoHitscanShoot(Vector3 ShootDirection, Vector3 Origin, Vector3 TrailOrigin, int Iteration = 0)
        {
            if (Physics.Raycast(
                    Origin,
                    ShootDirection,
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        TrailOrigin,
                        hit.point,
                        hit,
                        Iteration
                    )
                );
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        TrailOrigin,
                        TrailOrigin + (ShootDirection * TrailConfig.MissDistance),
                        new RaycastHit(),
                        Iteration
                    )
                );
            }
        }

        public Vector3 GetRaycastOrigin()
        {
            Vector3 origin = ShootSystem.transform.position;

            if (ShootConfig.ShootType == ShootType.FromCamera)
            {
                origin = ActiveCamera.transform.position
                         + ActiveCamera.transform.forward * Vector3.Distance(
                             ActiveCamera.transform.position,
                             ShootSystem.transform.position
                         );
            }


            return origin;
        }

        public Vector3 GetGunForward()
        {
            return Model.transform.forward;
        }

        private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit, int Iteration = 0)
        {
            TrailRenderer instance = TrailPool.Get();
            instance.gameObject.SetActive(true);
            instance.transform.position = StartPoint;
            yield return null;

            instance.emitting = true;

            float distance = Vector3.Distance(StartPoint, EndPoint);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    StartPoint,
                    EndPoint,
                    Mathf.Clamp01(1 - (remainingDistance / distance))
                );
                remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

                yield return null;
            }

            instance.transform.position = EndPoint;

            if (Hit.collider != null)
            {
                HandleBulletImpact(distance, EndPoint, Hit.normal, Hit.collider, Iteration);
            }

            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            TrailPool.Release(instance);

            if (BulletPenConfig != null && BulletPenConfig.MaxObjectsToPenetrate > Iteration)
            {
                yield return null;
                Vector3 direction = (EndPoint - StartPoint).normalized;
                Vector3 backCastOrigin = Hit.point + direction * BulletPenConfig.MaxPenetrationDepth;

                if (Physics.Raycast(
                        backCastOrigin,
                        -direction,
                        out RaycastHit hit,
                        BulletPenConfig.MaxPenetrationDepth,
                        ShootConfig.HitMask
                    ))
                {
                    Vector3 penetrationOrigin = hit.point;
                    direction += new Vector3(
                        Random.Range(-BulletPenConfig.AccuracyLoss.x, BulletPenConfig.AccuracyLoss.x),
                        Random.Range(-BulletPenConfig.AccuracyLoss.y, BulletPenConfig.AccuracyLoss.y),
                        Random.Range(-BulletPenConfig.AccuracyLoss.z, BulletPenConfig.AccuracyLoss.z)
                    );

                    DoHitscanShoot(direction, penetrationOrigin, penetrationOrigin, Iteration + 1);
                }
            }
        }

        private void HandleBulletCollision(Bullet Bullet, Collision Collision, int ObjectsPenetrated)
        {
            TrailRenderer trail = Bullet.GetComponentInChildren<TrailRenderer>();

            if (Collision != null && BulletPenConfig != null &&
                BulletPenConfig.MaxObjectsToPenetrate > ObjectsPenetrated)
            {
                Vector3 direction = (Bullet.transform.position - Bullet.SpawnLocation).normalized;
                ContactPoint contact = Collision.GetContact(0);
                Vector3 backCastOrigin = contact.point + direction * BulletPenConfig.MaxPenetrationDepth;

                if (Physics.Raycast(
                        backCastOrigin,
                        -direction,
                        out RaycastHit hit,
                        BulletPenConfig.MaxPenetrationDepth,
                        ShootConfig.HitMask
                    ))
                {
                    direction += new Vector3(
                        Random.Range(-BulletPenConfig.AccuracyLoss.x, BulletPenConfig.AccuracyLoss.x),
                        Random.Range(-BulletPenConfig.AccuracyLoss.y, BulletPenConfig.AccuracyLoss.y),
                        Random.Range(-BulletPenConfig.AccuracyLoss.z, BulletPenConfig.AccuracyLoss.z)
                    );
                    Bullet.transform.position = hit.point + direction * 0.01f;

                    Bullet.Rigidbody.velocity = Bullet.SpawnVelocity - direction;
                }
                else
                {
                    DisableTrailAndBullet(trail, Bullet);
                }
            }
            else
            {
                DisableTrailAndBullet(trail, Bullet);
            }

            if (Collision != null)
            {
                ContactPoint contactPoint = Collision.GetContact(0);

                HandleBulletImpact(
                    Vector3.Distance(contactPoint.point, Bullet.SpawnLocation),
                    contactPoint.point,
                    contactPoint.normal,
                    contactPoint.otherCollider,
                    ObjectsPenetrated
                );
            }
        }

        private void DisableTrailAndBullet(TrailRenderer Trail, Bullet Bullet)
        {
            if (Trail != null)
            {
                Trail.transform.SetParent(null, true);
                ActiveMonoBehaviour.StartCoroutine(DelayedDisableTrail(Trail));
            }

            Bullet.gameObject.SetActive(false);
            BulletPool.Release(Bullet);
        }

        private IEnumerator DelayedDisableTrail(TrailRenderer Trail)
        {
            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            Trail.emitting = false;
            Trail.gameObject.SetActive(false);
            TrailPool.Release(Trail);
        }

        private void HandleBulletImpact(
            float DistanceTraveled,
            Vector3 HitLocation,
            Vector3 HitNormal,
            Collider HitCollider,
            int ObjectsPenetrated = 0)
        {
            SurfaceManager.Instance.HandleImpact(
                HitCollider.gameObject,
                HitLocation,
                HitNormal,
                ImpactType,
                0
            );

            if (HitCollider.TryGetComponent(out IDamageable damageable))
            {
                float maxPercentDamage = 1;
                if (BulletPenConfig != null && ObjectsPenetrated > 0)
                {
                    for (int i = 0; i < ObjectsPenetrated; i++)
                    {
                        maxPercentDamage *= BulletPenConfig.DamageRetentionPercentage;
                    }
                }

                var damage = DamageConfig.GetDamage(DistanceTraveled, maxPercentDamage);
                damageable.TakeDamage(damage);

                StatisticsCollector.AddDamage(damage);
            }

            foreach (ICollisionHandler collisionHandler in BulletImpactEffects)
            {
                collisionHandler.HandleImpact(HitCollider, HitLocation, HitNormal, this);
            }
        }

        private TrailRenderer CreateTrail()
        {
            GameObject instance = new("Bullet Trail");
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = TrailConfig.Color;
            trail.material = TrailConfig.Material;
            trail.widthCurve = TrailConfig.WidthCurve;
            trail.time = TrailConfig.Duration;
            trail.minVertexDistance = TrailConfig.MinVertexDistance;

            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(ShootConfig.BulletPrefab);
            Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.mass = ShootConfig.BulletWeight;

            return bullet;
        }

        public object Clone()
        {
            GunScriptableObject config = CreateInstance<GunScriptableObject>();

            config.ImpactType = ImpactType;
            config.Type = Type;
            config.Name = Name;
            config.name = name;
            config.DamageConfig = DamageConfig.Clone() as DamageConfigScriptableObject;
            config.ShootConfig = ShootConfig.Clone() as ShootConfigurationScriptableObject;
            config.AmmoConfig = AmmoConfig.Clone() as AmmoConfigScriptableObject;
            config.TrailConfig = TrailConfig.Clone() as TrailConfigurationScriptableObject;
            config.AudioConfig = AudioConfig.Clone() as AudioConfigScriptableObject;
            config.BulletPenConfig = BulletPenConfig.Clone() as BulletPenetrationConfigScriptableObject;

            config.ModelPrefab = ModelPrefab;
            config.SpawnPoint = SpawnPoint;
            config.SpawnRotation = SpawnRotation;

            return config;
        }
    }
}
