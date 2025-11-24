using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class LinePuzzle : MonoBehaviour, ILinePuzzle
{
    [Header("Puzzle Mechanics")]
    private Color originalColor;
    private Renderer originalColorRenderer;

    [SerializeField] private GameObject linePuzzle;
    [SerializeField] private GameObject mainLine;
    [SerializeField] private float mainLineMoveSpeed = 1f;
    [SerializeField] private RectTransform linePuzzleRectTransform;
    [SerializeField] private GameObject correctNumerPanel;
    [SerializeField] private TextMeshProUGUI correctNumerText;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isMoving = false;
    private bool isPuzzleFinish = false;

    [SerializeField] private GameObject[] linePuzzleNumberImage;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerController playerController;

    private bool isInLinePuzzle = false;

    public int correctNumerOne = 0;
    public int correctNumerTwo = 6;
    public int correctNumerThree = 7;
    public int correctNumerFour = 4;

    private readonly float shakeDuration = 0.2f;
    private readonly float shakeMagnitude = 2f;
    private Vector3 originalPosition;

    private bool correctNumberOneWasPreesed;
    private bool correctNumberTwoWasPreesed;
    private bool correctNumberThreeWasPreesed;
    private bool correctNumberFourWasPreesed;

    private bool youPassedThePuzzle;

    private bool hasLost;
    private int[] correctOrder;

    private readonly Dictionary<GameObject, Color> originalButtonColors = new Dictionary<GameObject, Color>();

    [Header("Platform Moving")]
    public Transform platformObject;
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 0.45f;
    private bool isPlatformMoving = false;
    private float t = 0.0f;

    public LocalizedString localizeStringEvent;
    [SerializeField] private AudioClip bridgeSound;
    private AudioSource loopIdleAudioSource;

    private void Start()
    {
        youPassedThePuzzle = false;
        hasLost = false;

        originalColorRenderer = GetComponent<Renderer>();
        originalColor = originalColorRenderer.material.color;

        startPos = new Vector2(-linePuzzleRectTransform.rect.width / 2, mainLine.GetComponent<RectTransform>().anchoredPosition.y);
        endPos = new Vector2(linePuzzleRectTransform.rect.width / 2, mainLine.GetComponent<RectTransform>().anchoredPosition.y);

        AssignNumbersToText();

        if (linePuzzle != null)
        {
            originalPosition = linePuzzle.transform.localPosition;
        }

        correctOrder = new[] { correctNumerOne, correctNumerTwo, correctNumerThree, correctNumerFour };
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

        if (isPlatformMoving && platformObject != null)
        {
            MovePlatformToNextPoint();
        }
    }

    [ContextMenu("Active Line Puzzle")]
    public void ActiveLinePuzzle()
    {
        PlayerSingleton.Instance.canShoot = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateNumberOneUI();
        correctNumerPanel.SetActive(true);
        linePuzzle.SetActive(true);
        mainLine.SetActive(true);
        isInLinePuzzle = true;
        DisablePlayer();
        StartToMoveMainLine();
        MainLineAtStartPosition();
        ResetButtonColors(); 
        EnableButtons();

        hasLost = false;

        correctNumberOneWasPreesed = false;
        correctNumberTwoWasPreesed = false;
        correctNumberThreeWasPreesed = false;
        correctNumberFourWasPreesed = false;
        youPassedThePuzzle = false;
    }

    public void DeactivateLinePuzzle()
    {
        PlayerSingleton.Instance.canShoot = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        correctNumerPanel.SetActive(false);
        linePuzzle.SetActive(false);
        mainLine.SetActive(false);
        isInLinePuzzle = false;
        EnablePlayer();
        isMoving = false;
    }

    public void ShakeAfterLosePuzzle()
    {
        if (hasLost || youPassedThePuzzle) return;
        hasLost = true;

        isMoving = false;
        if (linePuzzle != null)
        {
            StartCoroutine(Shake());
        }
    }

    void DisableButtons()
    {
        foreach (GameObject puzzleImage in linePuzzleNumberImage)
        {
            Button button = puzzleImage.GetComponent<Button>();
            button.interactable = false;
        }
    }

    void EnableButtons()
    {
        foreach (GameObject puzzleImage in linePuzzleNumberImage)
        {
            Button button = puzzleImage.GetComponent<Button>();
            button.interactable = true;
        }
    }

    private IEnumerator Shake()
    {
        DisableButtons();

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            linePuzzle.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        linePuzzle.transform.localPosition = originalPosition;

        yield return new WaitForSeconds(1);
        DeactivateLinePuzzle();
    }

    public void Highlight()
    {
        NotificationSystem.Instance.ShowInfiniteNotification(localizeStringEvent, "Press [E] to Activate Line Puzzle!");
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

    public bool IsInLinePuzzleFinish()
    {
        return isPuzzleFinish;
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
                    textComponent.text = i.ToString();
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

        if (mainLineRectTransform.anchoredPosition.x >= endPos.x)
        {
            if (correctNumberOneWasPreesed && correctNumberTwoWasPreesed && correctNumberThreeWasPreesed && correctNumberFourWasPreesed)
            {
                Debug.Log("Platform Move and u win the puzzle!");
                youPassedThePuzzle = true;
                DeactivateLinePuzzle();
                ActivatePlatform();
            }
            else
            {
                ShakeAfterLosePuzzle();
            }
        }
    }

    void CheckLinePuzzleOverlap()
    {
        RectTransform mainLineRectTransform = mainLine.GetComponent<RectTransform>();

        Vector2 topPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y + mainLineRectTransform.rect.height / 8);
        Vector2 bottomPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y - mainLineRectTransform.rect.height / 8);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject clicked = null;
            RectTransform clickedRect = null;
            NumerLinePuzzle clickedNumer = null;

            foreach (GameObject puzzleImage in linePuzzleNumberImage)
            {
                RectTransform rt = puzzleImage.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, null))
                {
                    clicked = puzzleImage;
                    clickedRect = rt;
                    clickedNumer = puzzleImage.GetComponent<NumerLinePuzzle>();
                    break;
                }
            }

            if (clicked != null)
            {
                bool isOver = IsOverlappingRect(clickedRect, topPoint, bottomPoint);
                int expected = GetExpectedNumber();
                int clickedNumber = clickedNumer != null ? clickedNumer.number : -999;

                if (!isOver || clickedNumber != expected)
                {
                    ColorizeButton(clicked, Color.red);
                    ShakeAfterLosePuzzle();
                    return;
                }
            }
        }

        int currentStage = GetCurrentStage();
        if (currentStage < 4)
        {
            int expectedNumber = GetExpectedNumber();
            if (TryGetPuzzleByNumber(expectedNumber, out var expectedGO, out var expectedRect, out _))
            {
                if (HasMainLinePassedRightEdge(expectedRect, mainLineRectTransform) && !HasPressedStage(currentStage))
                {
                    ShakeAfterLosePuzzle();
                    return;
                }
            }
        }

        foreach (GameObject puzzleImage in linePuzzleNumberImage)
        {
            RectTransform puzzleImageRectTransform = puzzleImage.GetComponent<RectTransform>();
            NumerLinePuzzle numerLinePuzzle = puzzleImage.GetComponent<NumerLinePuzzle>();
            Button button = puzzleImage.GetComponent<Button>();

            if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, topPoint, null) ||
                RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, bottomPoint, null))
            {
                Debug.Log($"MainLine overlaps with {numerLinePuzzle.number}");

                if (numerLinePuzzle.number == correctNumerOne)
                {
                    if (button != null && Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, Input.mousePosition, null))
                        {
                            ColorizeButton(puzzleImage, Color.green); 
                            UpdateNumberTwoUI();
                        }
                    }
                }
                if (correctNumberOneWasPreesed && numerLinePuzzle.number == correctNumerTwo)
                {
                    if (button != null && Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, Input.mousePosition, null))
                        {
                            ColorizeButton(puzzleImage, Color.green); 
                            UpdateNumberThreeUI();
                        }
                    }
                }
                if (correctNumberOneWasPreesed && correctNumberTwoWasPreesed && numerLinePuzzle.number == correctNumerThree)
                {
                    if (button != null && Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, Input.mousePosition, null))
                        {
                            ColorizeButton(puzzleImage, Color.green); 
                            UpdateNumberFourUI();
                        }
                    }
                }
                if (correctNumberOneWasPreesed && correctNumberTwoWasPreesed && correctNumberThreeWasPreesed && numerLinePuzzle.number == correctNumerFour)
                {
                    if (button != null && Input.GetMouseButtonDown(0))
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(puzzleImageRectTransform, Input.mousePosition, null))
                        {
                            ColorizeButton(puzzleImage, Color.green); 
                            correctNumberFourWasPreesed = true;
                            correctNumerPanel.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    void UpdateNumberOneUI()
    {
        correctNumerText.text = correctNumerOne.ToString();
    }
    void UpdateNumberTwoUI()
    {
        correctNumerText.text = correctNumerTwo.ToString();
        correctNumberOneWasPreesed = true;
    }
    void UpdateNumberThreeUI()
    {
        correctNumerText.text = correctNumerThree.ToString();
        correctNumberTwoWasPreesed = true;
    }
    void UpdateNumberFourUI()
    {
        correctNumerText.text = correctNumerFour.ToString();
        correctNumberThreeWasPreesed = true;
    }

    private void OnDrawGizmos()
    {
        if (mainLine != null)
        {
            RectTransform mainLineRectTransform = mainLine.GetComponent<RectTransform>();
            Vector2 topPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y + mainLineRectTransform.rect.height / 8);
            Vector2 bottomPoint = new Vector2(mainLineRectTransform.position.x, mainLineRectTransform.position.y - mainLineRectTransform.rect.height / 8);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(topPoint, 5f);
            Gizmos.DrawSphere(bottomPoint, 5f);
        }
    }

    public void ActivatePlatform()
    {
        isPuzzleFinish = true;

        loopIdleAudioSource = AudioManager.Instance.PlayClip(bridgeSound, transform.position, 0.05f, true, 1, 500, 1, true, platformObject.transform);

        if (!isPlatformMoving && youPassedThePuzzle)
        {
            isPlatformMoving = true;
            t = 0.0f;

            if (loopIdleAudioSource != null)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Stop(loopIdleAudioSource);
                }
                else
                {
                    loopIdleAudioSource.Stop();
                }
                loopIdleAudioSource = null;
            }
        }
    }

    void MovePlatformToNextPoint()
    {
        t += Time.deltaTime * moveSpeed;
        platformObject.position = Vector3.Lerp(pointA.position, pointB.position, t);
        if (t >= 1.0f)
        {
            isMoving = false;
           
        }
    }

    int GetCurrentStage()
    {
        if (correctNumberFourWasPreesed) return 4;
        if (correctNumberThreeWasPreesed) return 3;
        if (correctNumberTwoWasPreesed) return 2;
        if (correctNumberOneWasPreesed) return 1;
        return 0;
    }

    int GetExpectedNumber()
    {
        int stage = GetCurrentStage();
        if (stage >= 4) return -1;
        return correctOrder[stage];
    }

    bool HasPressedStage(int stage)
    {
        switch (stage)
        {
            case 0: return correctNumberOneWasPreesed;
            case 1: return correctNumberTwoWasPreesed;
            case 2: return correctNumberThreeWasPreesed;
            case 3: return correctNumberFourWasPreesed;
            default: return true;
        }
    }

    bool TryGetPuzzleByNumber(int number, out GameObject puzzleGo, out RectTransform rect, out NumerLinePuzzle numer)
    {
        foreach (var go in linePuzzleNumberImage)
        {
            var n = go.GetComponent<NumerLinePuzzle>();
            if (n != null && n.number == number)
            {
                puzzleGo = go;
                rect = go.GetComponent<RectTransform>();
                numer = n;
                return true;
            }
        }
        puzzleGo = null;
        rect = null;
        numer = null;
        return false;
    }

    bool IsOverlappingRect(RectTransform rect, Vector2 topPoint, Vector2 bottomPoint)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, topPoint, null)
               || RectTransformUtility.RectangleContainsScreenPoint(rect, bottomPoint, null);
    }

    bool HasMainLinePassedRightEdge(RectTransform targetRect, RectTransform mainLineRectTransform)
    {
        Vector3[] wc = new Vector3[4];
        targetRect.GetWorldCorners(wc);
        float rightEdgeX = wc[2].x; 
        return mainLineRectTransform.position.x > rightEdgeX;
    }

    void ColorizeButton(GameObject go, Color color)
    {
        var g = GetTargetGraphic(go);
        if (g != null)
        {
            if (!originalButtonColors.ContainsKey(go))
                originalButtonColors[go] = g.color;
            g.color = color;
        }
    }

    void ResetButtonColors()
    {
        foreach (var go in linePuzzleNumberImage)
        {
            var g = GetTargetGraphic(go);
            if (g == null) continue;

            if (originalButtonColors.TryGetValue(go, out var original))
                g.color = original;
            else
                g.color = Color.white;
        }
    }

    Graphic GetTargetGraphic(GameObject go)
    {
        var btn = go.GetComponent<Button>();
        if (btn != null && btn.targetGraphic != null) return btn.targetGraphic;
        return go.GetComponent<Image>();
    }
}
