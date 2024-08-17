using TMPro;
using UnityEngine;

public class Npc : MonoBehaviour, INpc
{
    [SerializeField] private string npcName = "Steve";
    [SerializeField] private GameObject talkToNPC_Panel;
    [SerializeField] private TextMeshProUGUI hintText;
    private string HintString => $"Press [E] to Talk to {npcName}";

    [SerializeField] private Conversation conversationData;

    [SerializeField] private Conversation secondConversationData;

    public bool wasOpen;

    public void TalkToNpc()
    {
        if(!wasOpen)
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
        hintText.text = HintString;
    }

    public void DeactiveHint()
    {
        talkToNPC_Panel.SetActive(false);
        hintText.text = "";
    }

    
}
