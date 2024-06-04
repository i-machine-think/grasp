using UnityEngine;

public class P_Inertia2 : Experiment
{

    public Transform ballSpawn1;
    public Transform ballSpawn2;
    public Transform teleportation1;
    public Transform teleportation2;
    public GameObject cover;

    public Transform target;

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
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        cover.transform.SetPositionAndRotation(coverStartPosition, coverStartRotation);
        Rigidbody rbCover = cover.GetComponent<Rigidbody>();
        rbCover.velocity = Vector3.zero;
        rbCover.angularVelocity = Vector3.zero;
        uncovered = false;
        RandomizeBallSpawn();
        ball.SetTarget(new Vector3(target.position.x, target.position.y, transform.position.z));
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
