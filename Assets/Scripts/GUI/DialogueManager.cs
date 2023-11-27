using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private GameObject dialogueView;
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
        optionOne.gameObject.SetActive(false);
        optionTwo.gameObject.SetActive(false);
        W.gameObject.SetActive(false);
        Q.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            OnPressE();
        }
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

            foreach (var sentence in dialogue.Sentences)
            {
                yield return TypeLetter(sentence);
                yield return new WaitForSeconds(typingSpeed);

                if (currentDialogueData.isAskingQuestion && dialogue.OptionOne != null && dialogue.OptionTwo != null)
                {
                    optionOne.gameObject.SetActive(true);
                    optionTwo.gameObject.SetActive(true);
                    W.gameObject.SetActive(true);
                    Q.gameObject.SetActive(true);
                    optionOne.text = dialogue.OptionOne;
                    optionTwo.text = dialogue.OptionTwo;

                    yield return WaitForAnswer();

                    optionOne.gameObject.SetActive(false);
                    optionTwo.gameObject.SetActive(false);
                    W.gameObject.SetActive(false);
                    Q.gameObject.SetActive(false);
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
        currentDialogueData = null;
        dialogueView.SetActive(false);
        isTalking = false;
        playerController.canMove = false;
        mouseLook.canLookAround = false;
    }

    public void OnPressE()
    {
        if (isTalking && waitForInput)
        {
            waitForInput = false;
        }
    }

    public bool IsTalking()
    {
        return isTalking;
    }


}
