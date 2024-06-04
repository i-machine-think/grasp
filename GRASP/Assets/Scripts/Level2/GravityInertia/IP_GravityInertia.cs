using UnityEngine;

public class IP_GravityInertia : Experiment
{

    protected Vector3 targetPosition;
    public Rigidbody cover;
    public GameObject ball;
    public GameObject leftHolder;
    public GameObject rightHolder;
    protected Vector3 leftHolderStartPosition;
    protected Vector3 rightHolderStartPosition;
    protected Vector3 coverStartPosition;
    protected bool uncovered = false;
    protected Vector3 coverEndPosition;
    protected bool targetReached;

    protected override void Start()
    {
        base.Start();
        coverStartPosition = cover.position;
        coverEndPosition = new Vector3(cover.transform.position.x, cover.transform.position.y - cover.transform.localScale.y, cover.transform.position.z);

        leftHolderStartPosition = leftHolder.transform.position;
        rightHolderStartPosition = rightHolder.transform.position;

        targetPosition = new Vector3(ball.transform.position.x, leftHolder.transform.position.y + leftHolder.transform.localScale.y / 2 + ball.transform.localScale.y / 2, ball.transform.position.z);
    }

    public override void Reset()
    {
        base.Reset();
        cover.position = coverStartPosition;
        cover.rotation = Quaternion.Euler(Vector3.zero);
        cover.velocity = Vector3.zero;
        cover.angularVelocity = Vector3.zero;
        uncovered = false;
        RandomizeHolders();

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = true;
        ball.SetActive(false);
    }

    public void RandomizeHolders()
    {
        float holderOffset = Random.Range(-0.2f, 0.2f);

        leftHolder.transform.position = new Vector3(leftHolderStartPosition.x - holderOffset, leftHolderStartPosition.y, leftHolderStartPosition.z);
        rightHolder.transform.position = new Vector3(rightHolderStartPosition.x + holderOffset, rightHolderStartPosition.y, rightHolderStartPosition.z);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (frameCount >= startDelay)
        {
            ball.SetActive(true);
        }
        if (Vector3.Distance(ball.transform.position, targetPosition) <= 0.05f && !uncovered && frameCount >= startDelay)
        {
            if(!targetReached)
            {
                targetReached = true;
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            } else
            {
                DropCover();
            }
        }
    }

    protected void DropCover()
    {
        float step = 0.5f * Time.fixedDeltaTime;
        if (!uncovered)
        {
            cover.position = Vector3.MoveTowards(cover.position, coverEndPosition, step);
        }
        if (Vector3.Distance(cover.position, coverEndPosition) <= 0.1f)
        {
            uncovered = true;
        }
    }

}
