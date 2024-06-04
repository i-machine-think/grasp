using UnityEngine;

public class IP_Unchangeableness2 : P_Unchangeableness2
{
    public GameObject cover;
    private bool colorChanged;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector3.Distance(ball.transform.position, new Vector3(cover.transform.position.x, ball.transform.position.y, ball.transform.position.z)) <= 0.1 && !colorChanged)
        {
            Renderer renderer = ball.GetComponent<Renderer>();
            renderer.material.color = new Color(255f, 255f, 255f);
            colorChanged = true;
        }
    }

    public override void Reset()
    {
        base.Reset();
        colorChanged = false;
    } 
}
