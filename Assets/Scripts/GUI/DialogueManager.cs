using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private GameObject dialogueView;
    [SerializeField] private TextMeshProUGUI textField;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTalk()
    {
        dialogueView.SetActive(true);
    }

    public void EndTalk()
    {
        dialogueView.SetActive(false);
    }
}
