using TMPro;
using UnityEngine;

public class Npc : MonoBehaviour, INpc
{
    [SerializeField] private string npcName = "Steve";
    [SerializeField] private GameObject talkToNPC_Panel;
    [SerializeField] private TextMeshProUGUI hintText;
    private string HintString => $"Press [E] to Talk to {npcName}";
    public DialogueLine[] dialogue;
    public DialogueLine[] secondDialogue;
    private bool hasTalkedOnce = false;

    public void TalkToNpc()
    {
        if (!hasTalkedOnce)
        {
            DialogueManager.Instance.StartTalk(dialogue);
            hasTalkedOnce = true;
        }
        else if (secondDialogue != null && secondDialogue.Length > 0)
        {
            DialogueManager.Instance.StartSecondTalk(secondDialogue);
        }
    }

    public void ActiveHint()
    {
        talkToNPC_Panel.SetActive(true);
        hintText.text = HintString;
    }

    public void DeactiveHint()
    {
        talkToNPC_Panel.SetActive(false);
        hintText.text = "";
    }
}
