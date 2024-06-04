using UnityEngine;

public class P_Continuity : Experiment
{
    public Transform ballSpawn1;
    public Transform ballSpawn2;
    public Transform endTarget1;
    public Transform endTarget2;
    public GameObject cover;
    public GameObject barrier1;
    public GameObject barrier2;
    public MovingExperimentComponent ball;

    protected Vector3 coverEndPosition;
    protected Vector3 coverStartPosition;
    protected Quaternion coverStartRotation;
    protected bool uncovered = false;

    protected override void Start()
    {
        base.Start();
        coverStartPosition = cover.transform.position;
        coverStartRotation = cover.transform.rotation;
        coverEndPosition = new Vector3(cover.transform.position.x - cover.transform.localScale.z, cover.transform.position.y, cover.transform.position.z);
        RandomizeBallSpawn();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ball.ReachedTarget() && !uncovered)
        {
            MoveCover();
        }
    }

    public override void Reset()
    {
        base.Reset();
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0) {
            barrier1.SetActive(false);
            barrier2.SetActive(true);
            ball.SetTarget(new Vector3(endTarget1.position.x, endTarget1.position.y, endTarget1.position.z));
        } else {
            barrier1.SetActive(true);
            barrier2.SetActive(false);
            ball.SetTarget(new Vector3(endTarget2.position.x, endTarget2.position.y, endTarget2.position.z));
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        cover.transform.SetPositionAndRotation(coverStartPosition, coverStartRotation);
        Rigidbody coverRb = cover.GetComponent<Rigidbody>();
        coverRb.velocity = Vector3.zero;
        coverRb.angularVelocity = Vector3.zero;
        uncovered = false;
        RandomizeBallSpawn();
    }

    public void RandomizeBallSpawn() {
        float randomLerpFactor = Random.Range(0f, 1f);
        Vector3 randomPosition = Vector3.Lerp(ballSpawn1.position, ballSpawn2.position, randomLerpFactor);

        ball.transform.position = randomPosition;
    }

    protected virtual void MoveCover()
    {
        float step = 0.5f * Time.fixedDeltaTime;
        if (!uncovered)
        {
            cover.transform.position = Vector3.MoveTowards(cover.transform.position, coverEndPosition, step);
        }
        if (Vector3.Distance(cover.transform.position, coverEndPosition) <= 0.01f)
        {
            uncovered = true;
        }
    }
}
