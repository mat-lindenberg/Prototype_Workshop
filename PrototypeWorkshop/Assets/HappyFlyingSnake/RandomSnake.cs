using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSnake : MonoBehaviour
{
    public Rigidbody r;

    public List<Rigidbody> segments;

    public Transform waypointParent;
    Vector3 targetPos;

    public float springStrength;

    public float moveForce;

    public List<Transform> waypoints;
    public float distanceToWaypoint = 1f;
    public int placeInWaypoints = 0;


    public Sprite normalFace;
    public Sprite winceFace;
    public Sprite talkFace;

    public float winceTimer;

    public SpriteRenderer face;

    public AudioSource aud;
    public AudioClip omf;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();

        targetPos = returnRandomPos();

        face = GetComponent<SpriteRenderer>();

        r = transform.GetComponent<Rigidbody>();
        springStrength = .4f;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            segments.Add(transform.parent.GetChild(i).GetComponent<Rigidbody>());
        }

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 0 - i;
        }
    }

    public Vector3 returnRandomPos()
    {
        int random = Random.Range(0, waypointParent.transform.childCount);
        return waypointParent.GetChild(random).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (winceTimer > 0)
        {
            winceTimer -= Time.deltaTime;
        }
        else
        {

                face.sprite = normalFace;
            
           
        }

        if (Vector3.Distance(transform.position, targetPos) < distanceToWaypoint)
        {
            targetPos = returnRandomPos();
        }

        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        if (Vector3.Distance(transform.position, targetPos) < 5f)
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
            Debug.Log("HELLO");
            Vector3 dir = transform.position - collision.contacts[0].point;
            dir = dir.normalized;
            dir.z = 0;
            r.AddForce(dir * 75f);
            winceTimer = .25f;
            face.sprite = winceFace;
            if (aud.isPlaying == false)
            {
                aud.pitch = Random.Range(.90f, 1.13f);
                aud.Play();
            }


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
