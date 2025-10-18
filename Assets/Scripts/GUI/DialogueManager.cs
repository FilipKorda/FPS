using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private GameObject dialogueView;
    [SerializeField] private GameObject pressEPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private TextMeshProUGUI optionOne;
    [SerializeField] private TextMeshProUGUI optionTwo;
    [SerializeField] private TextMeshProUGUI W;
    [SerializeField] private TextMeshProUGUI Q;

    private ConversationData currentDialogueData;

    public float typingSpeed = 0.03f;
    public bool isTalking = false;
    private bool waitForInput = false;

    private ConversationData optionsSubscribedFrom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pressEPanel.SetActive(false);
        optionOne.gameObject.SetActive(false);
        optionTwo.gameObject.SetActive(false);
        W.gameObject.SetActive(false);
        Q.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                OnPressE();
            }
        }
    }

    public void StartAutomaticDialgue(Conversation conversationData)
    {
        if (conversationData == null || conversationData.conversation == null || conversationData.conversation.Length == 0)
            return;

        StopAllCoroutines();

        isTalking = true;
        playerController.canMove = true;
        mouseLook.canLookAround = true;
        dialogueView.SetActive(true);

        StartCoroutine(AutoTypeDialogue(conversationData.conversation));
    }

    public void StartDialogue(ConversationData[] dialogueData)
    {
        isTalking = true;
        playerController.canMove = true;
        mouseLook.canLookAround = true;
        dialogueView.SetActive(true);
        StartCoroutine(TypeDialogue(dialogueData));
    }

    IEnumerator TypeDialogue(ConversationData[] dialogueData)
    {
        foreach (var dialogue in dialogueData)
        {
            currentDialogueData = dialogue;
            nameText.text = dialogue.Name;

            foreach (var sentence in dialogue.LocalizedSentences)
            {
                yield return TypeLetter(sentence.GetLocalizedString());
                yield return new WaitForSeconds(typingSpeed);

                if (currentDialogueData != null && dialogue.OptionOne != null && dialogue.OptionTwo != null && currentDialogueData.isAskingQuestion)
                {
                    optionOne.gameObject.SetActive(true);
                    optionTwo.gameObject.SetActive(true);
                    W.gameObject.SetActive(true);
                    Q.gameObject.SetActive(true);

                    SubscribeOptionLocalization(dialogue);

                    yield return WaitForAnswer();

                    optionOne.gameObject.SetActive(false);
                    optionTwo.gameObject.SetActive(false);
                    W.gameObject.SetActive(false);
                    Q.gameObject.SetActive(false);

                    UnsubscribeOptionLocalization();
                    optionOne.text = "";
                    optionTwo.text = "";
                }

                waitForInput = true;
                while (waitForInput)
                {
                    yield return null;
                }
            }
        }

        EndDialogue();
    }

    IEnumerator AutoTypeDialogue(ConversationData[] dialogueData)
    {
        foreach (var dialogue in dialogueData)
        {
            currentDialogueData = dialogue;
            nameText.text = dialogue.Name;

            foreach (var sentence in dialogue.LocalizedSentences)
            {
                yield return TypeLetter(sentence.GetLocalizedString());

                yield return new WaitForSeconds(typingSpeed);

                yield return new WaitForSeconds(1.5f);
            }

        }

        GiveQuestToPlayer();
        EndDialogue();
    }

    IEnumerator WaitForAnswer()
    {
        while (currentDialogueData.isAskingQuestion)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChooseAnswer(0);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ChooseAnswer(1);
            }
            yield return null;
        }
    }

    public void ChooseAnswer(int answerIndex)
    {
        if (currentDialogueData.isAskingQuestion)
        {

            if (answerIndex == 0 && currentDialogueData.answerOne != null)
            {

                StartDialogue(currentDialogueData.answerOne.conversation);
            }
            else if (answerIndex == 1 && currentDialogueData.answerTwo != null)
            {

                StartDialogue(currentDialogueData.answerTwo.conversation);
            }
        }
    }

    IEnumerator TypeLetter(string sentence)
    {
        textBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textBox.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        pressEPanel.SetActive(true);
        currentDialogueData = null;
        dialogueView.SetActive(false);
        isTalking = false;
        playerController.canMove = false;
        mouseLook.canLookAround = false;

        UnsubscribeOptionLocalization();
    }

    public void OnPressE()
    {
        GiveQuestToPlayer();

        if (isTalking && waitForInput)
        {
            waitForInput = false;
        }
    }

    public bool IsTalking()
    {
        return isTalking;
    }

    private void GiveQuestToPlayer()
    {
        if (QuestManager.Instance != null && currentDialogueData.questToGive != null)
            QuestManager.Instance.GetQuest(currentDialogueData.questToGive);
    }


    private void SubscribeOptionLocalization(ConversationData dialogue)
    {
        UnsubscribeOptionLocalization();
        optionsSubscribedFrom = dialogue;

        if (dialogue.OptionOne != null)
        {
            optionOne.text = dialogue.OptionOne.GetLocalizedString();
          
            dialogue.OptionOne.StringChanged += OnOptionOneChanged;
        }

        if (dialogue.OptionTwo != null)
        {
            optionTwo.text = dialogue.OptionTwo.GetLocalizedString();
            dialogue.OptionTwo.StringChanged += OnOptionTwoChanged;
        }
    }

    private void UnsubscribeOptionLocalization()
    {
        if (optionsSubscribedFrom == null) return;

        if (optionsSubscribedFrom.OptionOne != null)
            optionsSubscribedFrom.OptionOne.StringChanged -= OnOptionOneChanged;

        if (optionsSubscribedFrom.OptionTwo != null)
            optionsSubscribedFrom.OptionTwo.StringChanged -= OnOptionTwoChanged;

        optionsSubscribedFrom = null;
    }

    private void OnOptionOneChanged(string value)
    {
        optionOne.text = value;
    }

    private void OnOptionTwoChanged(string value)
    {
        optionTwo.text = value;
    }
}
