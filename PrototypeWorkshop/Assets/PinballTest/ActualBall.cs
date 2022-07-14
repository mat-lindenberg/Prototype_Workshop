using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualBall : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody r;
    public float zGravity;
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        r.AddForce(new Vector3(0, 0, -1)* zGravity);
    }
}
