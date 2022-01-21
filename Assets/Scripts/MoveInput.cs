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
    Quaternion targetAngels, climbAngels;
    // Start is called before the first frame update
    void Start()
    {
        moveState = EnumMoveState.Move;
        boby = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    Vector3 input;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            if (IsClimbing())
            {
                ChangeMoveMode();
            }
            else if(tempDir != EnumClimbDir.None)
            {
                ChangeClimbMode();
            }
        }
        if (IsClimbing())
        {
            Climb();
        }
        else
        {
            Move();
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetAngels, rotateSpeed * Time.deltaTime);
    }

    void Move()
    {
        input.x = input.y = input.z = 0;
        input.x = Input.GetAxis("Vertical"); 
        input.z = Input.GetAxis("Horizontal") * -1;
        input.Normalize();
        var moveAmount = input * moveSpeed * Time.deltaTime;
        transform.position += moveAmount;
        transform.LookAt(transform.position);
        Rotate();
        if (input.magnitude != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    void Rotate()
    {
        if (input.x > 0)
        {
            if (input.z > 0)
            {
                targetAngels = Quaternion.Euler(0, 315 + rotateOffset, 0);
            }
            else if (input.z < 0)
            {
                targetAngels = Quaternion.Euler(0, 45 + rotateOffset, 0);
            }
            else
            {
                targetAngels = Quaternion.Euler(0, 0 + rotateOffset, 0);
            }
        }
        else if (input.x < 0)
        {
            if (input.z > 0)
            {
                targetAngels = Quaternion.Euler(0, 225 + rotateOffset, 0);
            }
            else if (input.z < 0)
            {
                targetAngels = Quaternion.Euler(0, 135 + rotateOffset, 0);
            }
            else
            {
                targetAngels = Quaternion.Euler(0, 180 + rotateOffset, 0);
            }
        }
        else
        {
            if (input.z > 0)
            {
                targetAngels = Quaternion.Euler(0, 270 + rotateOffset, 0);
            }
            else if (input.z < 0)
            {
                targetAngels = Quaternion.Euler(0, 90 + rotateOffset, 0);
            }
        }
    }

    void Climb()
    {
        input.x = input.y = input.z = 0;
        switch (climbDir)
        {
            case EnumClimbDir.WU:
                input.y = Input.GetAxis("Vertical");
                break;
            case EnumClimbDir.WD:
                input.y = Input.GetAxis("Vertical") * -1;
                break;
            case EnumClimbDir.AU:
                input.y = Input.GetAxis("Horizontal") * -1;
                break;
            case EnumClimbDir.AD:
                input.y = Input.GetAxis("Horizontal");
                break;
        }
        var climbAmount = input * climbSpeed * Time.deltaTime;
        if(input.y > 0)
        {
            LayerMask mask = LayerMask.GetMask("stairup");
            if (Physics.Raycast(transform.position, Vector3.up, climbSpeed * Time.deltaTime, mask))
            {
                anim.SetBool("climbing", false);
                return;
            }
        }
        else if(input.y < 0)
        {
            LayerMask mask = LayerMask.GetMask("stairdown");
            if (Physics.Raycast(transform.position, Vector3.down, climbSpeed * Time.deltaTime, mask))
            {
                anim.SetBool("climbing", false);
                return;
            }
        }
        transform.position += climbAmount;
        if (input.y != 0)
        {
            anim.SetBool("climbing", true);
        }
        else
        {
            anim.SetBool("climbing", false);
        }
    }

    public bool IsClimbing()
    {
        return moveState == EnumMoveState.Climp;
    }

    public void ChangeMoveMode()
    {
        moveState = EnumMoveState.Move;
        climbDir = EnumClimbDir.None;
        //boby.useGravity = true;
    }

    public void ChangeClimbMode()
    {
        moveState = EnumMoveState.Climp;
        //boby.useGravity = false;
        climbDir= tempDir;
        Debug.Log(tempDir);
        switch (climbDir)
        {
            case EnumClimbDir.WU:
                targetAngels = Quaternion.Euler(0, 0 + rotateOffset, 0);
                break;
            case EnumClimbDir.WD:
                targetAngels = Quaternion.Euler(0, 180 + rotateOffset, 0);
                break;
            case EnumClimbDir.AU:
                targetAngels = Quaternion.Euler(0, 270 + rotateOffset, 0);
                break;
            case EnumClimbDir.AD:
                targetAngels = Quaternion.Euler(0, 90 + rotateOffset, 0);
                break;
        }
    }
}