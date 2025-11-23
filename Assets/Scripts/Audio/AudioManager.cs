using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Tooltip("Iloœæ preinstancjonowanych AudioSource dla SFX")]
    [SerializeField] private int initialPoolSize = 16;

    [Tooltip("Domyœlne ustawienia rolloffu (dla przestrzennych Ÿróde³)")]
    [SerializeField] private AudioRolloffMode defaultRolloff = AudioRolloffMode.Logarithmic;

    private readonly Queue<AudioSource> pool = new Queue<AudioSource>();
    private readonly List<AudioSource> activeLooping = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
        InitializePool(initialPoolSize);
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

        return src;
    }

    private void ReleaseSource(AudioSource src)
    {
        if (src == null) return;
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
        src.volume = Mathf.Clamp01(volume);
        src.pitch = Mathf.Clamp(pitch, -3f, 3f);
        src.loop = loop;

        if (attachTo != null)
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
            src.transform.position = position ?? transform.position;
        }
        else
        {
            src.spatialBlend = 0f;
            src.transform.position = transform.position;
        }

        src.Play();

        if (loop)
        {
            activeLooping.Add(src);
        }
        else
        {
            StartCoroutine(ReleaseAfter(src, clip.length / Mathf.Abs(src.pitch)));
        }

        return src;
    }

    public void Stop(AudioSource src)
    {
        if (src == null) return;
        if (activeLooping.Contains(src)) activeLooping.Remove(src);
        ReleaseSource(src);
    }

    private IEnumerator ReleaseAfter(AudioSource src, float time)
    {
        if (src == null) yield break;
        yield return new WaitForSeconds(time + 0.05f);
        // w miêdzyczasie Ÿród³o mog³o zostaæ zatrzymane rêcznie
        if (src == null) yield break;
        if (activeLooping.Contains(src)) yield break; // nie zwalniaj jeœli to looping
        ReleaseSource(src);
    }
}
