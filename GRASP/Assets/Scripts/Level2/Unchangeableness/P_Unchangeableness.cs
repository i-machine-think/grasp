using System.Collections.Generic;
using UnityEngine;

public class P_Unchangeableness : Experiment
{
    public Transform spawns;
    public ExperimentComponent cube;
    public GameObject cover;

    public ExperimentComponent ball;
    public int maxObjects;

    protected List<ExperimentComponent> experimentComponents = new List<ExperimentComponent>();
    protected Vector3 coverStartPosition;

    protected bool spawnedOne;
    protected Vector3 coverTarget;

    protected override void Start()
    {
        base.Start();
        coverStartPosition = cover.transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(frameCount > startDelay)
        {
            MoveCover();
            if (Vector3.Distance(cover.transform.position, coverTarget) <= 0.01f)
            {
                ReverseCoverMovement();
            }
        }
    }

    protected virtual void ReverseCoverMovement()
    {
        coverTarget = new Vector3(coverStartPosition.x, coverStartPosition.y - cover.transform.localScale.y, coverStartPosition.z);
    }

    public override void Reset()
    {
        base.Reset();
        DestroyObjects();
        spawnedOne = false;
        for(int i = 0; i < spawns.childCount; i++)
        {
            ExperimentComponent newObj;
            if(!spawnedOne || Random.Range(0, 2) == 0)
            {
                if(Random.Range(0, 2) == 0)
                {
                    newObj = Instantiate(ball, spawns.GetChild(i).transform.position, spawns.GetChild(i).transform.rotation);
                } else
                {
                    newObj = Instantiate(cube, spawns.GetChild(i).transform.position, spawns.GetChild(i).transform.rotation);
                }
                experimentComponents.Add(newObj);
                newObj.RandomizeColor();
                spawnedOne = true;
            }
            if(experimentComponents.Count == maxObjects)
            {
                break;
            }
        }
        cover.transform.position = new Vector3(coverStartPosition.x, coverStartPosition.y - cover.transform.localScale.y, coverStartPosition.z);
        coverTarget = coverStartPosition;

        Rigidbody rb = cover.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void DestroyObjects()
    {
        foreach(ExperimentComponent component in experimentComponents)
        {
            Destroy(component.gameObject);
        }

        experimentComponents.Clear();
    }

    protected void MoveCover()
    {
        float step = 0.5f * Time.fixedDeltaTime;
        cover.transform.position = Vector3.MoveTowards(cover.transform.position, coverTarget, step);
    }
}
