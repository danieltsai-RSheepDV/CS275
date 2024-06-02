using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public int SwarmIndex { get; set; }
    public float NoClumpingRadius { get; set; }
    public float LocalAreaRadius { get; set; }
    public float Speed { get; set; }
    public float SteeringSpeed { get; set; }
    public int NumberOfRays { get; set; }
    public Transform RayOrigin;

    public void SimulateMovement(List<ButterflyController> other, float time)
    {
        //default vars
        var steering = Vector3.zero;

        var separationDirection = Vector3.zero;
        var separationCount = 0;
        var alignmentDirection = Vector3.zero;
        var alignmentCount = 0;
        var cohesionDirection = Vector3.zero;
        var cohesionCount = 0;

        var leaderBoid = other[0];
        var leaderAngle = 180f;

        foreach (ButterflyController boid in other)
        {
            //skip self
            if (boid == this)
                continue;

            var distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify local neighbour
            if (distance < NoClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
                separationCount++;
            }

            //identify local neighbour
            if (distance < LocalAreaRadius && boid.SwarmIndex == this.SwarmIndex)
            {
                alignmentDirection += boid.transform.forward;
                alignmentCount++;

                cohesionDirection += boid.transform.position - transform.position;
                cohesionCount++;

                //identify leader
                var angle = Vector3.Angle(boid.transform.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        //flip
        separationDirection = -separationDirection;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        //get direction to center of mass
        cohesionDirection -= transform.position;

        //weighted rules
        steering += separationDirection.normalized;
        steering += alignmentDirection.normalized;
        steering += cohesionDirection.normalized;

        //local leader
        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized;

        //obstacle avoidance

        // RaycastHit hitInfo;
        // // var bam = 1;
        // if (Physics.Raycast(transform.position, transform.forward, out hitInfo, LocalAreaRadius, LayerMask.GetMask("Default")))
        // {
        //     steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;
        // }

        float goldenAngle = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden angle in radians

        for (int i = 0; i < NumberOfRays; i++)
        {
            // Calculate spherical coordinates
            float theta = goldenAngle * i;
            float z = 1 - (i / (float)(NumberOfRays - 1)) * 2; // Z-value between 1 and -1
            float radius = Mathf.Sqrt(1 - z * z); // Radius at z

            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            Vector3 rayDirection = new Vector3(x, y, z);

            RaycastHit hitInfo;
            // Perform raycast
            if (Physics.Raycast(RayOrigin.position, rayDirection, out hitInfo, LocalAreaRadius, LayerMask.GetMask("Default")))
            {
                // Calculate avoidance force
                Vector3 avoidDirection = (RayOrigin.position - hitInfo.point).normalized;
                steering += avoidDirection;

                // Draw hit point
                Debug.DrawRay(RayOrigin.position, rayDirection * hitInfo.distance, Color.green);
            }
            else
            {
                // Draw full length ray if no hit
                // Debug.DrawRay(RayOrigin.position, rayDirection * LocalAreaRadius, Color.red);
            }
        }


        //apply steering
        if (steering != Vector3.zero)
        {
            steering = steering.normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);
        }
        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;
    }
}