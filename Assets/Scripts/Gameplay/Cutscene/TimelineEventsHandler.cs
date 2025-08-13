using UnityEngine;
using System.Collections;

public class TimelineEventsHandler : MonoBehaviour
{
    [Header("End Cutscene")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera cutsceneCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MeshRenderer playerMeshRenderer;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSingleton playerSingleton;

    [Header("Blink Settings")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float blinkDuration = 0.6f;
    [SerializeField] private AnimationCurve blinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Dialogues")]
    [SerializeField] private DialogueManager dialogueManager;


    public void Blink()
    {
        if (fadeCanvas != null)
            StartCoroutine(BlinkCoroutine());
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }

    public void StartDialogue(string dialogueId)
    {

    }


    public void StartCutscene()
    {
        playerController.canMove = true;
        mouseLook.canLookAround = true;
        playerSingleton.canShoot = false;
        playerMeshRenderer.enabled = false;
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        if (fadeCanvas != null)
            fadeCanvas.gameObject.SetActive(true);
        if (playerTransform != null)
        {
            playerTransform.position = new Vector3(-35.5f, 1f, 68.932f);
            playerTransform.rotation = Quaternion.Euler(0f, -135f, 0f);
        }
    }

    public void EndCutscene()
    {
        playerController.canMove = false;
        mouseLook.canLookAround = false;
        playerSingleton.canShoot = true;
        playerMeshRenderer.enabled = true;
        playerCamera.gameObject.SetActive(true);
        cutsceneCamera.gameObject.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.gameObject.SetActive(false);
    }


    private IEnumerator BlinkCoroutine()
    {
        if (fadeCanvas == null)
            yield break;

        for (int i = 0; i < 3; i++)
        {
            float halfDuration = blinkDuration / 2f;
            float timer = 0f;

            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / halfDuration);
                fadeCanvas.alpha = blinkCurve.Evaluate(t);
                yield return null;
            }

            timer = 0f;
            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / halfDuration);
                fadeCanvas.alpha = blinkCurve.Evaluate(1 - t);
                yield return null;
            }
        }

        fadeCanvas.alpha = 0f;
    }
}
