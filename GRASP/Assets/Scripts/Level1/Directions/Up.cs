using System.Collections.Generic;
using UnityEngine;

public class Up : Experiment
{
    public Transform zMin;
    public Transform zMax;

    public Transform xMin;

    public Transform xMax;

    public MovingExperimentComponent ball;

    // Update is called once per frame
    public override void Reset()
    {
        base.Reset();
        RandomizeSetup();
    }

    protected virtual void RandomizeSetup()
    {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor);
        ball.transform.position = new Vector3(randomPosition.x, ball.transform.position.y, zMin.position.z);
        ball.SetTarget(new Vector3(randomPosition.x, ball.transform.position.y, zMax.position.z));
    }
}
