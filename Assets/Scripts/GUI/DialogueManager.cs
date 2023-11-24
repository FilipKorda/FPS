using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueManager;

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
    public bool isAnswering;
    private readonly Queue<DialogueLine[]> dialogueQueue = new();


    [SerializeField] private TextMeshProUGUI optionOne;
    [SerializeField] private TextMeshProUGUI optionTwo;

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


    IEnumerator DisplayDialogue(DialogueLine[] dialogueLine)
    {
        isTalking = true;
        dialogueView.SetActive(true);

        foreach (var line in dialogueLine)
        {
            nameText.text = line.name;

            foreach (var dialogueSegment in line.dialog)
            {
                textField.text = "";

                foreach (char letter in dialogueSegment.ToCharArray())
                {
                    textField.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }


                if (line.options.Length > 0)
                {
                    optionOne.gameObject.SetActive(true);
                    optionTwo.gameObject.SetActive(true);
                    optionOne.text = line.options[0].optionText;
                    optionTwo.text = line.options[1].optionText;

                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Q));

                    DialogueOption selectedOption = Input.GetKeyDown(KeyCode.W) ? line.options[0] : line.options[1];
                    StartCoroutine(DisplayAnswerDialogue(selectedOption.response));
                }

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) && !isAnswering);
            }
        }

        EndTalk();
    }

    IEnumerator DisplayAnswerDialogue(AnswerOption answerOption)
    {
        optionOne.gameObject.SetActive(false);
        optionTwo.gameObject.SetActive(false);
        optionOne.text = "";
        optionTwo.text = "";
        isAnswering = true;

        foreach (var answerSegment in answerOption.answer)
        {
            textField.text = "";

            foreach (char letter in answerSegment.ToCharArray())
            {
                textField.text += letter;

                yield return new WaitForSeconds(typingSpeed);
            }

            if (isAnswering)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }


        }

        EndTalk();
    }

    public void EndTalk()
    {
        mouseLook.canLookAround = false;
        playerController.canMove = false;
        isTalking = false;
        isAnswering = false;
        dialogueView.SetActive(false);

        textField.text = "";
        nameText.text = "";

        optionOne.gameObject.SetActive(false);
        optionTwo.gameObject.SetActive(false);
        optionOne.text = "";
        optionTwo.text = "";

        dialogueQueue.Clear();
    }

}

[System.Serializable]
public struct DialogueLine
{
    public string name;
    public string[] dialog;
    public DialogueOption[] options;
}

[System.Serializable]
public struct DialogueOption
{
    public string optionText;
    public AnswerOption response;
}

[System.Serializable]
public struct AnswerOption
{
    public string[] answer;
}
