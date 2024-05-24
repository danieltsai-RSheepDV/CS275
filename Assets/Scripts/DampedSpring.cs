using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampedSpring : MonoBehaviour
{
    [SerializeField] Rigidbody leftLink;
    [SerializeField] Transform leftPositionRef;
    [SerializeField] Rigidbody rightLink;
    [SerializeField] Transform rightPositionRef;

    [Header("Spring Properties")]
    [SerializeField] float springConst = 20f;
    [SerializeField] float damperConst = 5f;
    [SerializeField] float restLength = 1f;
    [SerializeField] bool useStartingLengthAsRest = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Expected to run every timestep. Apply viscoelastic forces to RBs attached. 
    void ApplyForces()
    {
        
    }
}
