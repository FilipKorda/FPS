using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NumerLinePuzzle : MonoBehaviour
{
   public int number;

    [SerializeField] private Button button;

    public void Reset()
    {
        StartCoroutine(CheckReset());
    }

    public IEnumerator CheckReset()
    {
        yield return null;
        button.interactable = false;
        yield return null;
        button.interactable = true;
    }
}
