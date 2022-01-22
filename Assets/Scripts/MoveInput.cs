using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveInput : MonoBehaviour
{
    private Rigidbody boby;
    public float moveSpeed = 5f;
    public float climbSpeed = 2f;
    public float rotateSpeed = 2f;
    public EnumClimbDir climbDir;
    public EnumClimbDir tempDir;
    public EnumMoveState moveState;
    private Animator anim;
    public float rotateOffset = 90;
    private Vector3 forward;
    private Vector3 right;
    Quaternion targetAngels, climbAngels;
    public Camera followCamera;
    // Start is called before the first frame update
    void Start()
    {
        moveState = EnumMoveState.Move;
        boby = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        RreshCamera();
    }

    void RreshCamera()
    {
        forward = followCamera.transform.forward - Vector3.Dot(followCamera.transform.forward, Vector3.up) * Vector3.up;
        right = followCamera.transform.right - Vector3.Dot(followCamera.transform.right, Vector3.up) * Vector3.up;
    }

    Vector3 input;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsClimbing())
        {
            Move();
        }
    }

    void Move()
    {
        input.x = input.y = input.z = 0;
        var moveAmount = Vector3.zero;
        if (Input.GetKey("w")) moveAmount += forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey("a")) moveAmount += -right * moveSpeed * Time.deltaTime;
        if (Input.GetKey("s")) moveAmount += -forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey("d")) moveAmount += right * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + moveAmount);
        transform.position += moveAmount;
        if (moveAmount.magnitude != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    public bool IsClimbing()
    {
        return moveState == EnumMoveState.Climp;
    }

    public void ChangeMoveMode()
    {
        moveState = EnumMoveState.Move;
        anim.SetBool("climbing", false);
        //boby.useGravity = true;
    }

    public void ChangeClimbMode()
    {
        moveState = EnumMoveState.Climp;
        anim.SetBool("moving", false);
        anim.SetBool("climbing", true);
    }
}