using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{

    public Transform target;

    public float yOffset;
    public float zOffset;
    public float xOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x + xOffset, target.position.y+yOffset,target.position.z+zOffset);
    }
}
