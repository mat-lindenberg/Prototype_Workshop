using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelCollider : MonoBehaviour
{
    public List<GameObject> weeds;

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
        if (other.tag == "Weed")
        {
            weeds.Add(other.gameObject);
        }
        if (other.tag == "Flower")
        {
            weeds.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weed")
        {
            weeds.Remove(other.gameObject);
        }
        if (other.tag == "Flower")
        {
            weeds.Remove(other.gameObject);
        }
    }
}