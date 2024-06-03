using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTEst : MonoBehaviour
{
    [SerializeField] public MuscledJoint j;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        j[2] = Mathf.Abs(Mathf.Sin(Time.time / 2f));
        j[3] = 1 - Mathf.Abs(Mathf.Sin(Time.time / 2f));
        // Debug.Log(Mathf.Sin(Time.time / 10f));
    }
}
