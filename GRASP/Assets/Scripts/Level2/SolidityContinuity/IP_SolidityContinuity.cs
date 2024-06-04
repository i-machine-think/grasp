using UnityEngine;

public class IP_SolidityContinuity : P_SolidityContinuity
{

    protected override void SetupBall(int randomIndex)
    {
        if(randomIndex == 0)
        {
            ball.transform.position = leftBallSpawn.position;
            ball.SetTarget(new Vector3(outerBarrier.transform.position.x - outerBarrier.transform.localScale.x / 2 - ball.transform.localScale.x /2, outerBarrier.transform.position.y - outerBarrier.transform.localScale.y / 2 + ball.transform.localScale.y / 2, outerBarrier.transform.position.z));
        } else
        {
            ball.transform.position = rightBallSpawn.position;
            ball.SetTarget(new Vector3(outerBarrier.transform.position.x + outerBarrier.transform.localScale.x / 2 + ball.transform.localScale.x /2, outerBarrier.transform.position.y - outerBarrier.transform.localScale.y / 2 + ball.transform.localScale.y / 2, outerBarrier.transform.position.z));
        }
    }
}
