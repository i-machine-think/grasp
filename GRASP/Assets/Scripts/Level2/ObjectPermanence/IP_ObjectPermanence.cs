using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IP_ObjectPermanence : P_ObjectPermanence
{
    public Transform leftBarrier;
    public Transform rightBarrier;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        GameObject gameObject = ball.gameObject;
        if(leftCover.activeSelf)
        {
            if(Mathf.Abs(ball.transform.position.x - leftBarrier.position.x) < 0.03f)
            {
                gameObject.SetActive(false);
            } else if(Mathf.Abs(ball.transform.position.x - rightBarrier.position.x) < 0.03f)
            {
                gameObject.SetActive(true);
            }
        } else
        {
            if(Mathf.Abs(ball.transform.position.x - rightBarrier.position.x) < 0.03f)
            {
                gameObject.SetActive(false);
            } else if(Mathf.Abs(ball.transform.position.x - leftBarrier.position.x) < 0.03f)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
