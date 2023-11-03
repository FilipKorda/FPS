using UnityEngine;
using System.Linq;

namespace FPS.Guns
{
    [CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Config", order = 2)]
    public class ShootConfigurationScriptableObject : ScriptableObject, System.ICloneable
    {
        public bool IsHitscan = true;
        public Bullet BulletPrefab;
        public float BulletSpawnForce = 100;
        public LayerMask HitMask;
        public float FireRate = 0.25f;
        public BulletSpreadType SpreadType = BulletSpreadType.Simple;
        public float RecoilRecoverySpeed = 1f;
        public float MaxSpreadTime = 1f;
        public float BulletWeight = 0.1f;

        public ShootType ShootType = ShootType.FromGun;


        [Header("Simple Spread")]
        public Vector3 Spread = new (0.1f, 0.1f, 0.1f);
        [Header("Texture-Based Spread")]

        [Range(0.001f, 5f)]
        public float SpreadMultiplier = 0.1f;

        public Texture2D SpreadTexture;

        public Vector3 GetSpread(float ShootTime = 0)
        {
            Vector3 spread = Vector3.zero;

            if (SpreadType == BulletSpreadType.Simple)
            {
                spread = Vector3.Lerp(
                    Vector3.zero,
                    new Vector3(
                        Random.Range(-Spread.x, Spread.x),
                        Random.Range(-Spread.y, Spread.y),
                        Random.Range(-Spread.z, Spread.z)
                    ),
                    Mathf.Clamp01(ShootTime / MaxSpreadTime)
                );
            }
            else if (SpreadType == BulletSpreadType.TextureBased)
            {
                spread = GetTextureDirection(ShootTime);
                spread *= SpreadMultiplier;
            }

            return spread;
        }

     
        private Vector2 GetTextureDirection(float ShootTime)
        {
            Vector2 halfSize = new(SpreadTexture.width / 2f, SpreadTexture.height / 2f);

            int halfSquareExtents = Mathf.CeilToInt(Mathf.Lerp(0.01f, halfSize.x, Mathf.Clamp01(ShootTime / MaxSpreadTime)));

            int minX = Mathf.FloorToInt(halfSize.x) - halfSquareExtents;
            int minY = Mathf.FloorToInt(halfSize.y) - halfSquareExtents;

            Color[] sampleColors = SpreadTexture.GetPixels(
                minX,
                minY,
                halfSquareExtents * 2,
                halfSquareExtents * 2
            );

            float[] colorsAsGrey = System.Array.ConvertAll(sampleColors, (color) => color.grayscale);
            float totalGreyValue = colorsAsGrey.Sum();

            float grey = Random.Range(0, totalGreyValue);
            int i = 0;
            for (; i < colorsAsGrey.Length; i++)
            {
                grey -= colorsAsGrey[i];
                if (grey <= 0)
                {
                    break;
                }
            }

            int x = minX + i % (halfSquareExtents * 2);
            int y = minY + i / (halfSquareExtents * 2);

            Vector2 targetPosition = new(x, y);

            Vector2 direction = (targetPosition - new Vector2(halfSize.x, halfSize.y)) / halfSize.x;

            return direction;
        }

        public object Clone()
        {
            ShootConfigurationScriptableObject config = CreateInstance<ShootConfigurationScriptableObject>();

            Utilities.CopyValues(this, config);

            return config;
        }
    }
}