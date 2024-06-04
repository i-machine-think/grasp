using UnityEngine;

public class P_GravitySupport : Experiment
{
    public MovingExperimentComponent stick;
    public GameObject plank;
    public Transform topBlockRightSpawn;
    public Transform topBlockLeftSpawn;
    public GameObject bottomBlock;
    public GameObject topBlock;
    public GameObject leftSetup;
    public GameObject rightSetup;
    public float blockMinSize;
    public float blockMaxSize;
    public Transform RightStickSpawn;
    public Transform LeftStickSpawn;
    private Vector3 stickStartingPosition;
    private Quaternion stickStartRotation;

    private bool returnedToOrigin;

    protected override void Start()
    {
        base.Start();
        stickStartRotation = stick.transform.rotation;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody topBlockRb = stick.GetComponent<Rigidbody>();
        if (stick.ReachedTarget() && !returnedToOrigin)
        {
            stick.ResetReachedTarget();
            stick.SetTarget(stickStartingPosition);
            returnedToOrigin = true;
        }
        Rigidbody stickRb = stick.GetComponent<Rigidbody>();
        stickRb.velocity = Vector3.zero;
        stickRb.angularVelocity = Vector3.zero;
        stick.transform.rotation = stickStartRotation;
    }

    public override void Reset()
    {
        base.Reset();
        int randomIndex = Random.Range(0, 2);
        
        returnedToOrigin = false;
        SpawnBottomBlock();
        SpawnTopBlock(randomIndex);
        SampleTarget(randomIndex);

        if(randomIndex == 0)
        {
            rightSetup.SetActive(true);
            leftSetup.SetActive(false);
            stick.transform.position = new Vector3(RightStickSpawn.position.x, topBlock.transform.position.y, topBlock.transform.position.z);
        } else
        {
            rightSetup.SetActive(false);
            leftSetup.SetActive(true);
            stick.transform.position = new Vector3(LeftStickSpawn.position.x, topBlock.transform.position.y, topBlock.transform.position.z);
        }
        stickStartingPosition = stick.transform.position;
    }

    public void SpawnBottomBlock()
    {
        float randomSize = Random.Range(blockMinSize, blockMaxSize);
        bottomBlock.transform.localScale = new Vector3(bottomBlock.transform.localScale.x, randomSize, randomSize);
        bottomBlock.transform.position = new Vector3(bottomBlock.transform.position.x, plank.transform.position.y + plank.transform.localScale.y / 2 + bottomBlock.transform.localScale.y / 2, bottomBlock.transform.position.z);
    }

    public void SpawnTopBlock(int randomIndex)
    {
        topBlock.transform.localScale = new Vector3(bottomBlock.transform.localScale.y, bottomBlock.transform.localScale.y, bottomBlock.transform.localScale.y);

        if(randomIndex == 0)
        {
            topBlock.transform.position = new Vector3(topBlockRightSpawn.position.x, bottomBlock.transform.position.y + bottomBlock.transform.localScale.y / 2 + topBlock.transform.localScale.y / 2, topBlockRightSpawn.position.z);
        } else
        {
            topBlock.transform.position = new Vector3(topBlockLeftSpawn.position.x, bottomBlock.transform.position.y + bottomBlock.transform.localScale.y / 2 + topBlock.transform.localScale.y / 2, topBlockLeftSpawn.position.z);
        }
    }

    public virtual void SampleTarget(int randomIndex)
    {
        float randomPoint;
        if(randomIndex == 0)
        {
            randomPoint = Random.Range(
                bottomBlock.transform.position.x - bottomBlock.transform.localScale.x / 2 + stick.transform.localScale.x / 2 + topBlock.transform.localScale.x / 2,
                bottomBlock.transform.position.x + stick.transform.localScale.x / 2 
            );
        } else
        {
            randomPoint = Random.Range(
                bottomBlock.transform.position.x - stick.transform.localScale.x / 2, 
                bottomBlock.transform.position.x + bottomBlock.transform.localScale.x / 2 - stick.transform.localScale.x / 2 - topBlock.transform.localScale.x / 2
            );
        }
        stick.SetTarget(new Vector3(randomPoint, topBlock.transform.position.y, topBlock.transform.position.z));
    }
}
