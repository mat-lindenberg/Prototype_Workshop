using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ntBullet : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 targetPos;
    Rigidbody r;

    float timer;
    void Start()
    {
        timer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Turret")
        {
            collision.gameObject.GetComponent<ntEnemyTurret>().TakeDamage(2);
            Destroy(gameObject);
        }
    }
    public void OnCreation(Vector3 targetPos)
    {
        r = GetComponent<Rigidbody>();
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        dir *= 3f;
        r.AddForce(dir,ForceMode.Impulse);
    }
}
