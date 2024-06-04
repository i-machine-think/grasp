using UnityEngine;

public class SolidityContinuity2 : Experiment
{

    protected Vector3 targetPosition;
    public Rigidbody cover;
    public GameObject ball;
    public GameObject leftPlatform;
    public GameObject rightPlatform;

    public GameObject leftHolder;
    public GameObject rightHolder;
    protected Vector3 leftHolderStartPosition;
    protected Vector3 rightHolderStartPosition;
    protected Vector3 coverStartPosition;
    protected bool uncovered = false;
    protected Vector3 coverEndPosition;

    protected Transform leftPlatformStartTransform;
    protected Transform rightPlatformStartTransform;
    protected bool active = false;

    protected override void Start()
    {
        base.Start();
        coverStartPosition = cover.position;
        coverEndPosition = new Vector3(cover.transform.position.x, cover.transform.position.y - cover.transform.localScale.y, cover.transform.position.z);

        leftPlatformStartTransform = new GameObject("LeftPlatformStartTransform").transform;
        leftPlatformStartTransform.SetPositionAndRotation(leftPlatform.transform.position, leftPlatform.transform.rotation);
        leftPlatformStartTransform.localScale = leftPlatform.transform.localScale;

        rightPlatformStartTransform = new GameObject("RightPlatformStartTransform").transform;
        rightPlatformStartTransform.SetPositionAndRotation(rightPlatform.transform.position, rightPlatform.transform.rotation);
        rightPlatformStartTransform.localScale = rightPlatform.transform.localScale;

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
        leftPlatform.transform.SetPositionAndRotation(leftPlatformStartTransform.position, leftPlatformStartTransform.rotation);
        rightPlatform.transform.SetPositionAndRotation(rightPlatformStartTransform.position, rightPlatformStartTransform.rotation);
        leftPlatform.transform.localScale = leftPlatformStartTransform.localScale;
        rightPlatform.transform.localScale = rightPlatformStartTransform.localScale;
        RandomizeSetup();
        RandomizeHolders();
        ball.SetActive(false);
        active = false;
    }

    public void RandomizeHolders()
    {
        float holderOffset = Random.Range(-0.2f, 0.2f);

        leftHolder.transform.position = new Vector3(leftHolderStartPosition.x - holderOffset, leftHolderStartPosition.y, leftHolderStartPosition.z);
        rightHolder.transform.position = new Vector3(rightHolderStartPosition.x + holderOffset, rightHolderStartPosition.y, rightHolderStartPosition.z);
    }

    public void RandomizeSetup()
    {
        float gapSize = Random.Range(0f, ball.transform.localScale.x - 0.05f);

        leftPlatform.transform.localScale = new Vector3(leftPlatform.transform.localScale.x - gapSize / 2, leftPlatform.transform.localScale.y, leftPlatform.transform.localScale.z);
        rightPlatform.transform.localScale = new Vector3(rightPlatform.transform.localScale.x - gapSize / 2, rightPlatform.transform.localScale.y, rightPlatform.transform.localScale.z);

        leftPlatform.transform.position = new Vector3(leftPlatform.transform.position.x - gapSize / 4, leftPlatform.transform.position.y, leftPlatform.transform.position.z);
        rightPlatform.transform.position = new Vector3(rightPlatform.transform.position.x + gapSize / 4, rightPlatform.transform.position.y, rightPlatform.transform.position.z);
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
