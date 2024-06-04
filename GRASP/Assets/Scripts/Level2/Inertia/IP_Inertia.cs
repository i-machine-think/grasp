using UnityEngine;

public class IP_Inertia : P_Inertia
{
    public override void RandomizeBallSpawn() {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, randomLerpFactor);

        ball.transform.position = randomPosition;

        if(randomLerpFactor < 0.5f)
        {
            reflectionTarget = new Vector3(ball.transform.position.x, ball.transform.position.y, Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, Random.Range(0f, 0.5f)).z);
        } else
        {
            reflectionTarget = new Vector3(ball.transform.position.x, ball.transform.position.y, Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, Random.Range(0.5f, 1f)).z);
        }
    }
}
