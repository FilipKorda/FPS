using DG.Tweening;
using UnityEngine;

public class CardReader : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y - 0.1f, 0.5f).SetEase(Ease.OutQuad);
    }

}
