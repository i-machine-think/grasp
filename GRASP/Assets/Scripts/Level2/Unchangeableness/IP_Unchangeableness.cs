using UnityEngine;

public class IP_Unchangeableness : P_Unchangeableness
{
    protected bool changed;
    protected override void ReverseCoverMovement()
    {
        base.ReverseCoverMovement();
        while(!changed)
        {
            foreach(ExperimentComponent component in experimentComponents)
            {
                int randomIndex = Random.Range(0, 3);
                if(randomIndex == 0)    // change color
                {
                    component.RandomizeColor();
                    Renderer renderer = component.GetComponent<Renderer>();
                    renderer.material.color = new Color(255f, 255f, 255f);
                    changed = true;
                    break;
                } else if(randomIndex == 1) // change shape
                {
                    if(IsClone(component.gameObject, ball.gameObject)) {
                        ExperimentComponent newObj = Instantiate(cube, component.transform.position, component.transform.rotation);

                        Renderer componentRenderer = component.GetComponent<Renderer>();
                        Renderer newObjRenderer = newObj.GetComponent<Renderer>();
                        newObjRenderer.material.color = componentRenderer.material.color;

                        experimentComponents.Add(newObj);
                        component.gameObject.SetActive(false);
                    }
                    changed = true;
                    break;
                }
                // otherwise leave object unchanged
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        changed = false;
    }

    bool IsClone(GameObject obj, GameObject original)
    {
        // Check if the object is a clone by comparing their instance IDs
        return !Object.ReferenceEquals(obj, original) && obj.GetInstanceID() != original.GetInstanceID();
    }
}
