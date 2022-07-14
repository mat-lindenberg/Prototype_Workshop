using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraSeedScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grass")
        {
            GameObject.Find("Manager").GetComponent<FloreManager>().SeedHere(this);
        }
    }

}
