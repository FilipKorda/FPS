using UnityEngine;


[DisallowMultipleComponent]
public class CameraHeadBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform bobTarget;       
    [SerializeField] private Transform motionSource;    
    [SerializeField] private CharacterController characterController; 

    [Header("Behavior")]
    [SerializeField] private bool requireGrounded = true;
    [SerializeField] private float minSpeedToBob = 0.1f;     
    [SerializeField] private float returnSpeed = 12f;       

    [Header("Amplitude (X,Y)")]
    [SerializeField] private Vector2 crouchAmplitude = new Vector2(0.03f, 0.02f);
    [SerializeField] private Vector2 walkAmplitude = new Vector2(0.05f, 0.03f);
    [SerializeField] private Vector2 sprintAmplitude = new Vector2(0.08f, 0.05f);

    [Header("Frequency (Hz)")]
    [SerializeField] private float crouchFrequency = 6f;
    [SerializeField] private float walkFrequency = 9f;
    [SerializeField] private float sprintFrequency = 12f;

    [Header("Shaping")]
    [Tooltip("Wyostrzenie kroku w pionie (1 = czysta sinusoida, >1 = bardziej 'step').")]
    [SerializeField] private float stepSharpness = 2.2f;
    [Tooltip("Wyostrzenie ruchu bocznego (X). 1 = sin/cos, >1 = mocniejsze akcenty L/R.")]
    [SerializeField] private float lateralSharpness = 2.2f;

    [Header("Stomp mode")]
    [Tooltip("Włącza 'stomp' – wyraźne uderzenia lewa/prawa.")]
    [SerializeField] private bool useStompStyle = true;
    [Tooltip("Kształt boczny w stomp (większa wartość = szybsze przejście przez środek, mniej szarpnięć).")]
    [SerializeField] private float stompLateralShape = 5f; 
    [Tooltip("Kształt impulsu pionowego (większa = krótszy i wyższy impuls).")]
    [SerializeField] private float stompImpulseShape = 6f; 
    [Tooltip("Czas wygładzenia SmoothDamp dla stomp (0.04–0.1s).")]
    [SerializeField] private float stompSmoothTime = 0.06f;
    [Tooltip("Maks. prędkość SmoothDamp (ustaw wys. by praktycznie nie ograniczać).")]
    [SerializeField] private float stompMaxSpeed = 100f;

    [Header("State (opcjonalne)")]
    [Tooltip("Jeśli true, użyje poniższych flag, zamiast automatycznego wnioskowania ze speed.")]
    [SerializeField] private bool useManualState = false;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private bool isSprinting = false;

    [Header("Speed mapping (gdy useManualState = false)")]
    [SerializeField] private float sprintSpeedReference = 7.5f; 
    [SerializeField] private float crouchSpeedReference = 2.0f; 

    [Header("Zoom")]
    [SerializeField, Range(0f, 1f)] private float zoomAmplitudeScale = 0.3f;
    private float currentZoomAmpScale = 1f;
    public void SetZooming(bool zoom) => currentZoomAmpScale = zoom ? zoomAmplitudeScale : 1f;

    private Vector3 originalLocalPos; 
    private Vector3 baseLocalPos;    
    private Vector3 bobOffset;     

    private Vector3 lastSourcePos;
    private float phase;

    private Vector3 bobVelocity;

    public void SetCrouching(bool crouch) => isCrouching = crouch;
    public void SetSprinting(bool sprint) => isSprinting = sprint;

    private static float Tanh(float x) => (float)System.Math.Tanh(x);

    private void Awake()
    {
        if (bobTarget == null) bobTarget = transform;
        if (motionSource == null) motionSource = bobTarget.parent != null ? bobTarget.parent : bobTarget;

        originalLocalPos = bobTarget.localPosition;
        baseLocalPos = originalLocalPos;
        bobOffset = Vector3.zero;

        lastSourcePos = motionSource.position;
    }

    private void OnEnable()
    {
        if (bobTarget != null)
        {
            baseLocalPos = bobTarget.localPosition;
            bobOffset = Vector3.zero;
            bobVelocity = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        if (bobTarget != null)
        {
            bobTarget.localPosition = baseLocalPos;
            bobOffset = Vector3.zero;
            bobVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if (bobTarget == null || motionSource == null)
            return;

        float dt = Mathf.Max(0.0001f, Time.deltaTime);

        Vector3 expected = baseLocalPos + bobOffset;
        if ((bobTarget.localPosition - expected).sqrMagnitude > 0.000001f)
        {
            baseLocalPos = bobTarget.localPosition - bobOffset;
        }

        Vector3 srcPos = motionSource.position;
        float speed = (srcPos - lastSourcePos).magnitude / dt;
        lastSourcePos = srcPos;

        bool grounded = !requireGrounded || (characterController == null || characterController.isGrounded);
        bool moving = speed > minSpeedToBob;

        Vector2 amp;
        float freq;

        if (useManualState)
        {
            if (isCrouching) { amp = crouchAmplitude; freq = crouchFrequency; }
            else if (isSprinting) { amp = sprintAmplitude; freq = sprintFrequency; }
            else { amp = walkAmplitude; freq = walkFrequency; }
        }
        else
        {
            float tSprint = Mathf.Clamp01(speed / Mathf.Max(0.0001f, sprintSpeedReference));
            float tCrouch = 1f - Mathf.Clamp01(speed / Mathf.Max(0.0001f, crouchSpeedReference));

            Vector2 crouchToWalk = Vector2.Lerp(crouchAmplitude, walkAmplitude, 1f - tCrouch);
            Vector2 walkToSprint = Vector2.Lerp(walkAmplitude, sprintAmplitude, tSprint);
            amp = Vector2.Lerp(crouchToWalk, walkToSprint, tSprint);

            float freqC2W = Mathf.Lerp(crouchFrequency, walkFrequency, 1f - tCrouch);
            float freqW2S = Mathf.Lerp(walkFrequency, sprintFrequency, tSprint);
            freq = Mathf.Lerp(freqC2W, freqW2S, tSprint);
        }

        amp *= currentZoomAmpScale;

        Vector3 targetOffset;

        if (moving && grounded)
        {
            phase += dt * freq;

            if (useStompStyle)
            {
                float half = Mathf.Repeat(phase, Mathf.PI);
                float t = half / Mathf.PI;

                float x = amp.x * Tanh(Mathf.Cos(phase) * stompLateralShape);

                float k = Mathf.Max(0.0001f, stompImpulseShape);
                float imp = k * t * Mathf.Exp(1f - k * t);
                float y = -amp.y * imp;

                targetOffset = new Vector3(x, y, 0f);
            }
            else
            {
                float c = Mathf.Cos(phase);
                float x = Mathf.Sign(c) * Mathf.Pow(Mathf.Abs(c), lateralSharpness) * amp.x;

                float s = Mathf.Sin(phase);
                float y = Mathf.Sign(s) * Mathf.Pow(Mathf.Abs(s), stepSharpness) * amp.y;

                targetOffset = new Vector3(x, y, 0f);
            }
        }
        else
        {
            targetOffset = Vector3.zero;
            phase = Mathf.Lerp(phase, 0f, dt * 2f);
        }

        if (useStompStyle)
        {
            bobTarget.localPosition = baseLocalPos + Vector3.SmoothDamp(
                bobOffset,
                targetOffset,
                ref bobVelocity,
                stompSmoothTime,
                stompMaxSpeed,
                dt
            );
            bobOffset = bobTarget.localPosition - baseLocalPos;
        }
        else
        {
            bobOffset = Vector3.Lerp(bobOffset, targetOffset, dt * returnSpeed);
            bobTarget.localPosition = baseLocalPos + bobOffset;
        }
    }
}
