using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seasonStump : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float f)
    {
        health -= f;

        GameObject.Find("Particles").transform.Find("Sparks").GetComponent<ParticleSystem>().transform.position = transform.position;
        GameObject.Find("Particles").transform.Find("Sparks").GetComponent<ParticleSystem>().Play();

        if (health <= 0)
        {
            
            GameObject.Find("Particles").transform.Find("BreakObject").GetComponent<ParticleSystem>().transform.position = transform.position;
            GameObject.Find("Particles").transform.Find("BreakObject").GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }
}
