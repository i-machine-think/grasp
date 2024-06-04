using UnityEngine;

public class ExperimentComponent : MonoBehaviour
{
    public bool randomizeColor = true;
    protected Vector3 startingPosition;

    protected virtual void Start()
    {
        startingPosition = transform.position;
    }

    public virtual void Step()
    {
    }

    public virtual void Reset()
    {
        transform.position = startingPosition;
        if(randomizeColor)
        {
            RandomizeColor();
        }
    }

    public virtual void RandomizeColor()
    {   
        Renderer rendererComponent = GetComponent<Renderer>();
        if (rendererComponent)
        {
            float hue = Random.Range(0f, 1f);
            float saturation = Random.Range(0.6f, 1f);
            float lightness = Random.Range(0.6f, 1f);
            Color newColor = Color.HSVToRGB(hue, saturation, lightness);

            rendererComponent.material.color = newColor;
        }
    }
}
