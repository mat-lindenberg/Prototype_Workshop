using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyPixelMover : MonoBehaviour
{
    public Vector3 newPos;
    public Vector3 originPos;

    public bool hasMovedThisTick;

    public Color c;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StyleMe(Color c, Vector3 _org)
    {
        GetComponent<Renderer>().material.color = c;
        originPos = _org;
    }

    public void Update()
    {
       // transform.position = Vector2.Lerp(transform.position, newPos, .2f);
    }

    // Update is called once per frame
    void Old()
    {

        newPos = Vector3.zero;

        if (transform.position.x < 0)
        {
            transform.position = new Vector3(1, transform.position.y, transform.position.z);
            
        }
        else if (transform.position.x > 90)
        {
            transform.position = new Vector3(89, transform.position.y, transform.position.z);
        }
        else
        {
            newPos.x = .25f;
            if (Random.Range(0,10)>= 5)
            {
                newPos.x = -.25f;
            }
        }

        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x,1, transform.position.z);
        }
        else if (transform.position.y > 90)
        {
            transform.position = new Vector3(transform.position.x, 89, transform.position.z);

        }
        else
        {
            newPos.y = .25f;
            if (Random.Range(0, 10) >= 5)
            {
                newPos.y = -.25f;
            }
        }

        transform.position += newPos;

    }
}
