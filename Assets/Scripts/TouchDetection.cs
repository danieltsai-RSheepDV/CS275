using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    [HideInInspector] public bool isTouching = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Env"))
        {
            isTouching = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Env"))
        {
            isTouching = false;
        }
    }
}
