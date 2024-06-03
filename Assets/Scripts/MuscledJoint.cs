using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscledJoint : MonoBehaviour
{
    [SerializeField] private int numMuscles;
    [SerializeField] private float neutralK;
    private SpringJoint[] muscles;
    private float[] muscleActivations;
    public float this[int key]{
        get => muscleActivations[key];
        set
        {
            muscleActivations[key] = value;
            muscles[key].spring = value * neutralK * 2;
        }
    }
    public int Length
    {
        get { return numMuscles; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SpringJoint[] allSprings = GetComponents<SpringJoint>();
        List<SpringJoint> muscleJoints = new List<SpringJoint>();
        for (int i = numMuscles; i < allSprings.Length; i++)
        {
            muscleJoints.Add(allSprings[i]);
        }
        muscles = muscleJoints.ToArray();

        muscleActivations = new float[numMuscles];
    }
}
