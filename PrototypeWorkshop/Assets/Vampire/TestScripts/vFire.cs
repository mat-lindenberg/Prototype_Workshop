using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vFire : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTimer;
    void Start()
    {
        lifeTimer = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vampZombie>())
        {
            other.GetComponent<vampZombie>().TakeDamage(.5f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<vampZombie>())
        {
            other.GetComponent<vampZombie>().TakeDamage(.5f);
        }
    }
}
