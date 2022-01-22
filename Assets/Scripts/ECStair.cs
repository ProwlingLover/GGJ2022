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
    private Transform tempPlayer;
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
            if (target != null)
            {
                player.position = Vector3.Lerp(lastPos, target.position, t);
                t += climbSpeed;
                if(player.position == target.position)
                {
                    climbing = false;
                    var input = player.GetComponent<MoveInput>();
                    input.ChangeMoveMode();
                    player = null;
                }
            }
        }
        else if (canTrans &&Input.GetKey("g"))
        {
            player = tempPlayer;
            var input = player.GetComponent<MoveInput>();
            input.ChangeClimbMode();
            // player.LookAt(transform.position + transform.forward);
            lastPos = player.position;
            climbing = true;
            t = 0;
            canTrans = false;
            tempPlayer = null;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            canTrans = true;
            tempPlayer = other.transform;
        }
    }

    private void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Player")
        {
            canTrans = false;
            tempPlayer = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canTrans = true;
            tempPlayer = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canTrans = false;
            tempPlayer = null;
        }
    }

}
