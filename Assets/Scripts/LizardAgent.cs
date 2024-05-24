using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LizardAgent : Agent
{
    [SerializeField] SpringJoint[] springJoints;
    float[] originalMaxLengths;

    [SerializeField] Transform targetObject;

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
        originalMaxLengths = new float[springJoints.Length];
        for (int i = 0; i < springJoints.Length; i++)
        {
            originalMaxLengths[i] = springJoints[i].maxDistance;
        }

        previousPosition = transform.position;
        if (targetObject)
        {
            previousDistance = Vector3.Distance(transform.position, targetObject.position);
        }
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(transform.position.z);
        sensor.AddObservation(transform.forward.x);
        sensor.AddObservation(transform.forward.y);
        sensor.AddObservation(transform.forward.z);
        sensor.AddObservation(transform.up.x);
        sensor.AddObservation(transform.up.y);
        sensor.AddObservation(transform.up.z);
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

        if (springJoints.Length != actions.ContinuousActions.Length)
        {

        } else
        {
            for (int i = springJoints.Length - 1; i >= 0; i--)
            {
                springJoints[i].maxDistance = originalMaxLengths[i] * actions.ContinuousActions[i];
            }
        }


        AddReward(transform.position.z - previousPosition.z);  // use to train one dimensional movement 
        previousPosition = transform.position;

        // Target navigation
        //float curDist = Vector3.Distance(transform.position, targetObject.position);
        //AddReward(previousDistance - curDist);
        //previousDistance = curDist;
    }


}
