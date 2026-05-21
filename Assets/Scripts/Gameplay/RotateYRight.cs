using UnityEngine;

public class RotateYRight : MonoBehaviour
{
    public GameObject rotatingCube;
    public GameObject arrow;
    public GameObject arrow1;
    public GameObject arrow2;

    private float speedX = 0.1f;
    private float speedY = 0.2f;
    private float speedZ = 0.15f;

    private float floatSpeed = 1f;
    private float floatAmount = 0.1f;

    private Vector3 startCubePos;

    private Vector3 startArrowPos;
    private Vector3 startArrow1Pos;
    private Vector3 startArrow2Pos;

    void Start()
    {
        startCubePos = rotatingCube.transform.position;
        startArrowPos = arrow.transform.position;
        startArrow1Pos = arrow1.transform.position;
        startArrow2Pos = arrow2.transform.position;
    }

    void Update()
    {
        rotatingCube.transform.Rotate(speedX, speedY, speedZ);

        float newY = startCubePos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        rotatingCube.transform.position = new Vector3(startCubePos.x, newY, startCubePos.z);

        float newArrowY = startArrowPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        arrow.transform.position = new Vector3(startArrowPos.x, newArrowY, startArrowPos.z);

        float newArrow1Y = startArrow1Pos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        arrow1.transform.position = new Vector3(startArrow1Pos.x, newArrow1Y, startArrow1Pos.z);   

        float newArrow2Y = startArrow2Pos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        arrow2.transform.position = new Vector3(startArrow2Pos.x, newArrow2Y, startArrow2Pos.z);
    }

}