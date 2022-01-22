using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FlowObject : MonoBehaviour {
    public float speed = 1f;
    public Transform ori;
    public Transform des;
    float z = 0;
    float y = 0;
    float x = 0;
    private GameObject lockFollower = null;
    private bool isStayCollide = false;
    public bool isLockCollide = true;
    public Axis moveAxis = Axis.X;
    void Start () 
    {
        z = transform.position.z;
        y = transform.position.y;
        x = transform.position.x;
    }
 
    // Update is called once per frame
    void FixedUpdate () 
    {
        float distance = Mathf.PingPong(Time.time * speed, Vector3.Distance(ori.position, des.position));
        //每帧都给游戏物体一个新的坐标
        if (moveAxis == Axis.Z) transform.position = new Vector3(transform.position.x, transform.position.y, z + distance);
        else if (moveAxis == Axis.Y) transform.position = new Vector3(transform.position.x, y + distance, transform.position.z);
        else transform.position = new Vector3(x + distance, transform.position.y, transform.position.z);
 
    }

    private void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.gameObject.name == "host")
        {
            isStayCollide = true;
        }
        if (isLockCollide && collisionInfo.gameObject.name == "host")
        {
            lockFollower = collisionInfo.gameObject;
            lockFollower.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision collisionInfo) 
    {
        isStayCollide = false;
        if (lockFollower != null)
        {
            lockFollower.transform.parent = null;
            lockFollower = null;
        }
    }
}