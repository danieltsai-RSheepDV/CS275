using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LizardAgent : Agent
{
    // [SerializeField] SpringJoint[] springJoints;
    [SerializeField] MuscledJoint[] joints;
    // float[] originalMaxLengths;

    float[] activationStates;
    [SerializeField] [Tooltip("Transforms of all bones influenced by muscles")] Transform[] childTransforms;  


    [SerializeField] Transform targetObject;
    [SerializeField] Transform mainBody;

    // Things to test for training
    Vector3 previousPosition = Vector3.zero;
    float previousDistance;

    // For inference
    public void SetTargetObject(Transform target)
    {
        targetObject = target;
    }

    public override void Initialize()
    {
        /*
        originalMaxLengths = new float[springJoints.Length];
        for (int i = 0; i < springJoints.Length; i++)
        {
            originalMaxLengths[i] = springJoints[i].maxDistance;
        }
        */

        joints = GetComponentsInChildren<MuscledJoint>();
        childTransforms = new Transform[joints.Length];
        for (int i = 0; i < childTransforms.Length; i++)
        {
            childTransforms[i] = joints[i].transform;
        }
        
        previousPosition = mainBody.position;
        if (targetObject)
        {
            previousDistance = Vector3.Distance(transform.position, targetObject.position);
        }
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();

        // Move the agent back to starting position

        mainBody.position = transform.position;
        mainBody.rotation = transform.rotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        // AddObservation takes int, float, Vector3, Quaternion

        // Pelvis (position + rotation) + 12 joints, each 1 float (activation) + 3 floats (position) + 4 floats (rotation) = 105 state size

        sensor.AddObservation(mainBody.position);
        sensor.AddObservation(mainBody.rotation);

        foreach(float act in activationStates)
        {
            sensor.AddObservation(act); 
        }

        foreach(Transform trfm in childTransforms)
        {
            sensor.AddObservation(trfm.position);
            sensor.AddObservation(trfm.rotation);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        var outputmuscles = actionsOut.ContinuousActions;

        // Im sorry
        if (Input.GetKey(KeyCode.Alpha1))
        {
            outputmuscles[0] = 0.0f;
        } else
        {
            outputmuscles[0] = 1.0f;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            outputmuscles[1] = 0.0f;
        } else
        {
            outputmuscles[1] = 1.0f;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            outputmuscles[2] = 0.0f;
        } else
        {
            outputmuscles[2] = 1.0f;
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            outputmuscles[3] = 0.0f;
        } else
        {
            outputmuscles[3] = 1.0f;
        }

        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        // float[] muscleActuations = actions.ContinuousActions.Array;  // Can just feed directly into output, this is just easier visually

        // 8 joints with 4 muscles, 4 joints with 1 muscle, totalling 36
        int actioniter = 0;
        try
        {
            for (int i = 0; i < joints.Length; i++)
            {
                for (int j = 0; j < joints[i].Length; j++)
                {
                    joints[i][j] = Mathf.Clamp01(actions.ContinuousActions[actioniter]);
                    Debug.Log(actions.ContinuousActions[actioniter]);
                    actioniter++;
                }
            }
        } catch
        {
            Debug.LogWarning("Action space mismatch occurrred at index: " + actioniter);
        }

        /*
        if (springJoints.Length != actions.ContinuousActions.Length)
        {

        } else
        {
            for (int i = springJoints.Length - 1; i >= 0; i--)
            {
                springJoints[i].maxDistance = originalMaxLengths[i] * actions.ContinuousActions[i];
            }
        }
        */

        AddReward(mainBody.position.z - previousPosition.z);  // use to train one dimensional movement 
        previousPosition = mainBody.position;

        // Target navigation
        //float curDist = Vector3.Distance(transform.position, targetObject.position);
        //AddReward(previousDistance - curDist);
        //previousDistance = curDist;
    }


}
