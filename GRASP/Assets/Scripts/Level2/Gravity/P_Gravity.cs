using UnityEngine;

public class P_Gravity : Experiment
{
    public GameObject plank;
    public GameObject boxEdge;

    public GameObject leftCover;
    public GameObject rightCover;

    public GameObject ball;

    public float minAngle;
    public float maxAngle;

    protected float randomAngle;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (frameCount >= startDelay)
        {
            ball.SetActive(true);
        }
    }

    public override void Reset()
    {
        base.Reset();
        int randomIndex = RandomizePlank();
        SpawnBall(randomIndex);
        ball.SetActive(false);
    }

    protected virtual void SpawnBall(int randomIndex)
    {
        float yOffset = Mathf.Tan(Mathf.Deg2Rad * Mathf.Abs(randomAngle)) * boxEdge.transform.localScale.x + boxEdge.transform.localScale.y / 2 + ball.transform.localScale.y / 2;
        if(randomIndex == 0)
        {
            ball.transform.position = new Vector3(boxEdge.transform.position.x + boxEdge.transform.localScale.x / 2 - ball.transform.localScale.x, boxEdge.transform.position.y + yOffset, boxEdge.transform.position.z);
        } else
        {
            ball.transform.position = new Vector3(boxEdge.transform.position.x - boxEdge.transform.localScale.x / 2 + ball.transform.localScale.x, boxEdge.transform.position.y + yOffset, boxEdge.transform.position.z);
        }
    }

    protected virtual int RandomizePlank()
    {
        int randomIndex = Random.Range(0, 2);
        randomAngle = Random.Range(minAngle, maxAngle);

        Quaternion rotation;
        if (randomIndex == 0)
        {
            rotation = Quaternion.Euler(new Vector3(0f, 0f, -(90 - randomAngle)));
        }
        else
        {
            rotation = Quaternion.Euler(new Vector3(0f, 0f, 90 - randomAngle));
        }

        plank.transform.SetPositionAndRotation(boxEdge.transform.position, rotation);

        // compute length of plank
        float length = (boxEdge.transform.localScale.x - ball.transform.localScale.x) / Mathf.Cos(Mathf.Deg2Rad * Mathf.Abs(randomAngle));
        plank.transform.localScale = new Vector3(plank.transform.localScale.x, length, plank.transform.localScale.z);

        float yOffset = Mathf.Tan(Mathf.Deg2Rad * Mathf.Abs(randomAngle)) * (boxEdge.transform.localScale.x - ball.transform.localScale.x) / 2 + boxEdge.transform.localScale.y / 2;
        float xOffset = randomIndex == 0 ? ball.transform.localScale.x/2 : -ball.transform.localScale.x/2;
        plank.transform.position = new Vector3(plank.transform.position.x + xOffset, plank.transform.position.y + yOffset, plank.transform.position.z);

        return randomIndex;
    }
}
