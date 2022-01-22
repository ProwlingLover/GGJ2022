using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECStair : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;
    public float climbSpeed =0.005f;
    private bool canTrans = false;
    private Transform player;
    private bool climbing = false;
    private Vector3 lastPos;
    private float t = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbing)
        {
            player.position = Vector3.Lerp(lastPos, target.position, t);
            t += climbSpeed;
            if(player.position == target.position)
            {
                canTrans = false;
                climbing = false;
                var input = player.GetComponent<MoveInput>();
                input.ChangeMoveMode();
                player = null;
            }
        }
        else if (canTrans &&Input.GetKey("f"))
        {
            var input = player.GetComponent<MoveInput>();
            input.ChangeClimbMode();
            player.LookAt(transform.position + transform.forward);
            lastPos = player.position;
            climbing = true;
            t = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var input = other.collider.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = dir;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var input = other.collider.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = EnumClimbDir.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canTrans = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
