using UnityEngine;

public class IP_Gravity : P_Gravity
{
    public float minSpeed;
    public float maxSpeed;
    private Vector3 endPosition;
    private float randomSpeed;

    public override void Reset()
    {
        base.Reset();
        randomSpeed = Random.Range(minSpeed, maxSpeed);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (frameCount >= startDelay)
        {
            MoveToTarget();
        }
    }

    protected virtual void MoveToTarget()
    {
        float step = randomSpeed * Time.fixedDeltaTime;
        ball.transform.position = Vector3.MoveTowards(ball.transform.position, endPosition, step);
    }

    protected override void SpawnBall(int randomIndex)
    {
        float yOffsetStart = boxEdge.transform.localScale.y / 2 + ball.transform.localScale.y / 2;

        float yOffsetEnd = Mathf.Sin(Mathf.Deg2Rad * Mathf.Abs(randomAngle)) * plank.transform.localScale.y / 2 + ball.transform.localScale.y / 2;

        float endPositionXOffset = plank.transform.localScale.y / 2 / Mathf.Cos(Mathf.Deg2Rad * Mathf.Abs(randomAngle));
        if(randomIndex == 0)
        {
            ball.transform.position = new Vector3(boxEdge.transform.position.x - boxEdge.transform.localScale.x / 2 + ball.transform.localScale.x, boxEdge.transform.position.y + yOffsetStart, boxEdge.transform.position.z);
            endPosition = new Vector3(plank.transform.position.x + endPositionXOffset, plank.transform.position.y + yOffsetEnd, plank.transform.position.z);
        } else
        {
            ball.transform.position = new Vector3(boxEdge.transform.position.x + boxEdge.transform.localScale.x / 2 - ball.transform.localScale.x, boxEdge.transform.position.y + yOffsetStart, boxEdge.transform.position.z);
            endPosition = new Vector3(plank.transform.position.x - endPositionXOffset, plank.transform.position.y + yOffsetEnd, plank.transform.position.z);
        }
    }

}
