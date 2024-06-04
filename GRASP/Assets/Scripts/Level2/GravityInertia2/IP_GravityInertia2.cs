using UnityEngine;

public class IP_GravityInertia2 : P_GravityInertia2
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        bool leftBlockCondition = leftBlock.activeSelf && (ball.transform.position.x > leftBlock.transform.position.x + leftBlock.transform.localScale.x/2 + ball.transform.localScale.x/2 + 0.02f);
        bool rightBlockCondition = rightBlock.activeSelf && (ball.transform.position.x < rightBlock.transform.position.x - rightBlock.transform.localScale.x/2 - ball.transform.localScale.x/2 - 0.02f);
        if(!targetMoved && (leftBlockCondition || rightBlockCondition))
        {
            int randomIndex = Random.Range(0, 2);
            if(randomIndex == 0)
            {
                ball.StopMoving();
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = true;
            } 
            targetMoved = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        targetMoved = false;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected override int RandomizeBlock()
    {
        int randomIndex = base.RandomizeBlock();
        if(randomIndex == 0)
        {
            ball.SetTarget(new Vector3(rightTarget.position.x, ball.transform.position.y, rightTarget.position.z));
        } else 
        {
            ball.SetTarget(new Vector3(leftTarget.position.x, ball.transform.position.y, leftTarget.position.z));
        }

        return randomIndex;
    }
}
