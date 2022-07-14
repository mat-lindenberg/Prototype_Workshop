using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vGrenadeAmmo : MonoBehaviour
{
    public float speedMult;

    public float xDir;
    public float yDir;

    public float damage;
    float radius = 1;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Explode();
        }


        //transform.position += new Vector3(xDir, 0, yDir) * speedMult * Time.deltaTime;
    }

    public void Throw(float _x, float _z)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        Vector3 v = new Vector3(0, 0, 0);
        v.y = 0f;
        v.x = _x;
        v.z = _z;

        v = v.normalized;
        v.y = .2f;
        r.AddForce(v * Time.deltaTime * speedMult, ForceMode.Impulse);
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject hitCollider = hitColliders[i].gameObject;
            if (hitColliders[i].gameObject.GetComponent<vampZombie>())
            {
                hitColliders[i].gameObject.GetComponent<vampZombie>().TakeDamage(damage);
            }

        }
        GameObject.Find("Particles").transform.Find("Explosion").GetComponent<ParticleSystem>().transform.position = transform.position;
        GameObject.Find("Particles").transform.Find("Explosion").GetComponent<ParticleSystem>().Play();

        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<vampZombie>())
        {
            Explode();
            Destroy(collision.gameObject);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
