using UnityEngine;



public class Experiment : MonoBehaviour
{
    public float minDelay;
    public float maxDelay;

    public ExperimentComponent[] components;
    public RandomizeVisuals[] randomizationObjs;
    protected float frameCount;

    protected float startDelay;

    protected virtual void Start()
    {
    }

    protected virtual void FixedUpdate()
    {
        frameCount++;
        if(frameCount > startDelay)
        {
            foreach(ExperimentComponent component in components)
            {
                component.Step();
            }
        }
    }

    public virtual void Reset()
    {
        frameCount = 0;
        RandomizeStartDelay();
        foreach(ExperimentComponent component in components)
        {
            component.Reset();
        }

        foreach(RandomizeVisuals obj in randomizationObjs)
        {
            obj.Randomize();
        }
    }

    protected virtual void RandomizeStartDelay()
    {
        startDelay = Random.Range(minDelay, maxDelay);
    }
}
