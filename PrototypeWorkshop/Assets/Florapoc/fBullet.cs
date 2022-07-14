using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 dir;
    float speed = .16f;

    float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position + (dir * speed);
        GetComponent<Rigidbody>().MovePosition(pos);

        timer += Time.deltaTime;
        if (timer > 3)
        {
            Destroy(gameObject);
        }
    }
}
