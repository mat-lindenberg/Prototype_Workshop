using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputShower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.anyKey)
        {
            Debug.Log(Input.inputString);
        }   
    }
}
