using UnityEngine;

public class IP_Continuity : P_Continuity
{
    private GameObject activeBarrier;

    private bool teleported;

    protected override void Start()
    {
        base.Start();
        activeBarrier = barrier1;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(Vector3.Distance(ball.transform.position, activeBarrier.transform.position) <= 0.2 && !teleported) {
            ball.transform.position = ball.GetTarget();
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            teleported = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        float distance1 = Vector3.Distance(ball.transform.position, endTarget1.position);
        float distance2 = Vector3.Distance(ball.transform.position, endTarget2.position);

        if(distance1 > distance2) {
            barrier1.SetActive(true);
            barrier2.SetActive(false);
            activeBarrier = barrier1;
            ball.SetTarget(new Vector3(endTarget1.position.x, endTarget1.position.y, endTarget1.position.z));
            
        } else {
            barrier1.SetActive(false);
            barrier2.SetActive(true);
            activeBarrier = barrier2;
            ball.SetTarget(new Vector3(endTarget2.position.x, endTarget2.position.y, endTarget2.position.z));
        }
        teleported = false;
    }
}
