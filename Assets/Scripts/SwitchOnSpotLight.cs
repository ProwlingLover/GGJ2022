using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SwitchOnSpotLight : MonoBehaviour {
    private bool isLightOn = false;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "host")
        {
            
        }
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.name == "host")
        {
            if (Input.GetKeyDown("f"))
            {
                if (!isLightOn)
                {
                    isLightOn = true;
                    var light = gameObject.GetComponent<Light>();
                    light.enabled = true;
                }
            }
        }
    }
}