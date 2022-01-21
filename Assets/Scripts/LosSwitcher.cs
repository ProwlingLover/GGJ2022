using UnityEngine;
using LOS;
public class LosSwitcher : MonoBehaviour {
    Camera cam;
    private void Start() {
        cam = Camera.main;
    }
    private void Update ()
	{
		if (Input.GetKeyDown("c")) SwitchOnLOS();
        if (Input.GetKeyUp("c")) SwitchOffLOS();
	}

    void SwitchOnLOS()
    {
        var losBufferStorage = cam.gameObject.GetComponent<LOSBufferStorage>();
        var desaturate = cam.gameObject.GetComponent<Desaturate>();
        var losMask = cam.gameObject.GetComponent<LOSMask>();
        losBufferStorage.enabled = true;
        desaturate.enabled = true;
        losMask.enabled = true;
    }

    void SwitchOffLOS()
    {
        var losBufferStorage = cam.gameObject.GetComponent<LOSBufferStorage>();
        var desaturate = cam.gameObject.GetComponent<Desaturate>();
        var losMask = cam.gameObject.GetComponent<LOSMask>();
        losBufferStorage.enabled = false;
        desaturate.enabled = false;
        losMask.enabled = false;
    }
}