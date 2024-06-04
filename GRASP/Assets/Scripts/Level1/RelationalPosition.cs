using UnityEngine;

public class RelationalPosition : Experiment
{
    public Transform spawn;

    public ExperimentComponent ball;

    public override void Reset()
    {
        base.Reset();
        ball.transform.position = spawn.position;
    }
}
