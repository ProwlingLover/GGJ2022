using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleMover : MonoBehaviour {
    public bool enableReturnBack = false;
    public List<Transform> halfWayPositionList = new List<Transform>();
    private Queue<Transform> halfWayPositionQueue = new Queue<Transform>();
    
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
        if (Input.GetKeyDown("f"))
        {
            if (!inMoving)
            {
                if (collisionInfo.gameObject.name == "host")
                {
                    inMoving = true;
                    Move();
                }
            }
        }
    }

    private void OnCollisionExit(Collision collisionInfo) 
    {
        if (enableReturnBack)
        {
            int len = halfWayPositionList.Count;
            for (int i = len - 1; i >= 0; i--)
            {
                halfWayPositionQueue.Enqueue(halfWayPositionList[i]);
            }
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
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, halfWayPositionQueue.Peek().position, 3f * Time.deltaTime);
            yield return 0;
        }
        if (halfWayPositionQueue.Count > 0)
        {
            halfWayPositionQueue.Dequeue();
        }
        else inMoving = false;
    }
}