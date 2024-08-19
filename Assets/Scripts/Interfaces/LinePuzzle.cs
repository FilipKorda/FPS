using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinePuzzle : MonoBehaviour, ILinePuzzle
{
    private Color originalColor;
    private Renderer originalColorRenderer;

    [SerializeField] private GameObject linePuzzle;
    [SerializeField] private GameObject mainLine;
    [SerializeField] private float mainLineMoveSpeed = 1f;
    [SerializeField] private RectTransform linePuzzleRectTransform;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isMoving = false;

    [SerializeField] private GameObject[] linePuzzleNumberImage;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerController playerController;

    private bool isInLinePuzzle = false;

    private void Start()
    {
        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        startPos = new Vector2(-linePuzzleRectTransform.rect.width / 2, mainLine.GetComponent<RectTransform>().anchoredPosition.y);
        endPos = new Vector2(linePuzzleRectTransform.rect.width / 2, mainLine.GetComponent<RectTransform>().anchoredPosition.y);

        AssignNumbersToText();
    }

    void StartToMoveMainLine()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveMainLine();
            CheckLinePuzzleOverlap();
        }
    }

    public void ActiveLinePuzzle()
    {     
        linePuzzle.SetActive(true);
        mainLine.SetActive(true);
        isInLinePuzzle = true;
        DisablePlayer();
        StartToMoveMainLine();
        MainLineAtStartPosition();
    }

    public void DeactivateLinePuzzle()
    {
        linePuzzle.SetActive(false);
        mainLine.SetActive(false);
        isInLinePuzzle = false;
        EnablePlayer();
        isMoving = false;
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification("Press [E] to Activate Line Puzzle!");
        originalColorRenderer.material.color = Color.yellow;
    }

    public void ResetHighlight()
    {
        NotificationSystem.Instance.HideInfiniteNotification();
        originalColorRenderer.material.color = originalColor;
    }

    public bool IsInLinePuzzle()
    {
        return isInLinePuzzle;
    }

    void DisablePlayer()
    {
        mouseLook.canLookAround = true;
        playerController.canMove = true;
    }

    void EnablePlayer()
    {
        mouseLook.canLookAround = false;
        playerController.canMove = false;
    }

    void MainLineAtStartPosition()
    {
        mainLine.GetComponent<RectTransform>().anchoredPosition = startPos;
    }

    void AssignNumbersToText()
    {
        for (int i = 0; i < linePuzzleNumberImage.Length; i++)
        {
            if (linePuzzleNumberImage[i].TryGetComponent<Image>(out var imageComponent))
            {
                TextMeshProUGUI textComponent = imageComponent.GetComponentInChildren<TextMeshProUGUI>();

                if (textComponent != null)
                {
                    textComponent.text = (i + 1).ToString();
                }
                else
                {
                    Debug.LogWarning($"TextMeshProUGUI not found in child of {linePuzzleNumberImage[i].name}");
                }
            }
            else
            {
                Debug.LogWarning($"Image component not found on {linePuzzleNumberImage[i].name}");
            }
        }
    }

    void MoveMainLine()
    {
        RectTransform mainLineRectTransform = mainLine.GetComponent<RectTransform>();
        mainLineRectTransform.anchoredPosition = Vector2.MoveTowards(mainLineRectTransform.anchoredPosition, endPos, mainLineMoveSpeed * Time.deltaTime);

        // zakoñczenie ca³ego procesu Line Puzzle bo nie uda³o ci siê wykonaæ line puzzle
        if (mainLineRectTransform.anchoredPosition.x >= endPos.x)
        {
            DeactivateLinePuzzle();         
        }
    }

    void CheckLinePuzzleOverlap()
    {
        RectTransform mainLineRectTransform = mainLine.GetComponent<RectTransform>();

        // Górna i dolna po³owa mainLine
        Vector2 topPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y + mainLineRectTransform.rect.height / 8);
        Vector2 bottomPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y - mainLineRectTransform.rect.height / 8);

        foreach (GameObject puzzleImage in linePuzzleNumberImage)
        {
            RectTransform puzzleImageRectTransform = puzzleImage.GetComponent<RectTransform>();

            // Sprawdzanie, czy górna lub dolna czêœæ mainLine nachodzi na puzzleImage
            if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, topPoint, null) ||
                RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, bottomPoint, null))
            {
                Debug.Log($"MainLine overlaps with {puzzleImage.name}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (mainLine != null)
        {
            RectTransform mainLineRectTransform = mainLine.GetComponent<RectTransform>();
            Vector2 topPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y + mainLineRectTransform.rect.height / 8);
            Vector2 bottomPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y - mainLineRectTransform.rect.height / 8);

            // Draw top and bottom points
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topPoint, 5f);
            Gizmos.DrawSphere(bottomPoint, 5f);
        }
    }
}
