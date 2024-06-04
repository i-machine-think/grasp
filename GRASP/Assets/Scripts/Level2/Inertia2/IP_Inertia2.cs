using UnityEngine;

public class IP_Inertia2 : P_Inertia2
{
    private bool teleported;

    protected override void MoveCover()
    {
        if(!teleported) {
            TeleportBall();
            teleported = true;
        }
        base.MoveCover();
    }

    private void TeleportBall() {
        float distance1 = Vector3.Distance(ball.transform.position, teleportation1.position);
        float distance2 = Vector3.Distance(ball.transform.position, teleportation2.position);

        if(distance1 > distance2) {
            ball.transform.position = teleportation1.position;
        } else {
            ball.transform.position = teleportation2.position;
        }
    }

    public override void Reset()
    {
        base.Reset();
        teleported = false;
    }
}
