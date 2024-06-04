using UnityEngine;

public class IP_GravitySupport : P_GravitySupport
{
    public Transform leftEnd;
    public Transform rightEnd;

    public override void SampleTarget(int randomIndex)
    {
        float randomPoint;
        if(randomIndex == 0)
        {
            randomPoint = Random.Range(
                leftEnd.position.x + stick.transform.localScale.x / 2,
                bottomBlock.transform.position.x - bottomBlock.transform.localScale.x / 2 + stick.transform.localScale.x / 2 - topBlock.transform.localScale.x * 2/ 3
            );
        } else
        {
            randomPoint = Random.Range(
                bottomBlock.transform.position.x + bottomBlock.transform.localScale.x / 2 - stick.transform.localScale.x / 2 + topBlock.transform.localScale.x * 2/ 3, 
                rightEnd.position.x - stick.transform.localScale.x / 2
            );
        }
        stick.SetTarget(new Vector3(randomPoint, topBlock.transform.position.y, topBlock.transform.position.z));
    }
}
