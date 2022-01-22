using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LightRayTest : MonoBehaviour {
    public List<Transform> spotlightList = new List<Transform>();
    public List<float> distanceThresholdList = new List<float>();
    public GameObject target;

    public bool RayTest()
    {
        if (spotlightList != null && distanceThresholdList != null)
        {
            if (target != null)
            {
                var pos = target.transform.position;
                for (int i = 0; i < 1; i++)
                {
                    Debug.Log((spotlightList[i].position - pos).sqrMagnitude);
                    if ((spotlightList[i].position - pos).sqrMagnitude <= distanceThresholdList[i]) return true;
                }
            }
        }
        return false;
    }
}