using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{

    public float moveSpeed;

    public GameObject bowl;
    public Transform hands;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (bowl.transform.parent == hands)
            {
                bowl.transform.SetParent(null);
                bowl.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                bowl.transform.SetParent(hands);
                bowl.GetComponent<Rigidbody>().isKinematic = true;
                bowl.transform.localPosition = new Vector3(0, 0, 0);
            }


        }
    }
}
