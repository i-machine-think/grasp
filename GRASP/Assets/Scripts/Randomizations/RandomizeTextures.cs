using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class RandomizeTextures : RandomizeVisuals
{
    public override void Randomize()
    {
        Material[] materials = Resources.LoadAll<Material>("Materials/Wooden");
        List<string> materialNames = materials.Select(material => material.name).ToList();

        Material randomMaterial = materials[Random.Range(0, materials.Length)];

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        
        foreach(Renderer renderer in renderers)
        {
            if(materialNames.Contains(renderer.sharedMaterial.name))
            {
                renderer.material = randomMaterial;
            }
        }
    }

    
}
