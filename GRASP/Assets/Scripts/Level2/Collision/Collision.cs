using UnityEngine;

public class Collision : P_Gravity
{
    public GameObject stationaryBall;

    public override void Reset()
    {
        base.Reset();
        Rigidbody droppingBallRb = ball.GetComponent<Rigidbody>();
        Rigidbody stationaryBallRb = stationaryBall.GetComponent<Rigidbody>();

        droppingBallRb.velocity = Vector3.zero;
        droppingBallRb.angularVelocity = Vector3.zero;
        stationaryBallRb.velocity = Vector3.zero;
        stationaryBallRb.angularVelocity = Vector3.zero;
    }

    protected override void SpawnBall(int randomIndex)
    {
        base.SpawnBall(randomIndex);
        if(randomIndex == 0)
        {
            stationaryBall.transform.position = new Vector3(boxEdge.transform.position.x - stationaryBall.transform.localScale.x / 2, boxEdge.transform.position.y + boxEdge.transform.localScale.y / 2 + stationaryBall.transform.localScale.y / 2, ball.transform.position.z);
        } else
        {
            stationaryBall.transform.position = new Vector3(boxEdge.transform.position.x + stationaryBall.transform.localScale.x / 2, boxEdge.transform.position.y + boxEdge.transform.localScale.y / 2 + stationaryBall.transform.localScale.y / 2, ball.transform.position.z);
        }
    }

    protected override int RandomizePlank()
    {
        int randomIndex = Random.Range(0, 2);
        randomAngle = Random.Range(minAngle, maxAngle);

        Quaternion rotation;
        if (randomIndex == 0)
        {
            rightCover.SetActive(true);
            leftCover.SetActive(false);
            rotation = Quaternion.Euler(new Vector3(0f, 0f, -(90 - randomAngle)));
        }
        else
        {
            rightCover.SetActive(false);
            leftCover.SetActive(true);
            rotation = Quaternion.Euler(new Vector3(0f, 0f, 90 - randomAngle));
        }

        plank.transform.SetPositionAndRotation(boxEdge.transform.position, rotation);

        float length = (boxEdge.transform.localScale.x - boxEdge.transform.localScale.y*2) / 2 / Mathf.Cos(Mathf.Deg2Rad * Mathf.Abs(randomAngle));
        plank.transform.localScale = new Vector3(plank.transform.localScale.x, length, plank.transform.localScale.z);

        float yOffset = Mathf.Tan(Mathf.Deg2Rad * Mathf.Abs(randomAngle)) * ((boxEdge.transform.localScale.x - boxEdge.transform.localScale.y*2) / 2) / 2 + boxEdge.transform.localScale.y / 2;
        float xOffset = randomIndex == 0 ? (boxEdge.transform.localScale.x - boxEdge.transform.localScale.y*2) / 4 : -(boxEdge.transform.localScale.x - boxEdge.transform.localScale.y*2) / 4;
        plank.transform.position = new Vector3(plank.transform.position.x + xOffset, plank.transform.position.y + yOffset, plank.transform.position.z);

        return randomIndex;
    }
}
