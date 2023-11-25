using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private GameObject dialogueView;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI textBox;
    public float typingSpeed = 0.02f;

    private int sentenceIndex;
    private DialogueData[] currentDialogue;
    public bool isTalking = false;
    public bool isfullyDisplayedDialog = false;

    void Awake()
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

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (isfullyDisplayedDialog && isTalking && Input.GetKeyUp(KeyCode.E))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueData[] dialogue)
    {
        isTalking = true;
        playerController.canMove = true;
        mouseLook.canLookAround = true;
        currentDialogue = dialogue;
        sentenceIndex = 0;
        dialogueView.SetActive(true);
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (sentenceIndex < currentDialogue.Length)
        {
            nameText.text = currentDialogue[sentenceIndex].Name;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentDialogue[sentenceIndex].Sentences));
            sentenceIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string[] sentences)
    {

        foreach (string sentanceLine in sentences)
        {
            textBox.text = "";
            isfullyDisplayedDialog = false;

            foreach (char letter in sentanceLine.ToCharArray())
            {
                textBox.text += letter;

                yield return new WaitForSeconds(typingSpeed);

            }

            isfullyDisplayedDialog = true;
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E) && isfullyDisplayedDialog);

        }




    }

    void EndDialogue()
    {
        dialogueView.SetActive(false);
        playerController.canMove = false;
        mouseLook.canLookAround = false;
        isTalking = false;
    }

}

[System.Serializable]
public class DialogueData
{
    public string Name;
    public string[] Sentences;
}