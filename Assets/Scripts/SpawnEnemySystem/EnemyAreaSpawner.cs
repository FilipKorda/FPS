using UnityEngine;

public class EnemyAreaSpawner : MonoBehaviour
{
    [SerializeField] private GameObject firstEnemyArea;
    [SerializeField] private GameObject firstEnemyAreaNavmesh;
    [SerializeField] private GameObject secondEnemyArea;
    [SerializeField] private GameObject secondEnemyAreaNavmesh;
    [SerializeField] private GameObject thirdEnemyArea;
    [SerializeField] private GameObject thirdEnemyAreaNavmesh;


    private void Start()
    {
        firstEnemyArea.SetActive(true);
        secondEnemyArea.SetActive(false);
        thirdEnemyArea.SetActive(false);

        firstEnemyAreaNavmesh.SetActive(true);
        secondEnemyAreaNavmesh.SetActive(false);
        thirdEnemyAreaNavmesh.SetActive(false);
    }


    public void ActivateSecondArea()
    {
        firstEnemyArea.SetActive(false);
        secondEnemyArea.SetActive(true);
        thirdEnemyArea.SetActive(false);


        firstEnemyAreaNavmesh.SetActive(false);
        secondEnemyAreaNavmesh.SetActive(true);
        thirdEnemyAreaNavmesh.SetActive(false);
    }

    public void ActivateThirdArea()
    {
        firstEnemyArea.SetActive(false);
        secondEnemyArea.SetActive(false);
        thirdEnemyArea.SetActive(true);

        firstEnemyAreaNavmesh.SetActive(false);
        secondEnemyAreaNavmesh.SetActive(false);
        thirdEnemyAreaNavmesh.SetActive(true);
    }

}
