using System.Collections.Generic;
using UnityEngine;

public class MovementFalse : Experiment
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
        float randomLerpFactorX = Random.Range(0f, 1f);
        float randomLerpFactorZ = Random.Range(0f, 1f);
        Vector3 randomPositionX = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactorX);
        Vector3 randomPositionZ = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactorZ);

        ball.transform.position = new Vector3(randomPositionX.x, ball.transform.position.y, randomPositionZ.z);
        ball.SetTarget(new Vector3(randomPositionX.x, ball.transform.position.y, randomPositionZ.z));
    }
}
