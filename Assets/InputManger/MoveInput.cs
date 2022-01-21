using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInput : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 10f;
    public float rotateSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var valX = Input.GetAxis("Vertical");
        var valY = Input.GetAxis("Horizontal");
        //移动
        var moveX = transform.forward * moveSpeed * valX * Time.deltaTime;
        controller.Move(moveX);
        //转头
        transform.Rotate(Vector3.up, valY * rotateSpeed);
    }
}
