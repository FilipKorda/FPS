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
    public float typingSpeed = 0.02f;

    private bool isTalking = false;
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

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            OnPressE();
        }
    }

    public void StartDialogue(DialogueData[] dialogueData)
    {
        isTalking = true;
        playerController.canMove = true;
        mouseLook.canLookAround = true;
        dialogueView.SetActive(true);
        StartCoroutine(TypeDialogue(dialogueData));
    }

    IEnumerator TypeDialogue(DialogueData[] dialogueData)
    {
        foreach (var dialogue in dialogueData)
        {
            nameText.text = dialogue.Name;

            foreach (var sentence in dialogue.Sentences)
            {
                yield return TypeLetter(sentence);
                yield return new WaitForSeconds(typingSpeed);

                waitForInput = true;
                while (waitForInput)
                {
                    yield return null;
                }
            }

        }

        EndDialogue();
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


[System.Serializable]
public class DialogueData
{
    public string Name;
    public string[] Sentences;
}