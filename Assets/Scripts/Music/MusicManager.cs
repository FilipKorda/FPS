using Game.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Clips")]
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private AudioClip loadingClip;
    [SerializeField] private AudioClip levelStartClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip[] sectorClips = new AudioClip[3]; 

    [Header("Settings")]
    [SerializeField, Tooltip("Czas przejœcia (crossfade) w sekundach")] private float crossfadeDuration = 1.5f;
    [SerializeField, Range(0f, 1f), Tooltip("Docelowa g³oœnoœæ muzyki")] private float musicVolume = 1f;

    [Header("Audio")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    [Header("Optional Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private AudioSource[] sources = new AudioSource[2];
    private int activeSource = 0;
    private Coroutine crossfadeRoutine;
    private Coroutine introRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        float saved = PlayerPrefs.GetFloat(AudioKeys.PlayerPrefMusicVolume, musicVolume);
        float linear = (saved > 1f) ? Mathf.Clamp01(saved / 100f) : Mathf.Clamp01(saved);
        musicVolume = linear;

        for (int i = 0; i < 2; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = true;
            
            src.volume = musicVolume;
            if (musicMixerGroup != null)
                src.outputAudioMixerGroup = musicMixerGroup;
            sources[i] = src;
        }

        ApplyMixerVolume(musicVolume);
    }


    public void PlayMainMenu() => PlayClip(mainMenuClip, loop: true);

    public void PlayLoading() => PlayClip(loadingClip, loop: true);

    public void PlayLevelStart(int sectorAfterStart = 0)
    {
        if (levelStartClip == null)
        {
            PlaySector(sectorAfterStart);
            return;
        }

        StopIntroRoutineIfAny();
        introRoutine = StartCoroutine(PlayIntroThenLoop(levelStartClip, sectorAfterStart));
    }

    public void PlaySector(int sectorIndex)
    {
        if (sectorClips == null || sectorClips.Length == 0)
            return;

        if (sectorIndex < 0 || sectorIndex >= sectorClips.Length)
        {
            Debug.LogWarning($"MusicManager: nieprawid³owy index sektora {sectorIndex}");
            sectorIndex = Mathf.Clamp(sectorIndex, 0, sectorClips.Length - 1);
        }

        PlayClip(sectorClips[sectorIndex], loop: true);
    }

    public void PlayWin() => PlayClip(winClip, loop: false);

    public void PlayLose() => PlayClip(loseClip, loop: false);

    public void StopMusic(float fadeDuration = -1f) => PlayClip(null, loop: false, fadeDurationOverride: fadeDuration);

    private void PlayClip(AudioClip clip, bool loop, float fadeDurationOverride = -1f)
    {
        float fade = (fadeDurationOverride > 0f) ? fadeDurationOverride : crossfadeDuration;

        var active = sources[activeSource];
        if (clip != null && active.clip == clip && active.isPlaying && active.loop == loop)
            return;

        int other = 1 - activeSource;
        var otherSrc = sources[other];

        if (clip == null)
        {
            if (crossfadeRoutine != null) StopCoroutine(crossfadeRoutine);
            crossfadeRoutine = StartCoroutine(CrossfadeToSilent(sources[activeSource], fade));
            return;
        }

        otherSrc.clip = clip;
        otherSrc.loop = loop;
        otherSrc.volume = 0f;
        otherSrc.Play();

        if (crossfadeRoutine != null) StopCoroutine(crossfadeRoutine);
        crossfadeRoutine = StartCoroutine(Crossfade(active, otherSrc, fade));
    }

    private IEnumerator PlayIntroThenLoop(AudioClip introClip, int sectorAfter)
    {
        PlayClip(introClip, loop: false);
        
        yield return new WaitUntil(() =>
        {
            var src = sources[activeSource];
            return src.clip == introClip && (!src.isPlaying);
        });

        introRoutine = null;
        PlaySector(sectorAfter);
    }

    private void StopIntroRoutineIfAny()
    {
        if (introRoutine != null)
        {
            StopCoroutine(introRoutine);
            introRoutine = null;
        }
    }

    private IEnumerator Crossfade(AudioSource from, AudioSource to, float duration)
    {
        float t = 0f;
        to.volume = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / Mathf.Max(0.0001f, duration));
            float target = Mathf.Clamp01(musicVolume); 
            to.volume = Mathf.Lerp(0f, target, p);
            from.volume = Mathf.Lerp(target, 0f, p);
            yield return null;
        }

        to.volume = Mathf.Clamp01(musicVolume);
        from.volume = 0f;
        from.Stop();

        activeSource = (sources[0] == to) ? 0 : 1;
        crossfadeRoutine = null;
    }

    private IEnumerator CrossfadeToSilent(AudioSource from, float duration)
    {
        float t = 0f;
        float startVol = from.volume;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / Mathf.Max(0.0001f, duration));
            from.volume = Mathf.Lerp(startVol, 0f, p);
            yield return null;
        }

        from.volume = 0f;
        from.Stop();
        crossfadeRoutine = null;
    }

    public void SetVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i] != null)
                sources[i].volume = musicVolume;
        }

        ApplyMixerVolume(musicVolume);
    }

    private void ApplyMixerVolume(float linear)
    {
        if (audioMixer == null) return;

        if (linear <= 0f)
        {
            audioMixer.SetFloat(AudioKeys.MixerMusicParam, -80f);
        }
        else
        {
            float db = Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
            audioMixer.SetFloat(AudioKeys.MixerMusicParam, db);
        }
    }
}
