using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    int rubbishInMe;

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
            other.gameObject.SetActive(false);
            rubbishInMe++;
            GameObject.Find("LandscapeGameRunner").GetComponent<LandscapeGameRunner>().AddToTimer(1f);
        }
    }
}
