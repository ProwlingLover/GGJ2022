using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding.Examples;
public class HostStateHelper : MonoBehaviour {

    private Animator anim;
    private Rigidbody body;
    private BoxCollider collider;

    public Fungus.Flowchart crossFadeChat;

    private GameObject emptyHost = null;

    private float sumTime = 0f;
    private float fadeTime = 0f;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        emptyHost = new GameObject("emptyHost");
    }

    private void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.gameObject.tag == "Enemy")
        {
            emptyHost.transform.position = gameObject.transform.position;
            emptyHost.transform.eulerAngles = gameObject.transform.eulerAngles;
            if (anim != null)
            {
                anim.Play("die");
            }
            var cam = Camera.main;
            var follow = cam.GetComponent<AstarSmoothFollow2>();
            if (follow != null)
            {
                follow.target = emptyHost.transform;
                follow.enabled = true;
            }
            if (collider != null)
            {
                collider.enabled = false;
            }
            StartCoroutine(startCountDown());
        }
    }

    public IEnumerator startCountDown()
    {
        while (sumTime <= 0.2)
        {
            sumTime++;
            if (sumTime >= 0.2)
            {
                if (crossFadeChat != null) crossFadeChat.gameObject.SetActive(true);
                yield break;
            }
            else if (sumTime < 0.2f)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public IEnumerator BeKilled()
    {
        while (fadeTime <= 2.5f)
        {
            fadeTime++;
            if (fadeTime >= 2.5)
            {
                if (crossFadeChat != null) crossFadeChat.gameObject.SetActive(false);
                yield break;
            }
            else if (fadeTime < 2.5f)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}