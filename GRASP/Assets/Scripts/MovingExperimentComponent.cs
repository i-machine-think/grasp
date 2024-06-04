using UnityEngine;

public class MovingExperimentComponent : ExperimentComponent
{
    public float minSpeed;
    public float maxSpeed;
    public float distanceMargin;
    protected bool reachedTarget;
    protected Vector3 target;

    protected float randomSpeed;

    protected bool moving = true;

    public override void Step()
    {
        base.Step();
        if(!reachedTarget && moving)
        {
            MoveToTarget();
        }
    }

    public void ResetReachedTarget()
    {
        reachedTarget = false;
    }

    public void StopMoving()
    {
        moving = false;
    }

    public void StartMoving()
    {
        moving = true;
    }

    public Vector3 GetTarget()
    {
        return target;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public bool ReachedTarget() {
        return reachedTarget;
    }

    protected virtual void MoveToTarget()
    {
        float step = randomSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Distance(transform.position, target) <= distanceMargin)
        {
            reachedTarget = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        reachedTarget = false;
        moving = true;
        RandomizeSpeed();
    }

    protected void RandomizeSpeed()
    {
        randomSpeed = Random.Range(minSpeed, maxSpeed);
    }

}
