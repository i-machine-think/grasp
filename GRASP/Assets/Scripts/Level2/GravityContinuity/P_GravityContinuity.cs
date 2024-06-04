using UnityEngine;

public class P_GravityContinuity : Experiment
{
    public GameObject leftCover;
    public GameObject rightCover;
    public Transform ballSpawnLeft;
    public Transform ballSpawnRight;
    public Transform ballEndPositionLeft;
    public Transform ballEndPositionRight;
    public GameObject leftSetup;
    public GameObject rightSetup;
    public MovingExperimentComponent ball;
    protected GameObject activeCover;
    protected Vector3 coverEndPosition;
    protected Vector3 leftCoverStartPosition;
    protected Vector3 rightCoverStartPosition;

    protected bool uncovered = false;

    protected override void Start()
    {
        base.Start();
        leftCoverStartPosition = leftCover.transform.position;
        rightCoverStartPosition = rightCover.transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ball.ReachedTarget() && !uncovered)
        {
            DropCover();
        }
    }

    public override void Reset()
    {
        base.Reset();
        int randomIndex = RandomizeSetup();
        if(randomIndex == 0)
        {
            ball.transform.position = ballSpawnLeft.position;
            ball.SetTarget(ballEndPositionRight.position);
        } else 
        {
            ball.transform.position = ballSpawnRight.position;
            ball.SetTarget(ballEndPositionLeft.position);
        }
        uncovered = false;
    }

    public int RandomizeSetup()
    {
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0)
        {
            leftCover.SetActive(false);
            rightCover.SetActive(true);
            leftSetup.SetActive(true);
            rightSetup.SetActive(false);
            rightCover.transform.position = rightCoverStartPosition;
            activeCover = rightCover;
        } else
        {
            rightCover.SetActive(false);
            leftCover.SetActive(true);
            leftSetup.SetActive(false);
            rightSetup.SetActive(true);
            leftCover.transform.position = leftCoverStartPosition;
            activeCover = leftCover;
        }

        activeCover.transform.rotation = Quaternion.Euler(Vector3.zero);
        Rigidbody rb = activeCover.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        coverEndPosition = new Vector3(activeCover.transform.position.x, activeCover.transform.position.y - activeCover.transform.localScale.y, activeCover.transform.position.z);

        return randomIndex;
    }

    protected void DropCover()
    {
        float step = 0.5f * Time.fixedDeltaTime;
        if (!uncovered)
        {
            activeCover.transform.position = Vector3.MoveTowards(activeCover.transform.position, coverEndPosition, step);
        }
        if (Vector3.Distance(activeCover.transform.position, coverEndPosition) <= 0.01f)
        {
            uncovered = true;
        }
    }
}
