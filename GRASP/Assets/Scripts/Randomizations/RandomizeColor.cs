using UnityEngine;

public class RandomizeColor : RandomizeVisuals
{
    public override void Randomize()
    {
        Renderer rendererComponent = GetComponent<Renderer>();
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        rendererComponent.material.color = newColor;
    }
}
