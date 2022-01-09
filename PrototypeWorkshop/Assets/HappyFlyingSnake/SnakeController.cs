using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public Rigidbody r;

    public List<Rigidbody> segments;

    public float springStrength;

    public float moveForce;

    public float dragForce;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            r.AddForce(Vector3.left * moveForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            r.AddForce(Vector3.down * moveForce);
        }
        if (Input.GetKey(KeyCode.D))
        {
            r.AddForce(Vector3.right * moveForce);
        }
        if (Input.GetKey(KeyCode.W))
        {
            r.AddForce(Vector3.up * moveForce);
        }

        for (int i = 0; i < segments.Count; i++)
        {
            if (i == 0)
            {

            }
            else
            {
                segments[i].transform.position = Vector3.Lerp(segments[i].transform.position, segments[i-1].transform.position, springStrength);
            }
            
        }

        r.velocity *= dragForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 re = Vector3.Reflect(r.velocity, collision.contacts[0].normal.normalized);
        //re = re.normalized;
        float velo = r.velocity.magnitude;
        //r.velocity = re * velo;
    }
}
