using System.Collections.Generic;
using UnityEngine;

public class Colors : Experiment
{
    public GameObject sphere;

    public float minSize;

    public float maxSize;

    public Transform xMin;
    public Transform xMax;

    public Transform zMin;
    public Transform zMax;

    private List<string> colorNames;
    public int colorIndex;

    protected override void Start()
    {

        colorNames = new List<string>
        {
            "Red",
            "Green",
            "Blue",
            "Black"
        };
    }

    public override void Reset()
    {
        base.Reset();
        sphere.SetActive(true);
        float randomSize = Random.Range(minSize, maxSize);
        sphere.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        Renderer renderer = sphere.GetComponent<Renderer>();
        Color color = MapColorNameToColor(colorNames[colorIndex]);
        renderer.material.color = color;
        sphere.transform.position = GetRandomPosition(sphere);

        //System.IO.File.WriteAllText("color_info.txt", GetColorName(color));
    }

    private Vector3 GetRandomPosition(GameObject obj)
    {
        float randomLerpFactorX = Random.Range(0.2f, 0.8f);
        float randomLerpFactorZ = Random.Range(0.2f, 0.8f);
        Vector3 randomPositionX = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactorX);
        Vector3 randomPositionZ = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactorZ);

        return new Vector3(randomPositionX.x, randomPositionZ.y + obj.transform.localScale.y / 2, randomPositionZ.z);
    }

    string GetColorName(Color color)
    {
        if (color == Color.red)
        {
            return "red";
        }
        else if (color == Color.green)
        {
            return "green";
        }
        else if (color == Color.blue)
        {
            return "blue";
        }
        else if (color == Color.black)
        {
            return "black";
        }
        else
        {
            return "UnknownColor";
        }
    }


    Color MapColorNameToColor(string colorName)
    {
        switch (colorName)
        {
            case "Red":
                return Color.red;
            case "Green":
                return Color.green;
            case "Blue":
                return Color.blue;
            default:
                return Color.black;
        }
    }
}
