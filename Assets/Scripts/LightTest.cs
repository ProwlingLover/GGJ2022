using UnityEngine;
using System.Collections;

public class LightTest : LightEventListener 
{

    private bool isSpotLightRay = false;
    public bool IsRay()
    {
        return isSpotLightRay;
    }
    protected override void OnLightEnter(RaycastHit info)
    {
        isSpotLightRay = true;
    }

    protected override void OnLightStay(RaycastHit info)
    {
    }

    protected override void OnLightExit(RaycastHit info)
    {
        isSpotLightRay = false;
    }

}
