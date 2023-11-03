using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CompasBar : MonoBehaviour
{
    [SerializeField]
    private GameObject iconPrefab;
    List<QuestMarker> questMarks = new List<QuestMarker>();

    [SerializeField]
    private Transform player;
    [SerializeField]
    private RawImage compasBarImage;

    float compassUnit;

    public QuestMarker one;


    private void Start()
    {
        compassUnit = compasBarImage.rectTransform.rect.width / 360f;

        AddQuestMark(one);

    }
    private void Update()
    {
        compasBarImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach(QuestMarker marker in questMarks)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);
        }
    }

    public void AddQuestMark(QuestMarker marker)
    {
        GameObject newMarker = Instantiate(iconPrefab, compasBarImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        questMarks.Add(marker);
    }

    Vector2 GetPosOnCompass(QuestMarker marker)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(compassUnit * angle, 0f);
    }
}
