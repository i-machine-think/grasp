using UnityEngine;

public class IP_GravityContinuity : P_GravityContinuity
{
    protected bool teleported;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ball.ReachedTarget() && !teleported)
        {
            if(leftCover.activeSelf)
            {
                ball.transform.position = ballSpawnLeft.position;
            } else 
            {
                ball.transform.position = ballSpawnRight.position;
            }
            teleported = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        teleported = false;
    }

}
