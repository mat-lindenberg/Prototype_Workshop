using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheContact : MonoBehaviour
{
    public Scythe scythe;

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
        if (scythe.velo < scythe.veloThresh)
        {
            return;
        }

        if (other.tag == "Grass")
        {
            other.GetComponent<GrassScript>().CutMe();
        }
        if (other.tag == "Weed")
        {
            other.GetComponent<WeedScript>().CutTop();
        }
        if (other.tag == "Flower")
        {
            other.GetComponent<FlowerScript>().CutTop();
        }
        if (other.tag == "Bush")
        {
            other.GetComponent<BushScript>().ClipMe();
        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (scythe.velo < scythe.veloThresh)
        {
            return;
        }

        if (other.tag == "Grass")
        {
            other.GetComponent<GrassScript>().CutMe();
        }
        if (other.tag == "Weed")
        {
            other.GetComponent<WeedScript>().CutTop();
        }
        if (other.tag == "Flower")
        {
            other.GetComponent<FlowerScript>().CutTop();
        }
        if (other.tag == "Bush")
        {
            other.GetComponent<BushScript>().ClipMe();
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (scythe.velo < scythe.veloThresh)
        {
            return;
        }

        if (other.tag == "Grass")
        {
            other.GetComponent<GrassScript>().CutMe();
        }
        if (other.tag == "Weed")
        {
            other.GetComponent<WeedScript>().CutTop();
        }
        if (other.tag == "Flower")
        {
            other.GetComponent<FlowerScript>().CutTop();
        }
        if (other.tag == "Bush")
        {
            other.GetComponent<BushScript>().ClipMe();
        }

    }
}
