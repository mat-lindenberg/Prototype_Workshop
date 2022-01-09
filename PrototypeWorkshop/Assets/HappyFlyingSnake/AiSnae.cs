using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSnae : MonoBehaviour
{
    public Rigidbody r;

    public List<Rigidbody> segments;

    public float springStrength;

    public float moveForce;

    public List<Transform> waypoints;
    public float distanceToWaypoint = 1f;
    public int placeInWaypoints = 0;

    // Start is called before the first frame update
    void Start()
    {


        r = transform.GetComponent<Rigidbody>();
        springStrength = .4f;
        
        for ( int i = 0; i < transform.parent.childCount; i++)
        {
            segments.Add(transform.parent.GetChild(i).GetComponent<Rigidbody>());
        }

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 0 - i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position,waypoints[placeInWaypoints].transform.position) < distanceToWaypoint)
        {
            placeInWaypoints++;
            if (placeInWaypoints > waypoints.Count - 1)
            {
                placeInWaypoints = 0;
            }
        }

        Vector3 dir = waypoints[placeInWaypoints].transform.position - transform.position;
        dir = dir.normalized;

        if (Vector3.Distance(transform.position, waypoints[placeInWaypoints].transform.position) < 5f)
        {
            r.AddForce(dir * moveForce * .95f);
        }
        else
        {
            r.AddForce(dir * moveForce);
        }

        r.velocity *= .95f;


        for (int i = 0; i < segments.Count; i++)
        {
            if (i == 0)
            {

            }
            else
            {
                segments[i].transform.position = Vector3.Lerp(segments[i].transform.position, segments[i - 1].transform.position, springStrength);
            }

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "AI")
        {

            Vector3 dir = transform.position - collision.contacts[0].point;
            dir = dir.normalized;
            dir.z = 0;
            r.AddForce(dir * 75f);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        Vector3 dir = transform.position - collision.contacts[0].point;
        dir = dir.normalized;
        dir.z = 0;
        r.AddForce(dir * 15f);
    }
}
