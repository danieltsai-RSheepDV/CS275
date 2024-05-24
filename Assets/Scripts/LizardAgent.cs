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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Initialize()
    {
        originalMaxLengths = new float[springJoints.Length];
        for (int i = 0; i < springJoints.Length; i++)
        {
            originalMaxLengths[i] = springJoints[i].maxDistance;
        }
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);


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
    }


}
