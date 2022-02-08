using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vampZombie : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;

    public float moveMult;
    public float maximumVelo;

    Rigidbody r;

    void Start()
    {
        playerTransform = GameObject.Find("Player").gameObject.transform;
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVector = playerTransform.position - transform.position;
        inputVector = inputVector.normalized;

        r.AddForce(inputVector * moveMult);

        if (r.velocity.magnitude > maximumVelo)
        {
            r.velocity = Vector3.ClampMagnitude(r.velocity, maximumVelo);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<VampireBullet>())
        {
            Destroy(gameObject);
        }
    }

    public void TakeExplosiveDamage()
    {
        Destroy(gameObject);
    }
}
