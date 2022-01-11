using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWater : MonoBehaviour
{
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;

        if (transform.position.y <= 0)
        {
           // GameObject.Find("Splash").transform.position = transform.position;
           // GameObject.Find("Splash").GetComponent<ParticleSystem>().Play();
            //destroy
            transform.position = Vector3.zero;
            transform.gameObject.SetActive(false);

        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Vessel")
        {
            GameObject.Find("Splash").transform.position = transform.position;
            GameObject.Find("Splash").GetComponent<ParticleSystem>().Play();
            transform.position = Vector3.zero;
            transform.gameObject.SetActive(false);
            other.GetComponent<simpleVessel>().recieveWater(1);



        }
    }
}
