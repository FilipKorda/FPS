using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using FPS.ImpactSystem.Effects;
using FPS.ImpactSystem.Pool;
using System.Linq;

namespace FPS.ImpactSystem
{
    public class SurfaceManager : MonoBehaviour
    {
        private static SurfaceManager _instance;
        public static SurfaceManager Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one SurfaceManager active in the scene! Destroying latest one: " + name);
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        [SerializeField]
        private List<SurfaceType> Surfaces = new List<SurfaceType>();
        [SerializeField]
        private Surface DefaultSurface;
        private Dictionary<GameObject, ObjectPool<GameObject>> ObjectPools = new();
        [SerializeField] private Transform sfxParentAudioSources;

        public void HandleImpact(GameObject HitObject, Vector3 HitPoint, Vector3 HitNormal, ImpactType Impact, int TriangleIndex)
        {
            if (HitObject.TryGetComponent<Terrain>(out Terrain terrain))
            {
                List<TextureAlpha> activeTextures = GetActiveTexturesFromTerrain(terrain, HitPoint);
                foreach (TextureAlpha activeTexture in activeTextures)
                {
                    SurfaceType surfaceType = Surfaces.Find(surface => surface.Albedo == activeTexture.Texture);
                    if (surfaceType != null)
                    {
                        foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                        {
                            if (typeEffect.ImpactType == Impact)
                            {
                                PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, activeTexture.Alpha);
                            }
                        }
                    }
                    else
                    {
                        foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                        {
                            if (typeEffect.ImpactType == Impact)
                            {
                                PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                            }
                        }
                    }
                }
            }
            else if (HitObject.TryGetComponent<Renderer>(out Renderer renderer))
            {
                Texture activeTexture = GetActiveTextureFromRenderer(renderer, TriangleIndex);

                SurfaceType surfaceType = Surfaces.Find(surface => surface.Albedo == activeTexture);
                if (surfaceType != null)
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
                else
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in DefaultSurface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == Impact)
                        {
                            PlayEffects(HitPoint, HitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
            }
        }

        private List<TextureAlpha> GetActiveTexturesFromTerrain(Terrain Terrain, Vector3 HitPoint)
        {
            Vector3 terrainPosition = HitPoint - Terrain.transform.position;
            Vector3 splatMapPosition = new Vector3(
                terrainPosition.x / Terrain.terrainData.size.x,
                0,
                terrainPosition.z / Terrain.terrainData.size.z
            );

            int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

            float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            List<TextureAlpha> activeTextures = new List<TextureAlpha>();
            for (int i = 0; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > 0)
                {
                    activeTextures.Add(new TextureAlpha()
                    {
                        Texture = Terrain.terrainData.terrainLayers[i].diffuseTexture,
                        Alpha = alphaMap[0, 0, i]
                    });
                }
            }

            return activeTextures;
        }

        private Texture GetActiveTextureFromRenderer(Renderer Renderer, int TriangleIndex)
        {
            if (Renderer.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
            {
                Mesh mesh = meshFilter.sharedMesh;

                return GetTextureFromMesh(mesh, TriangleIndex, Renderer.sharedMaterials);
            }
            else if (Renderer is SkinnedMeshRenderer)
            {
                SkinnedMeshRenderer smr = (SkinnedMeshRenderer)Renderer;
                Mesh mesh = smr.sharedMesh;

                return GetTextureFromMesh(mesh, TriangleIndex, Renderer.sharedMaterials);
            }

            Debug.LogError($"{Renderer.name} has no MeshFilter or SkinnedMeshRenderer! Using default impact effect instead of texture-specific one because we'll be unable to find the correct texture!");
            return null;
        }

        private Texture GetTextureFromMesh(Mesh mesh, int triangleIndex, Material[] materials)
        {
            if (materials == null || materials.Length == 0)
                return null;

            Texture fallbackTexture = materials[0] != null ? materials[0].mainTexture : null;

            if (mesh == null || triangleIndex < 0)
                return fallbackTexture;

            if (!mesh.isReadable)
                return fallbackTexture;

            if (mesh.subMeshCount > 1)
            {
                int[] hitTriangleIndices = new int[]
                {
                    triangleIndex * 3,
                    triangleIndex * 3 + 1,
                    triangleIndex * 3 + 2
                };

                if (hitTriangleIndices.All(index => index < mesh.triangles.Length))
                {
                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        int[] submeshTriangles = mesh.GetTriangles(i);
                        for (int j = 0; j < submeshTriangles.Length; j += 3)
                        {
                            if (submeshTriangles[j] == hitTriangleIndices[0]
                                && submeshTriangles[j + 1] == hitTriangleIndices[1]
                                && submeshTriangles[j + 2] == hitTriangleIndices[2])
                            {
                                return i < materials.Length && materials[i] != null
                                    ? materials[i].mainTexture
                                    : fallbackTexture;
                            }
                        }
                    }
                }
            }

            return fallbackTexture;
        }


        private void PlayEffects(Vector3 HitPoint, Vector3 HitNormal, SurfaceEffect SurfaceEffect, float SoundOffset)
        {
            foreach (SpawnObjectEffect spawnObjectEffect in SurfaceEffect.SpawnObjectEffects)
            {
                
                    if (!ObjectPools.ContainsKey(spawnObjectEffect.Prefab))
                    {
                        ObjectPools.Add(spawnObjectEffect.Prefab, new ObjectPool<GameObject>(() => Instantiate(spawnObjectEffect.Prefab)));
                    }

                    GameObject instance = ObjectPools[spawnObjectEffect.Prefab].Get();

                    if (instance.TryGetComponent(out PoolableObject poolable))
                    {
                        poolable.Parent = ObjectPools[spawnObjectEffect.Prefab];
                    }

                    instance.SetActive(true);
                    instance.transform.position = HitPoint + HitNormal * 0.005f;

                    Vector3 normal = HitNormal;
                    Vector3 up = Vector3.up;

                    if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.9f)
                    {
                        up = Vector3.forward;
                    }

                    Quaternion rotation = Quaternion.LookRotation(-normal, up);
                    instance.transform.rotation = rotation;



                
            }

            foreach (PlayAudioEffect playAudioEffect in SurfaceEffect.PlayAudioEffects)
            {
                if (!ObjectPools.ContainsKey(playAudioEffect.AudioSourcePrefab.gameObject))
                {
                    ObjectPools.Add(
                        playAudioEffect.AudioSourcePrefab.gameObject,
                        new ObjectPool<GameObject>(() =>
                        {
                            return Instantiate(playAudioEffect.AudioSourcePrefab.gameObject, sfxParentAudioSources);
                        })
                    );
                }

                AudioClip clip = playAudioEffect.AudioClips[Random.Range(0, playAudioEffect.AudioClips.Count)];
                GameObject instance = ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject].Get();
                instance.transform.SetParent(sfxParentAudioSources);
                instance.SetActive(true);

                AudioSource audioSource = instance.GetComponent<AudioSource>();
                audioSource.transform.position = HitPoint;
                audioSource.PlayOneShot(clip, SoundOffset * Random.Range(playAudioEffect.VolumeRange.x, playAudioEffect.VolumeRange.y));
                StartCoroutine(DisableAudioSource(ObjectPools[playAudioEffect.AudioSourcePrefab.gameObject], audioSource, clip.length));

            }
        }

        private IEnumerator DisableAudioSource(ObjectPool<GameObject> Pool, AudioSource AudioSource, float Time)
        {
            yield return new WaitForSeconds(Time);

            AudioSource.gameObject.SetActive(false);
            Pool.Release(AudioSource.gameObject);
        }

        private class TextureAlpha
        {
            public float Alpha;
            public Texture Texture;
        }
    }
}
