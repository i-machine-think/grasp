using UnityEngine;

public class P_ObjectPermanence3 : Experiment
{
    public GameObject box;

    public GameObject plank;
    public GameObject platform;
    public float minSpeed;
    public float maxSpeed;
    public float minBoxHeight;
    public float maxBoxHeight;
    public float boxMaxXOffset;
    protected float currSpeed;
    protected HingeJoint hinge;
    private Quaternion startRotation;
    private bool hidden;
    private float previousAngle;
    private float rotationSign;
    private Vector3 boxStartPosition;

    protected override void Start()
    {
        base.Start();
        startRotation = plank.transform.rotation;
        hinge = plank.GetComponent<HingeJoint>();
        boxStartPosition = box.transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(frameCount > startDelay)
        {
            JointMotor motor = hinge.motor;
            motor.force = 100f;
            motor.targetVelocity = rotationSign * currSpeed;
            hinge.motor = motor;

            if(Mathf.Approximately(hinge.angle, previousAngle) && !hidden) {
                hidden = true;
                rotationSign = -1f;
            }

            previousAngle = hinge.angle;
        }

    }

    public override void Reset()
    {
        base.Reset();
        JointMotor motor = hinge.motor;
        motor.force = 0f;
        motor.targetVelocity = 0f;
        hinge.motor = motor;
        plank.transform.rotation = startRotation;
        hidden = false;
        previousAngle = 1000f;
        rotationSign = 1f;
        currSpeed = Random.Range(minSpeed, maxSpeed);
        float height = Random.Range(minBoxHeight, maxBoxHeight);
        box.transform.localScale = new Vector3(box.transform.localScale.x, height, box.transform.localScale.z);
        float offset = Random.Range(-boxMaxXOffset, boxMaxXOffset);
        box.transform.position = new Vector3(boxStartPosition.x + offset, platform.transform.position.y + platform.transform.localScale.y / 2 + height / 2, boxStartPosition.z);
    }
}
