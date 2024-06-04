using UnityEngine;

public class Down : Up
{
    protected override void RandomizeSetup()
    {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(xMin.position, xMax.position, randomLerpFactor);
        ball.transform.position = new Vector3(randomPosition.x, ball.transform.position.y, zMax.position.z);
        ball.SetTarget(new Vector3(randomPosition.x, ball.transform.position.y, zMin.position.z));
    }
}
