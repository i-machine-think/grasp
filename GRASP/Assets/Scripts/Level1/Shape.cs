using System.Collections.Generic;
using UnityEngine;

public class Shape : Experiment
{
    public GameObject cube;
    public GameObject sphere;

    public float minSize;

    public float maxSize;

    public Transform xMin;
    public Transform xMax;

    public Transform zMin;
    public Transform zMax;

    private List<GameObject> gameObjects;

    public int objIndex;

    protected override void Start()
    {
        gameObjects = new List<GameObject>
        {
            cube,
            sphere
        };
    }

    public override void Reset()
    {
        base.Reset();
        foreach(GameObject o in gameObjects)
        {
            o.SetActive(false);
        }

        //int randomIndex = Random.Range(0, 2);
        GameObject obj = gameObjects[objIndex];
        obj.SetActive(true);
        float randomSize = Random.Range(minSize, maxSize);
        obj.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        Renderer renderer = obj.GetComponent<Renderer>();
        Color color = Color.black;
        renderer.material.color = color;
        obj.transform.position = GetRandomPosition(obj);

        /* if(randomIndex == 0)
        {
            System.IO.File.WriteAllText("shape_info.txt", "cube");
        } else
        {
            System.IO.File.WriteAllText("shape_info.txt", "ball");
        } */
    }

    private Vector3 GetRandomPosition(GameObject obj)
    {
        float randomLerpFactorX = Random.Range(0.2f, 0.8f);
        float randomLerpFactorZ = Random.Range(0.2f, 0.8f);
        Vector3 randomPositionX = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactorX);
        Vector3 randomPositionZ = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactorZ);

        return new Vector3(randomPositionX.x, randomPositionZ.y + obj.transform.localScale.y / 2, randomPositionZ.z);
    }
}
