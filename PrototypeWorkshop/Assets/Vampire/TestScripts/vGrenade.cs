using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vGrenade : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer;

    public float radius = 3;
    public float forceMult;

    void Start()
    {
        timer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Throw();
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    public void Throw()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        Vector3 v = new Vector3(0, 0, 0);
        v.y = 5f;
        v.x = Random.Range(-5, 5);
        v.z = Random.Range(-5, 5);

        v = v.normalized;
        v.y = 1.5f;
        r.AddForce(v * Time.deltaTime * forceMult, ForceMode.Impulse);
    }
    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject hitCollider = hitColliders[i].gameObject;
            if (hitColliders[i].gameObject.GetComponent<vampZombie>())
            {
                hitColliders[i].gameObject.GetComponent<vampZombie>().TakeDamage(25f);
            }

        }
        GameObject.Find("Particles").transform.Find("Explosion").GetComponent<ParticleSystem>().transform.position = transform.position;
        GameObject.Find("Particles").transform.Find("Explosion").GetComponent<ParticleSystem>().Play();

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
