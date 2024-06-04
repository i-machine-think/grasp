using UnityEngine;
using Unity.MLAgents;

public class ObservingAgent : Agent
{
    public Experiment experiment;
    public float minXCameraOffset;
    public float maxXCameraOffset;
    public float minYCameraOffset;
    public float maxYCameraOffset;
    public float minZCameraOffset;
    public float maxZCameraOffset;

    public static string textInformation;

    public Transform focusPoint;

    private Vector3 startPosition;

    private void Start()
    {
        Academy.Instance.AutomaticSteppingEnabled = false;
        startPosition = transform.position;
        if(focusPoint)
        {
            SetCameraPosition();
            SetCameraRotation();
        }
    }
    
    void FixedUpdate()
    {
        Academy.Instance.EnvironmentStep();
    }

    //Reset various positions and other variables upon starting a new episode to allow for resetting the environment
    public override void OnEpisodeBegin()
    {
        if(focusPoint)
        {
            SetCameraPosition();
            SetCameraRotation();
        }
        experiment.Reset();
    }

    private void SetCameraPosition()
    {
        float xOffset = Random.Range(minXCameraOffset, maxXCameraOffset);
        float yOffset = Random.Range(minYCameraOffset, maxYCameraOffset);
        float zOffset = Random.Range(minZCameraOffset, maxZCameraOffset);
        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y + yOffset, startPosition.z + zOffset);
    }

    private void SetCameraRotation()
    {
        Vector3 direction = (focusPoint.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}
