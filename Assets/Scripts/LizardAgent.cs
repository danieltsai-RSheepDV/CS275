using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LizardAgent : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Initialize()
    {
        
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

        float[] outputmuscles = new float[4];

        // Im sorry
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        float[] muscleActuations = actions.ContinuousActions.Array;  // Can just feed directly into output, this is just easier visually

        
    }


}
