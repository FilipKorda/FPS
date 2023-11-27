using TMPro;
using UnityEngine;

public class Doors : MonoBehaviour, IDoorController
{
    [SerializeField] private GameObject Doors_Panel;
    [SerializeField] private TextMeshProUGUI hintText;
    private string HintString => "Press [E] to Open Doors";

    public void OpenDoor()
    {
        Debug.Log("Open Door");
    }

    public void ActiveHint()
    {
        Doors_Panel.SetActive(true);
        hintText.text = HintString;
    }

    public void DeactiveHint()
    {
        Doors_Panel.SetActive(false);
        hintText.text = "";
    }
}
