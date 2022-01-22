using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InputTeachTest : MonoBehaviour {
    public Fungus.Flowchart myflowchart;
    
    private void FixedUpdate() {
        if (Input.GetKeyDown("c"))
        {
            if (myflowchart != null)
            {
                myflowchart.SetBooleanVariable("isUseC", true);
            }
        }
    }
}