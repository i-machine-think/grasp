using UnityEngine;

public class P_Unchangeableness2 : Experiment
{
    public MovingExperimentComponent ball;

    public Transform leftSpawn;
    public Transform rightSpawn;

    public GameObject leftCover;
    public GameObject rightCover;

    public override void Reset()
    {
        base.Reset();
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0)
        {
            leftCover.SetActive(true);
            rightCover.SetActive(false);
            ball.transform.position = leftSpawn.position;
            ball.SetTarget(rightSpawn.position);
        } else
        {
            leftCover.SetActive(false);
            rightCover.SetActive(true);
            ball.transform.position = rightSpawn.position;
            ball.SetTarget(leftSpawn.position);
        }
    }
}
