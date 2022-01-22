using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraMoveToTarget : MonoBehaviour {
    public Transform targetCamTrans = null;
    public Camera targetCam = null;

    public float moveSpeed = 1.0f;

    private bool isEnableTransCam = false;
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.name == "host")
        {
            isEnableTransCam = true;
            MoveTargetCam();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            isEnableTransCam = true;
            MoveTargetCam();
        }
    }

    private void FixedUpdate() 
    {
        if (isEnableTransCam)
        {
            MoveTargetCam();
        }
    }

    private void MoveTargetCam()
    {
        if (targetCam != null && targetCamTrans != null)
        {
            StartCoroutine(MoveToPosition());
        }
    }


    IEnumerator MoveToPosition()
    {
        while (targetCam.transform.position != targetCamTrans.position || targetCam.transform.rotation.eulerAngles != targetCamTrans.transform.rotation.eulerAngles)
        {
            targetCam.transform.position = Vector3.MoveTowards(targetCam.transform.position, targetCamTrans.position, moveSpeed * Time.deltaTime);
            targetCam.transform.rotation =  Quaternion.Euler(Vector3.RotateTowards(targetCam.transform.rotation.eulerAngles, targetCamTrans.transform.rotation.eulerAngles, 180f, 50f * Time.deltaTime));
            yield return 0;
        }
        if (targetCam.transform.position == targetCamTrans.position && targetCam.transform.rotation.eulerAngles == targetCamTrans.transform.rotation.eulerAngles) isEnableTransCam = false;
    }
}