using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleMover : MonoBehaviour {
    private GameObject lockFollower = null;
    public bool enableReturnBack = false;
    public List<Transform> halfWayPositionList = new List<Transform>();
    private Queue<Transform> halfWayPositionQueue = new Queue<Transform>();
    private bool isStayCollide = false;

    public bool isLockCollide = false;

    public float speed = 1.0f;
    
    private bool inMoving = false;
    private void Start()
    {
        if (halfWayPositionList != null)
        {
            foreach (var it in halfWayPositionList)
            {
                halfWayPositionQueue.Enqueue(it);
            }
        }
    }
    private void OnCollisionStay(Collision collisionInfo) 
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

    private void FixedUpdate() 
    {
        if (Input.GetKeyDown("p"))
        {
            if (!inMoving && isStayCollide)
            {
                inMoving = true;
            }
        }
        if (inMoving)
        {
            Move();
        }   
    }

    private void OnCollisionExit(Collision collisionInfo) 
    {
        isStayCollide = false;
        if (enableReturnBack)
        {
            int len = halfWayPositionList.Count;
            for (int i = len - 1; i >= 0; i--)
            {
                halfWayPositionQueue.Enqueue(halfWayPositionList[i]);
            }
        }
        if (lockFollower != null)
        {
            lockFollower.transform.parent = gameObject.transform;
            lockFollower = null;
        }
    }

    private void Move()
    {
        if (halfWayPositionList != null)
        {
            StartCoroutine(MoveToPosition());
        }
    }

    IEnumerator MoveToPosition()
    {
        while (halfWayPositionQueue.Count > 0 && gameObject.transform.position != halfWayPositionQueue.Peek().position)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, halfWayPositionQueue.Peek().position, speed * Time.deltaTime);
            yield return 0;
        }
        if (halfWayPositionQueue.Count > 0)
        {
            halfWayPositionQueue.Dequeue();
        }
        else inMoving = false;
    }
}