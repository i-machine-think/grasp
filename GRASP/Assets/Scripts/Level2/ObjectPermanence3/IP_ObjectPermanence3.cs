public class IP_ObjectPermanence3 : P_ObjectPermanence3
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(hinge.angle > 90)
        {
            box.SetActive(false);
        } else if (hinge.angle < 90 && hinge.angle > 0)
        {
            box.SetActive(true);
        }
    } 
}
