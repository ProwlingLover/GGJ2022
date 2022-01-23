using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShowChatWhenCollide : MonoBehaviour {
    public Fungus.Flowchart targetChat;
    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collisionInfo) 
    {
        if (collisionInfo.gameObject.name == "host")
        {
            if (targetChat != null)
            {
                targetChat.gameObject.SetActive(true);
            }
        }
    }
}