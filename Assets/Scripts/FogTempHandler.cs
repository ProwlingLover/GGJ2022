using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FogTempHandler : MonoBehaviour {
    public float fogTargetStart = -10;
    private bool canHandleFog = false;
    private Camera cam;
    private void Start() 
    {
        cam = Camera.main;
    }
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.name == "host")
        {
            canHandleFog = true;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            canHandleFog = true;
        }
    }

    private void FixedUpdate() 
    {
        if (canHandleFog)
        {
            MoveTargetFog();
        }
    }

    private void MoveTargetFog()
    {
        if (cam != null)
        {
            StartCoroutine(Move());
        }
    }


    IEnumerator Move()
    {
        if (cam != null)
        {
            var FogWithDepthTexture = cam.GetComponent<FogWithDepthTexture>();
            if (FogWithDepthTexture != null)
            {
                while (FogWithDepthTexture.fogStart != fogTargetStart)
                {
                    FogWithDepthTexture.fogStart = Mathf.Lerp(FogWithDepthTexture.fogStart, fogTargetStart, 0.5f);
                    yield return 0;
                }
                if (FogWithDepthTexture.fogStart == fogTargetStart) canHandleFog = false;
            }
        }
    }
}