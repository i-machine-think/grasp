using UnityEngine;

public class P_GravityInertia2 : Experiment
{

    public GameObject leftBlock;
    public GameObject rightBlock;

    public Transform leftTarget;

    public Transform rightTarget;

    public MovingExperimentComponent ball;

    private Vector3 initialLeftBlockScale;
    private Vector3 initialRightBlockScale;
    private Vector3 initialLeftBlockPosition;
    private Vector3 initialRightBlockPosition;
    protected bool targetMoved;

    protected override void Start()
    {
        base.Start();
        initialLeftBlockScale = leftBlock.transform.localScale;
        initialLeftBlockPosition = leftBlock.transform.position;
        initialRightBlockScale = rightBlock.transform.localScale;
        initialRightBlockPosition = rightBlock.transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!targetMoved && (ball.transform.position.y <= rightTarget.position.y + 0.1))
        {
            ball.SetTarget(new Vector3(ball.GetTarget().x, rightTarget.position.y, rightTarget.position.z));
            targetMoved = true;
        }
    }


    public override void Reset()
    {
        base.Reset();
        RandomizeBlock();
    }

    protected virtual int RandomizeBlock()
    {
        float randomExtraLength = Random.Range(0f, 0.7f);
        float randomExtraHeight = Random.Range(0f, 1f);
        int randomIndex = Random.Range(0, 2);

        if(randomIndex == 0)
        {
            leftBlock.SetActive(true);
            rightBlock.SetActive(false);
            leftBlock.transform.localScale = new Vector3(initialLeftBlockScale.x + randomExtraLength, initialLeftBlockScale.y + randomExtraHeight, initialLeftBlockScale.z);
            leftBlock.transform.position = new Vector3(initialLeftBlockPosition.x + randomExtraLength/2, initialLeftBlockPosition.y + randomExtraHeight/2, initialLeftBlockPosition.z);
            ball.transform.position = new Vector3(leftBlock.transform.position.x - leftBlock.transform.localScale.x / 2 + ball.transform.localScale.x /2, leftBlock.transform.position.y + leftBlock.transform.localScale.y / 2 + ball.transform.localScale.y / 2, ball.transform.position.z);
            ball.SetTarget(new Vector3(rightTarget.position.x, ball.transform.position.y, ball.transform.position.z));
        } else
        {
            leftBlock.SetActive(false);
            rightBlock.SetActive(true);
            rightBlock.transform.localScale = new Vector3(initialRightBlockScale.x + randomExtraLength, initialRightBlockScale.y + randomExtraHeight, initialRightBlockScale.z);
            rightBlock.transform.position = new Vector3(initialRightBlockPosition.x - randomExtraLength / 2, initialRightBlockPosition.y + randomExtraHeight/2, initialRightBlockPosition.z);
            ball.transform.position = new Vector3(rightBlock.transform.position.x + rightBlock.transform.localScale.x / 2 - ball.transform.localScale.x /2, rightBlock.transform.position.y + rightBlock.transform.localScale.y / 2 + ball.transform.localScale.y / 2, ball.transform.position.z);
            ball.SetTarget(new Vector3(leftTarget.position.x, ball.transform.position.y, ball.transform.position.z));
        }

        return randomIndex;
    }
}
