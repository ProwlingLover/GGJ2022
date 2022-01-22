using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECStair : MonoBehaviour
{
    // Start is called before the first frame update
    public EnumClimbDir dir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        var input = other.collider.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = dir;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        var input = other.collider.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = EnumClimbDir.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = dir;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var input = other.GetComponent<MoveInput>();
        if (input)
        {
            input.tempDir = EnumClimbDir.None;
        }
    }
}
