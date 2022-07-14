using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vMolotov : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer;

    public float radius = 3;
    public float forceMult;

    public GameObject firePrefab;

    void Start()
    {
        timer = 2;
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
        GameObject g = Instantiate(firePrefab);
        g.transform.position = transform.position;
        g.transform.position = new Vector3(g.transform.position.x, 0, g.transform.position.z);

        Destroy(gameObject);
    }


}
