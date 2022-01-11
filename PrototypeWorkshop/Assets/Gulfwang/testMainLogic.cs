using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMainLogic : MonoBehaviour
{

    public GameObject golfBallObject;
    public GameObject clubObject;
    public GameObject holeObject;

    public GameObject startPlace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Hit restart");
            golfBallObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            golfBallObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            golfBallObject.transform.position = startPlace.transform.position;
        }
            //restart

        
    }
}
