using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrees : MonoBehaviour
{
    [SerializeField] private GameObject treeGen;
    [SerializeField] private float radiusGen;
    [SerializeField] private int numTrees;

    private List<GameObject> generators = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < numTrees; i++)
        {
            GameObject geni = Instantiate(treeGen, Vector3.zero, Quaternion.identity);
            generators.Add(geni);
        }
    }

    private IEnumerator GenerateAndPositionTrees()
    {
        foreach (var gen in generators)
        {
            LSystemTree treeScript = gen.GetComponentInChildren<LSystemTree>();
            yield return new WaitUntil(() => treeScript.isGenerationComplete);
            float x = Random.Range(-radiusGen, radiusGen);
            float z = Random.Range(-radiusGen, radiusGen);
            Debug.Log((x, z));
            gen.transform.position = new Vector3(transform.position.x + x, transform.position.y + gen.transform.position.y, transform.position.z + z);
            Debug.Log(gen.transform.position);
        }
    }

    void Start()
    {
        StartCoroutine(GenerateAndPositionTrees());
    }

    void Update()
    {

    }
}