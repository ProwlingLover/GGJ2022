using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding.Examples;
using Fungus;
public class CloseFade : MonoBehaviour {

    private void Start()
    {
        var FungusManager = GameObject.Find("FungusManager");
        if (FungusManager != null)
        {
            var CameraManager = FungusManager.GetComponent<CameraManager>();
            if (CameraManager != null)
            {
                CameraManager.enabled = false;
            }
        }
    }

    
}