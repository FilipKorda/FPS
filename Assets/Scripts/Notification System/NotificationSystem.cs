using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization;

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

    public void ShowNotification(LocalizedString localizeString, string message, float duration)
    {
        GameObject notificationObject = Instantiate(notificationPrefab, notificationParentTransform);

        TextMeshProUGUI textMesh = notificationObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            SetTextPreferLocalized(textMesh, localizeString, message);
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

    public void ShowInfiniteNotification(LocalizedString localizeString, string message)
    {
        if (activeGunNotification != null)
        {
            Destroy(activeGunNotification);
        }

        activeGunNotification = Instantiate(notificationGunPrefab, notificationGunParentTransform);
        TextMeshProUGUI textMesh = activeGunNotification.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            SetTextPreferLocalized(textMesh, localizeString, message);
        }
    }

    public void HideInfiniteNotification()
    {
        if (activeGunNotification != null)
        {
            Destroy(activeGunNotification);
            activeGunNotification = null;
        }
    }

    private void SetTextPreferLocalized(TextMeshProUGUI textMesh, LocalizedString localizedString, string fallbackMessage)
    {
        if (textMesh == null) return;

        textMesh.text = string.IsNullOrEmpty(fallbackMessage) ? string.Empty : fallbackMessage;

        if (localizedString == null) return;

        var handle = localizedString.GetLocalizedStringAsync();

        if (handle.IsDone)
        {
            if (!string.IsNullOrEmpty(handle.Result))
            {
                textMesh.text = handle.Result;
            }
        }
        else
        {
            handle.Completed += op =>
            {
                if (textMesh != null && !string.IsNullOrEmpty(op.Result))
                {
                    textMesh.text = op.Result;
                }
            };
        }
    }
}
