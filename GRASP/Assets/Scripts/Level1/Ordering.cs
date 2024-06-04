using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ordering : Experiment
{
    public ExperimentComponent cube;
    public ExperimentComponent ball;
    public Transform[] spawns;
    protected List<ExperimentComponent> experimentComponents = new List<ExperimentComponent>();

    private List<string> colorNames = new List<string>
        {
            "Red",
            "Green",
            "Blue",
            "Black",
            "Yellow",
            "Purple",
            "White"
        };

    private string randomColorName;

    private List<string> objInstances;

    public override void Reset()
    {
        base.Reset();
        string label;
        ExperimentComponent newObj;
        while(true)
        {
            label = "";
            DestroyObjects();
            objInstances = new List<string>();
            foreach(Transform spawn in spawns)
            {
                int randomIndex = Random.Range(0, 2);
                if(randomIndex == 0)
                {
                    newObj = Instantiate(ball, spawn.transform.position, spawn.transform.rotation);
                } else
                {
                    newObj = Instantiate(cube, spawn.transform.position, spawn.transform.rotation);
                }
                experimentComponents.Add(newObj);
                Renderer renderer = newObj.GetComponent<Renderer>();
                Color color = GetRandomColor();
                renderer.material.color = color;
                if(randomIndex == 0)
                {
                    label += GetColorName(color) + " ball,";
                    objInstances.Add(GetColorName(color) + " ball");
                } else
                {
                    label += GetColorName(color) + " cube,";
                    objInstances.Add(GetColorName(color) + " cube");
                }
            }

            // there are no duplicates in the list
            if(objInstances.Count == objInstances.Distinct().Count())
            {
                break;
            }
        }
        System.IO.File.WriteAllText(spawns.Count().ToString() + "obj_ordering.txt", label);
    }

    public void DestroyObjects()
    {
        foreach(ExperimentComponent component in experimentComponents)
        {
            Destroy(component.gameObject);
        }

        experimentComponents.Clear();
    }

    Color GetRandomColor()
    {
        // Select a random color name from the list
        randomColorName = colorNames[Random.Range(0, colorNames.Count)];

        // Map the color name to a Color value (you can define this mapping)
        Color color = MapColorNameToColor(randomColorName);

        return color;
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
        else if (color == Color.white)
        {
            return "white";
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
            case "Black":
                return Color.black;
            default:
                return Color.white;
        }
    }
}
