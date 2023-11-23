using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GameObject dialogueView;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TextMeshProUGUI nameText;

    public float typingSpeed = 0.02f;
    public bool isTalking;
    private Queue<DialogueLine[]> dialogueQueue = new Queue<DialogueLine[]>();
    private Queue<DialogueLine[]> secondDialogueQueue = new Queue<DialogueLine[]>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartTalk(DialogueLine[] dialogue)
    {
        mouseLook.canLookAround = true;
        playerController.canMove = true;
        if (isTalking)
        {
            dialogueQueue.Enqueue(dialogue);
        }
        else
        {
            StartCoroutine(DisplayDialogue(dialogue));
        }
    }

    public void StartSecondTalk(DialogueLine[] secondDialogue)
    {
        mouseLook.canLookAround = true;
        playerController.canMove = true;
        if (isTalking)
        {
            secondDialogueQueue.Enqueue(secondDialogue);       
        }
        else
        {
            StartCoroutine(DisplayDialogue(secondDialogue, isSecondDialogue: true));
        }
    }

    IEnumerator DisplayDialogue(DialogueLine[] dialogue, bool isSecondDialogue = false)
    {
        isTalking = true;
        dialogueView.SetActive(true);

        foreach (var line in dialogue)
        {
            nameText.text = line.name;
            textField.text = "";
            foreach (char letter in line.dialog.ToCharArray())
            {
                textField.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        EndTalk();

        if (isSecondDialogue)
        {
            if (secondDialogueQueue.Count > 0)
            {
                var nextDialogue = secondDialogueQueue.Dequeue();
                StartCoroutine(DisplayDialogue(nextDialogue, isSecondDialogue: true));
            }
        }
    }

    public void EndTalk()
    {
        mouseLook.canLookAround = false;
        playerController.canMove = false;
        isTalking = false;
        dialogueView.SetActive(false);

        if (dialogueQueue.Count > 0)
        {
            var queuedDialogue = dialogueQueue.Dequeue();
            StartCoroutine(DisplayDialogue(queuedDialogue));
        }
    }
}
[System.Serializable]
public struct DialogueLine
{
    public string name;
    public string dialog;
}