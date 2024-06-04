using UnityEngine;

public class Left : Up
{
    protected override void RandomizeSetup()
    {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(zMin.position, zMax.position, randomLerpFactor);
        ball.transform.position = new Vector3(xMax.position.x, ball.transform.position.y, randomPosition.z);
        ball.SetTarget(new Vector3(xMin.position.x, ball.transform.position.y, randomPosition.z));
    }
}
