using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug : MonoBehaviour
{

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            Debug.Log("Horizontal: " + Input.GetAxis("Horizontal"));
            Debug.Log("----------------------------------");
        }
        if (Input.GetAxis("Horizontal1") != 0)
        {
            Debug.Log("Horizontal1: " + Input.GetAxis("Horizontal1"));
            Debug.Log("----------------------------------");
        }
        if (Input.GetAxis("Horizontal2") != 0)
        {
            Debug.Log("Horizontal2: " + Input.GetAxis("Horizontal2"));
            Debug.Log("----------------------------------");
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            Debug.Log("Vertical: " + Input.GetAxis("Vertical"));
            Debug.Log("----------------------------------");
        }
        if (Input.GetAxis("Vertical1") != 0)
        {
            Debug.Log("Vertical1: " + Input.GetAxis("Vertical1"));
            Debug.Log("----------------------------------");
        }
        if (Input.GetAxis("Vertical2") != 0)
        {
            Debug.Log("Vertical2: " + Input.GetAxis("Vertical2"));
            Debug.Log("----------------------------------");
        }
    }
}
