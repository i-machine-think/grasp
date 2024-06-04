using UnityEngine;

public class P_ObjectPermanence : Experiment
{
    public Transform leftBallSpawn;
    public Transform rightBallSpawn;
    public GameObject leftCover;
    public GameObject rightCover;
    public MovingExperimentComponent ball;

    public override void Reset()
    {
        base.Reset();
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0)
        {
            ball.transform.position = leftBallSpawn.position;
            ball.SetTarget(rightBallSpawn.position);
            leftCover.SetActive(true);
            rightCover.SetActive(false);
        } else 
        {
            ball.transform.position = rightBallSpawn.position;
            ball.SetTarget(leftBallSpawn.position);
            leftCover.SetActive(false);
            rightCover.SetActive(true);
        }
    }
}
