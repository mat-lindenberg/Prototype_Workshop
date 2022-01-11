using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorMod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color c = Random.ColorHSV();
        c = Color.green;
        c.b += Random.Range(.10f, -.10f);
        c.r += Random.Range(.10f, -.10f);
        c.g += Random.Range(.10f, -.10f);
        GetComponent<Renderer>().material.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
