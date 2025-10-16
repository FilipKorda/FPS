using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Npc : MonoBehaviour, INpc
{
    [SerializeField] private string npcName = "Steve";
    [SerializeField] private GameObject talkToNPC_Panel;
    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private Conversation conversationData;
    [SerializeField] private Conversation secondConversationData;
    public bool wasOpen;

    public LocalizedString localizeStringEvent;

    public void TalkToNpc()
    {
        if (!wasOpen)
        {
            wasOpen = true;
            DialogueManager.Instance.StartDialogue(conversationData.conversation);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(secondConversationData.conversation);
        }
    }

    public void ActiveHint()
    {
        talkToNPC_Panel.SetActive(true);

        hintText.text = localizeStringEvent != null
            ? localizeStringEvent.GetLocalizedString(npcName)
            : string.Empty;
    }

    public void DeactiveHint()
    {
        talkToNPC_Panel.SetActive(false);
        hintText.text = string.Empty;
    }
}
