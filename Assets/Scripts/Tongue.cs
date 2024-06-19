using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    List<GameObject> caught;

    // Start is called before the first frame update
    void Start()
    {
        caught = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearCaught()
    {
        foreach (GameObject thing in caught)
        {
            Destroy(thing);
        }
        caught.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerConsumable"))
        {
            Debug.Log("Found catchable");

            ButterflyController bc = other.GetComponent<ButterflyController>();

            if (bc != null)  // is a butterfly
            {
                Debug.Log("Despawning fly, caught in tongue");
                GameObject obj = Instantiate(other.gameObject);
                Destroy(other.gameObject);
                obj.transform.parent = transform;
            }
        }
    }
}
