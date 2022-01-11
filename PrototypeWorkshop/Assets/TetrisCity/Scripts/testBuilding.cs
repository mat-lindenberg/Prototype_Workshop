using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBuilding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color c = Random.ColorHSV();

        Renderer[] r = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < r.Length; i++)
        {
            r[i].material.color = c;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
