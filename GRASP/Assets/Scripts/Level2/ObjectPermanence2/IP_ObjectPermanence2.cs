using UnityEngine;

public class IP_ObjectPermanence2 : P_ObjectPermanence2
{
    private bool teleported;    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(hidden && !teleported)
        {
            cube.transform.position = new Vector3(innerCover.transform.position.x, cube.transform.position.y, cube.transform.position.z);
            teleported = true;
        }
    }


    public override void Reset()
    {
        base.Reset();
        teleported = false;
    }

    public override int RandomizeSetup()
    {
        int randomIndex =  base.RandomizeSetup();
        float betweenCovers = (innerCover.transform.position + outerCover.transform.position).x / 2f;
        cube.transform.position = new Vector3(betweenCovers, cube.transform.position.y, cube.transform.position.z);

        return randomIndex;
    }
}
