using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "savePoint")
        {
            var sl = GameObject.FindWithTag("SLManager");
            if (!sl) return;
            var manage= sl.GetComponent<SavaLoadManager>();
            if (manage)
            {
                manage.Save();
                Debug.Log("Save");
            }
        }
    }
}
