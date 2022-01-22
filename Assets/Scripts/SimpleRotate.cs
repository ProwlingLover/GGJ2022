using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleRotate : MonoBehaviour {

    public Vector3 endRotation = new Vector3();
    
    private void Start()
    {
 
    }

    private void OnCollisionStay(Collision collisionInfo) 
    {
        if (Input.GetKeyDown("f"))
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        if (endRotation != null)
        {
            StartCoroutine(RotateToEndRot());
        }
    }

    IEnumerator RotateToEndRot()
    {
        while (endRotation != null && gameObject.transform.rotation.eulerAngles != endRotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(Vector3.RotateTowards(gameObject.transform.rotation.eulerAngles, endRotation, 180f, 50f * Time.deltaTime));
            yield return 0;
        }
    }
}