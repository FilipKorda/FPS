using System.Collections;
using TMPro;
using UnityEngine;

public class ButtonTurretQuest : MonoBehaviour, IButtonTurretQuest
{
    [Header("-= Gameplay =-")]
    [SerializeField] private Animator turretAnimator;
    [SerializeField] private ParticleSystem hugeExplosion;
    [SerializeField] private ParticleSystem hugeExplosion1;
    [SerializeField] private Animator hangarGateDoor;
    [Header("-= UI =-")]
    [SerializeField] private GameObject hint_Panel;
    [SerializeField] private TextMeshProUGUI hint_Text;

    private Color originalColor;
    private Renderer originalColorRenderer;
    private string HintString => $"Press [E] to Activate Turret";

    void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;
    }

    public void ActivateTurret()
    {
        turretAnimator.SetTrigger("Play");
        StartCoroutine(PlayHugeExplosion());
    }

    private IEnumerator PlayHugeExplosion()
    {
        yield return new WaitForSeconds(6.6f);
        hugeExplosion.Play();
        yield return new WaitForSeconds(0.7f);
        hugeExplosion1.Play();
        hangarGateDoor.SetTrigger("Play");
    }

    public void ActiveHint()
    {
        hint_Panel.SetActive(true);
        hint_Text.text = HintString;
        originalColorRenderer.material.color = Color.yellow;
    }

    public void DeactiveHint()
    {
        hint_Panel.SetActive(false);
        hint_Text.text = "";
        originalColorRenderer.material.color = originalColor;
    }

}
