using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ButtonTurretQuest : MonoBehaviour, IButtonTurretQuest
{
    [Header("-= Gameplay =-")]
    [SerializeField] private Animator turretAnimator;
    [SerializeField] private ParticleSystem hugeExplosion;
    [SerializeField] private ParticleSystem hugeExplosion1;
    [SerializeField] private Animator hangarGateDoor;
    [SerializeField] private LoadBarrelForTurret loadBarrelForTurret;
    [SerializeField] private BossFightManager bossFightManager;
    [SerializeField] private EnemyAreaSpawner enemyAreaSpawner;
    [SerializeField] private GameObject[] triggerEnemyToSpawn;
    [Header("-= UI =-")]
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;

    private Color originalColor;
    private Renderer originalColorRenderer;

    public LocalizedString localizeStringEvent;
    [SerializeField] private AudioClip turretSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip explosion2Sound;

    void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void ActivateTurret()
    {
        turretAnimator.SetTrigger("Play");
        StartCoroutine(PlayHugeExplosion());
        AudioManager.Instance.PlayClip(turretSound, transform.position, 0.01f, true, 1, 500, 1, false, null);
    }

    private IEnumerator PlayHugeExplosion()
    {
        yield return new WaitForSeconds(6.6f);
        hugeExplosion.Play();
        AudioManager.Instance.PlayClip(explosionSound, transform.position, 0.5f, true, 1, 500, 1, false, null);
        CameraShake.Instance.AlarmPlayer();
        yield return new WaitForSeconds(0.7f);
        hugeExplosion1.Play();
        AudioManager.Instance.PlayClip(explosion2Sound, transform.position, 0.5f, true, 1, 500, 1, false, null);
        hangarGateDoor.SetTrigger("Play");
        CameraShake.Instance.AlarmPlayer();
        bossFightManager.SetBossToPatrol();
        enemyAreaSpawner.ActivateThirdArea();

        foreach (var item in triggerEnemyToSpawn)
        {
            item.SetActive(true);
        }
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);
        hint_Text.text = localizeStringEvent != null
        ? localizeStringEvent.GetLocalizedString()
        : string.Empty;
        originalColorRenderer.material.color = Color.yellow;
    }

    public void DeactiveHint()
    {
        hint_Panel.SetActive(false);
        hint_Text.text = "";
        originalColorRenderer.material.color = originalColor;
    }

    public bool IsBarrelSet()
    {
        return loadBarrelForTurret.isBarrelSet;
    }
}
