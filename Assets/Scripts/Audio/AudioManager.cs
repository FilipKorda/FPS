using System.Collections;
using System.Collections.Generic;
using Game.Audio;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Tooltip("Ilosc preinstancjonowanych AudioSource dla SFX")]
    [SerializeField] private int initialPoolSize = 16;

    [Tooltip("Domyslne ustawienia rolloffu dla przestrzennych zrodel")]
    [SerializeField] private AudioRolloffMode defaultRolloff = AudioRolloffMode.Logarithmic;

    private readonly Queue<AudioSource> pool = new Queue<AudioSource>();
    private readonly List<AudioSource> activeLooping = new List<AudioSource>();

    private readonly Dictionary<AudioSource, float> baseVolumes = new Dictionary<AudioSource, float>();
    private readonly Dictionary<AudioSource, int> sourceVersions = new Dictionary<AudioSource, int>();

    private float masterVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializePool(initialPoolSize);
        RefreshVolumes();
    }

    private void InitializePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var go = new GameObject($"PooledAudioSource_{i}");
            go.transform.SetParent(transform, false);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            src.rolloffMode = defaultRolloff;
            pool.Enqueue(src);
            go.SetActive(false);
        }
    }

    private AudioSource GetSource()
    {
        AudioSource src;
        if (pool.Count > 0)
        {
            src = pool.Dequeue();
            src.gameObject.SetActive(true);
        }
        else
        {
            var go = new GameObject("PooledAudioSource_Dynamic");
            go.transform.SetParent(transform, false);
            src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.rolloffMode = defaultRolloff;
        }

        sourceVersions[src] = sourceVersions.TryGetValue(src, out int version) ? version + 1 : 1;
        return src;
    }

    private void ReleaseSource(AudioSource src)
    {
        if (src == null) return;

        if (baseVolumes.ContainsKey(src))
            baseVolumes.Remove(src);

        if (activeLooping.Contains(src))
            activeLooping.Remove(src);

        sourceVersions[src] = sourceVersions.TryGetValue(src, out int releaseVersion) ? releaseVersion + 1 : 1;

        src.Stop();
        src.clip = null;
        src.loop = false;
        src.spatialBlend = 0f;
        src.volume = 1f;
        src.pitch = 1f;
        src.transform.SetParent(transform, false);
        src.gameObject.SetActive(false);
        pool.Enqueue(src);
    }

    public AudioSource PlayClip(
        AudioClip clip,
        Vector3? position = null,
        float volume = 1f,
        bool spatial = false,
        float minDistance = 1f,
        float maxDistance = 500f,
        float pitch = 1f,
        bool loop = false,
        Transform attachTo = null)
    {
        if (clip == null) return null;
        var src = GetSource();
        src.clip = clip;
        src.pitch = Mathf.Clamp(pitch, -3f, 3f);
        src.loop = loop;

        baseVolumes[src] = Mathf.Clamp01(volume);

        bool isMuted = PlayerPrefs.GetInt(AudioKeys.PlayerPrefToggleMute, 0) == 1;
        src.volume = isMuted ? 0f : Mathf.Clamp01(baseVolumes[src] * sfxVolume * masterVolume);

        if (attachTo != null && loop)
        {
            src.transform.SetParent(attachTo, false);
            src.transform.localPosition = Vector3.zero;
            src.spatialBlend = 1f;
            src.minDistance = Mathf.Max(0.01f, minDistance);
            src.maxDistance = Mathf.Max(minDistance, maxDistance);
        }
        else if (spatial)
        {
            src.spatialBlend = 1f;
            src.minDistance = Mathf.Max(0.01f, minDistance);
            src.maxDistance = Mathf.Max(minDistance, maxDistance);
            src.transform.position = attachTo != null ? attachTo.position : position ?? transform.position;
        }
        else
        {
            src.spatialBlend = 0f;
            src.transform.position = transform.position;
        }

        src.Play();

        if (loop)
        {
            if (!activeLooping.Contains(src))
                activeLooping.Add(src);
        }
        else
        {
            float safePitch = Mathf.Max(0.01f, Mathf.Abs(src.pitch));
            int version = sourceVersions[src];
            StartCoroutine(ReleaseAfter(src, version, clip.length / safePitch));
        }

        return src;
    }

    public void Stop(AudioSource src)
    {
        if (src == null) return;
        if (activeLooping.Contains(src)) activeLooping.Remove(src);
        if (baseVolumes.ContainsKey(src)) baseVolumes.Remove(src);
        ReleaseSource(src);
    }

    private IEnumerator ReleaseAfter(AudioSource src, int version, float time)
    {
        if (src == null) yield break;
        yield return new WaitForSeconds(time + 0.05f);

        if (src == null) yield break;
        if (!sourceVersions.TryGetValue(src, out int currentVersion) || currentVersion != version) yield break;
        if (activeLooping.Contains(src)) yield break;
        ReleaseSource(src);
    }

    public void RefreshVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefMasterVolume, 1f);
        sfxVolume = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefSfxVolume, 1f);
        bool isMuted = PlayerPrefs.GetInt(AudioKeys.PlayerPrefToggleMute, 0) == 1;

        foreach (var kv in new List<KeyValuePair<AudioSource, float>>(baseVolumes))
        {
            var src = kv.Key;
            var baseVol = kv.Value;
            if (src != null)
            {
                src.volume = isMuted ? 0f : Mathf.Clamp01(baseVol * sfxVolume * masterVolume);
            }
        }
    }
}
