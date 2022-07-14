using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vMissleAmmo : MonoBehaviour
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
            Destroy(gameObject);
        }


        transform.position += new Vector3(xDir, 0, yDir) * speedMult * Time.deltaTime;
    }

    public void OtherShoot(float _xDir, float _yDir, float _damage, float f)
    {
        xDir = _xDir;
        yDir = _yDir;
        damage = _damage;
        transform.rotation = Quaternion.Euler(45, 0, f);
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
