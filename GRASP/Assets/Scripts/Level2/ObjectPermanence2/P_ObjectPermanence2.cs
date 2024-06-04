using UnityEngine;

public class P_ObjectPermanence2 : Experiment
{
    public MovingExperimentComponent innerCover;
    public MovingExperimentComponent outerCover;

    public GameObject leftSetup;
    public GameObject rightSetup;

    public Transform innerCoverLeftSpawn;
    public Transform innerCoverRightSpawn;
    public Transform outerCoverLeftSpawn;
    public Transform outerCoverRightSpawn;

    public Transform stickLeftSpawn;
    public Transform stickRightSpawn;

    public MovingExperimentComponent stick;
    public ExperimentComponent cube;

    protected bool hidden;

    protected bool reachedBox;
    protected bool reachedOrigin;
    protected Vector3 stickStartPosition;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!hidden && innerCover.ReachedTarget()) {
            stick.StartMoving();
            hidden = true;
        } else if(!reachedBox && stick.ReachedTarget())
        {
            stick.SetTarget(stickStartPosition);
            stick.ResetReachedTarget();
            reachedBox = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        reachedBox = false;
        hidden = false;
        reachedOrigin = false;
        stick.StopMoving();
        RandomizeSetup();
    }

    public virtual int RandomizeSetup()
    {
        int randomIndex = Random.Range(0, 2);
        if(randomIndex == 0)
        {
            innerCover.transform.position = innerCoverLeftSpawn.position;
            outerCover.transform.position = outerCoverLeftSpawn.position;

            stick.transform.position = stickRightSpawn.position;
            stickStartPosition = stickRightSpawn.position;

            innerCover.SetTarget(new Vector3(innerCover.transform.position.x + 1f, innerCover.transform.position.y, innerCover.transform.position.z));
            outerCover.SetTarget(new Vector3(outerCover.transform.position.x + 1f, outerCover.transform.position.y, outerCover.transform.position.z));

            cube.transform.position = new Vector3(innerCover.transform.position.x + 1f, cube.transform.position.y, cube.transform.position.z);

            float betweenCovers = (outerCover.transform.position + innerCover.transform.position).x / 2f + 1f;
            
            stick.SetTarget(new Vector3(betweenCovers + cube.transform.localScale.x / 2 + stick.transform.localScale.x / 2, cube.transform.position.y, cube.transform.position.z));
            leftSetup.SetActive(false);
            rightSetup.SetActive(true);
        } else
        {
            innerCover.transform.position = innerCoverRightSpawn.position;
            outerCover.transform.position = outerCoverRightSpawn.position;

            stick.transform.position = stickLeftSpawn.position;
            stickStartPosition = stickLeftSpawn.position;

            innerCover.SetTarget(new Vector3(innerCover.transform.position.x - 1f, innerCover.transform.position.y, innerCover.transform.position.z));
            outerCover.SetTarget(new Vector3(outerCover.transform.position.x - 1f, outerCover.transform.position.y, outerCover.transform.position.z));

            cube.transform.position = new Vector3(innerCover.transform.position.x - 1f, cube.transform.position.y, cube.transform.position.z);

            float betweenCovers = (innerCover.transform.position + outerCover.transform.position).x / 2f - 1f;
            stick.SetTarget(new Vector3(betweenCovers - cube.transform.localScale.x / 2 - stick.transform.localScale.x / 2, cube.transform.position.y, cube.transform.position.z));
            leftSetup.SetActive(true);
            rightSetup.SetActive(false);
        }

        return randomIndex;
    }
}
