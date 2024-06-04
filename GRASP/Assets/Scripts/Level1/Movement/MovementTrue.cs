using System.Collections.Generic;
using UnityEngine;

public class MovementTrue : Experiment
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
        float randomLerpFactor1 = Random.Range(0f, 1f);
        float randomLerpFactor2 = Random.Range(0f, 1f);

        int randomIndex = Random.Range(0, 4);
        if(randomIndex == 0)
        {
            Vector3 randomPosition1 = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor1);
            ball.transform.position = new Vector3(randomPosition1.x, ball.transform.position.y, zMin.position.z);

            Vector3 randomPosition2 = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor2);
            ball.SetTarget(new Vector3(randomPosition2.x, ball.transform.position.y, zMax.position.z));
        } else if(randomIndex == 1)
        {
            Vector3 randomPosition1 = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor1);
            ball.transform.position = new Vector3(randomPosition1.x, ball.transform.position.y, zMax.position.z);

            Vector3 randomPosition2 = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor2);
            ball.SetTarget(new Vector3(randomPosition2.x, ball.transform.position.y, zMin.position.z));
        } else if(randomIndex == 2)
        {
            Vector3 randomPosition1 = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactor1);
            ball.transform.position = new Vector3(xMin.position.x, ball.transform.position.y, randomPosition1.z);

            Vector3 randomPosition2 = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactor2);
            ball.SetTarget(new Vector3(xMax.position.x, ball.transform.position.y, randomPosition2.z));
        } else
        {
            Vector3 randomPosition1 = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactor1);
            ball.transform.position = new Vector3(xMax.position.x, ball.transform.position.y, randomPosition1.z);

            Vector3 randomPosition2 = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactor2);
            ball.SetTarget(new Vector3(xMin.position.x, ball.transform.position.y, randomPosition2.z));
        }
    }
}
