using UnityEngine;

public class P_Inertia : Experiment
{
    public Transform ballSpawn1;
    public Transform ballSpawn2;
    public MovingExperimentComponent ball;
    public GameObject barrier;

    protected Vector3 reflectionTarget;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ball.ReachedTarget())
        {
            ball.SetTarget(reflectionTarget);
            ball.ResetReachedTarget();
        }
    }

    public override void Reset()
    {
        base.Reset();
        RandomizeBallSpawn();
        ball.SetTarget(new Vector3(barrier.transform.position.x + barrier.transform.localScale.z / 2 + ball.transform.localScale.x / 2, ball.transform.position.y, barrier.transform.position.z));
    }

    public virtual void RandomizeBallSpawn() {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, randomLerpFactor);

        ball.transform.position = randomPosition;

        reflectionTarget = new Vector3(ball.transform.position.x, ball.transform.position.y, Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, 1f - randomLerpFactor).z);

    }
}
