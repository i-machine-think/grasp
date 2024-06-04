using UnityEngine;

public class P_GravityInertia : Experiment
{
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
    protected bool active;

    protected override void Start()
    {
        base.Start();
        coverStartPosition = cover.position;
        coverEndPosition = new Vector3(cover.transform.position.x, cover.transform.position.y - cover.transform.localScale.y, cover.transform.position.z);

        leftHolderStartPosition = leftHolder.transform.position;
        rightHolderStartPosition = rightHolder.transform.position;
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
        active = false;
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
        if (!active && frameCount >= startDelay)
        {
            ball.SetActive(true);
            active = true;
            return;
        }
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (Mathf.Abs(rb.velocity.y) <= 0.1f && !uncovered && frameCount >= startDelay)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            DropCover();
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
