using UnityEngine;
using DG.Tweening;
using TMPro;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance { get; private set; }
    public GameObject notificationPrefab;
    public Transform notificationParentTransform;

    public GameObject notificationGunPrefab;
    public Transform notificationGunParentTransform;
    private GameObject activeGunNotification;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification(string message, float duration)
    {
        GameObject notificationObject = Instantiate(notificationPrefab, notificationParentTransform);

        TextMeshProUGUI textMesh = notificationObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = message;
        }

        AnimateNotification(notificationObject.transform, duration);
    }

    private void AnimateNotification(Transform obj, float duration)
    {
        obj.DOMoveX(obj.position.x, duration).SetRelative();
        obj.DOScale(Vector3.one * 1, 1)
            .OnComplete(() =>
            {
                obj.DOScale(Vector3.one, duration).OnComplete(() =>
                {
                    Destroy(obj.gameObject);
                });
            });
    }

    public void ShowGunNotification(string message)
    {
        if (activeGunNotification != null)
        {
            // Jeœli istnieje aktywne powiadomienie, zniszcz je
            Destroy(activeGunNotification);
        }

        activeGunNotification = Instantiate(notificationGunPrefab, notificationGunParentTransform);
        TextMeshProUGUI textMesh = activeGunNotification.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textMesh.text = message;
        }
    }

    public void HideGunNotification()
    {
        if (activeGunNotification != null)
        {
            Destroy(activeGunNotification);
            activeGunNotification = null;
        }
    }
}
