using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RakeCollider : MonoBehaviour
{
    public List<GameObject> leafs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Leaf")
        {
            leafs.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Leaf")
        {
            leafs.Remove(other.gameObject);
        }
    }
}
