using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    private PlayableDirector currentDirector;

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayCutscene(PlayableDirector director)
    {
        if (currentDirector != null)
        {
            StopCurrentCutscene();
        }

        currentDirector = director;
        currentDirector.stopped += OnCutsceneStopped;
        currentDirector.Play();

        OnCutsceneStarted?.Invoke();
    }

    public void StopCurrentCutscene()
    {
        if (currentDirector == null) return;

        currentDirector.stopped -= OnCutsceneStopped;
        currentDirector.Stop();
        currentDirector = null;

        OnCutsceneEnded?.Invoke();
    }

    public void PauseCutscene()
    {
        if (currentDirector == null) return;

        currentDirector.Pause();
    }

    public void ResumeCutscene()
    {
        if (currentDirector == null) return;

        currentDirector.Play();
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        if (currentDirector == director)
        {
            currentDirector.stopped -= OnCutsceneStopped;
            currentDirector = null;
            OnCutsceneEnded?.Invoke();
        }
    }

    public bool IsPlaying()
    {
        return currentDirector != null && currentDirector.state == PlayState.Playing;
    }
}
