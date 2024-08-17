using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public Sprite icon;
    public Image image;

    //Testowy komentarz ¿eby zobaczyæ czy comit siê zgadza
    public Vector2 position
    {
        get { return new Vector2(transform.position.x, transform.position.z); }
    }
}
