using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding.Examples;
public class EndHostTriggerTest : MonoBehaviour {

    public Fungus.Flowchart targetChat;
    private Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.gameObject.name == "host")
        {
            if (animator != null)
            {
                animator.Play("active");
            }
            if (targetChat != null)
            {
                targetChat.gameObject.SetActive(true);
            }
        }
    }

    private void ShowFinalStage()
    {

    }
}