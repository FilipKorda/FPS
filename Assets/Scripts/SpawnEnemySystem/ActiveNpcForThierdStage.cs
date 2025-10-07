
using System.Collections;
using UnityEngine;

public class ActiveNpcForThierdStage : MonoBehaviour
{
    [SerializeField] private GameObject[] npcGameObjects;


    private void Start()
    {
        foreach (var npc in npcGameObjects)
        {
            npc.SetActive(false);
        }
    }

    public void StartActivatingNpcs()
    {
        StartCoroutine(ActiveNpcGameObjects());
    }

    private IEnumerator ActiveNpcGameObjects()
    {
        yield return new WaitForSeconds(2f);

        foreach (var npc in npcGameObjects)
        {
            npc.SetActive(true);
        }
    }
}
