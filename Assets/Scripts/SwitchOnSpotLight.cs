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
            if (!isLightOn)
            {
                isLightOn = true;
                var light = gameObject.GetComponentInChildren<Light>();
                if (light != null) light.enabled = true;
                var anim = gameObject.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    anim.Play("activating");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            if (!isLightOn)
            {
                isLightOn = true;
                var light = gameObject.GetComponentInChildren<Light>();
                if (light != null) light.enabled = true;
                var anim = gameObject.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    anim.Play("activating");
                }
            }
        }
    }
}