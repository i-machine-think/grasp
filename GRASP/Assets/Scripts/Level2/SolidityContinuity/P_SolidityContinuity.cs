using UnityEngine;

public class P_SolidityContinuity : Experiment
{
    public MovingExperimentComponent ball;
    public GameObject rightCover;

    public GameObject leftCover;

    public GameObject innerBarrier;
    public GameObject outerBarrier;

    public GameObject leftStartBox;
    public GameObject rightStartBox;

    public Transform leftBallSpawn;

    public Transform rightBallSpawn;

    protected GameObject activeCover;

    protected Vector3 coverEndPosition;

    public Transform outerBarrierLeftSpawn;
    public Transform outerBarrierRightSpawn;
    public Transform innerBarrierLeftSpawn;
    public Transform innerBarrierRightSpawn;

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
        SetupCover(randomIndex);
        SetupBall(randomIndex);
        uncovered = false;
    }

    protected virtual void SetupBall(int randomIndex)
    {
        if(randomIndex == 0)
        {
            ball.transform.position = leftBallSpawn.position;
            ball.SetTarget(new Vector3(innerBarrier.transform.position.x - innerBarrier.transform.localScale.x / 2 - ball.transform.localScale.x /2, innerBarrier.transform.position.y - innerBarrier.transform.localScale.y / 2 + ball.transform.localScale.y / 2, innerBarrier.transform.position.z));
        } else
        {
            ball.transform.position = rightBallSpawn.position;
            ball.SetTarget(new Vector3(innerBarrier.transform.position.x + innerBarrier.transform.localScale.x / 2 + ball.transform.localScale.x /2, innerBarrier.transform.position.y - innerBarrier.transform.localScale.y / 2 + ball.transform.localScale.y / 2, innerBarrier.transform.position.z));
        }
    }

    protected void SetupCover(int randomIndex)
    {
        if(randomIndex == 0)
        {
            leftCover.SetActive(false);
            rightCover.SetActive(true);
            rightCover.transform.position = rightCoverStartPosition;
            activeCover = rightCover;
        } else
        {
            rightCover.SetActive(false);
            leftCover.SetActive(true);
            leftCover.transform.position = leftCoverStartPosition;
            activeCover = leftCover;
        }

        activeCover.transform.rotation = Quaternion.Euler(Vector3.zero);
        Rigidbody rb = activeCover.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        coverEndPosition = new Vector3(activeCover.transform.position.x, activeCover.transform.position.y - activeCover.transform.localScale.y, activeCover.transform.position.z);
    }

    protected int RandomizeSetup()
    {
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0)
        {
            leftStartBox.SetActive(true);
            rightStartBox.SetActive(false);
        } else
        {
            leftStartBox.SetActive(false);
            rightStartBox.SetActive(true);
        }

        RandomizeBarriers(randomIndex);

        return randomIndex;
    }

    protected void RandomizeBarriers(int randomIndex)
    {
        float innerBarrierOffset = Random.Range(-0.4f, 0.4f);
        float outerBarrierOffset = Random.Range(-1f, 0f);
        if(randomIndex == 0)
        {
            innerBarrier.transform.position = new Vector3(innerBarrierRightSpawn.position.x + innerBarrierOffset, innerBarrierRightSpawn.position.y, innerBarrierRightSpawn.position.z);
            outerBarrier.transform.position = new Vector3(outerBarrierRightSpawn.position.x + outerBarrierOffset, outerBarrierRightSpawn.position.y, outerBarrierRightSpawn.position.z);
        } else 
        {
            innerBarrier.transform.position = new Vector3(innerBarrierLeftSpawn.position.x - innerBarrierOffset, innerBarrierLeftSpawn.position.y, innerBarrierLeftSpawn.position.z);
            outerBarrier.transform.position = new Vector3(outerBarrierLeftSpawn.position.x - outerBarrierOffset, outerBarrierLeftSpawn.position.y, outerBarrierLeftSpawn.position.z);
        }
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
