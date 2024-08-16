using UnityEngine;

public class ResetSounds : MonoBehaviour
{
    [SerializeField] private GameObject hintToReset;
    [SerializeField] private Settings masterVolume;
    [SerializeField] private Settings musicVolume;
    [SerializeField] private Settings sfxVolume;
    [SerializeField] private Settings soundToggle;
    private bool canReset;

    private void OnEnable()
    {
        hintToReset.SetActive(true);
        canReset = true;
    }

    private void Update()
    {
        if (canReset)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reset();
            }
        }
    }

    void Reset()
    {
        Debug.Log("Reset Sound Settings!");
        masterVolume.ResetMaster();
        musicVolume.ResetMusic();
        sfxVolume.ResetSfx();
        soundToggle.ResetMute();
    }

    private void OnDisable()
    {
        canReset = false;
        hintToReset.SetActive(false);
    }
}
