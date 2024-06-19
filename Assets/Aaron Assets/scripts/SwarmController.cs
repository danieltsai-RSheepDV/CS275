using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmController : MonoBehaviour
{
    public ButterflyController flyPrefab;
    public ButterflyController flyPrefab2;
    public ButterflyController flyPrefab3;

    public int spawnBoids = 100;
    public float boidSpeed = 10f;
    public float boidSteeringSpeed = 100f;
    public float boidNoClumpingArea = 2f;
    public float boidLocalArea = 10f;
    public float boidSimulationArea = 50f;

    public int numberOfRays = 36;

    private List<ButterflyController> _boids;
    // Start is called before the first frame update
    void Start()
    {
        _boids = new List<ButterflyController>();

        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(flyPrefab.gameObject, 0);
        }
        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(flyPrefab2.gameObject, 1);
        }
        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(flyPrefab3.gameObject, 2);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (ButterflyController boid in _boids)
        {
            if (boid == null) continue;

            boid.SimulateMovement(_boids, Time.deltaTime);

            var boidPos = boid.transform.position;

            if (boidPos.x > boidSimulationArea)
                boidPos.x -= boidSimulationArea * 2;
            else if (boidPos.x < -boidSimulationArea)
                boidPos.x += boidSimulationArea * 2;

            if (boidPos.y > boidSimulationArea)
                boidPos.y -= boidSimulationArea * 2;
            else if (boidPos.y < -boidSimulationArea)
                boidPos.y += boidSimulationArea * 2;

            if (boidPos.z > boidSimulationArea)
                boidPos.z -= boidSimulationArea * 2;
            else if (boidPos.z < -boidSimulationArea)
                boidPos.z += boidSimulationArea * 2;

            boid.transform.position = boidPos;
        }

    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab);

        boidInstance.transform.localPosition += new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        boidInstance.transform.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        var boidController = boidInstance.GetComponent<ButterflyController>();
        boidController.SwarmIndex = swarmIndex;
        boidController.Speed = boidSpeed;
        boidController.SteeringSpeed = boidSteeringSpeed;
        boidController.LocalAreaRadius = boidLocalArea;
        boidController.NoClumpingRadius = boidNoClumpingArea;
        boidController.NumberOfRays = numberOfRays;

        _boids.Add(boidController);
    }
}
