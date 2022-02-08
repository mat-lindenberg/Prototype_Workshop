using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBullet : MonoBehaviour
{
    public float speedMult;

    public float xDir;
    public float yDir;

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

    public void OnShoot(int _xDir, int _yDir)
    {
        xDir = _xDir;
        yDir = _yDir;
    }
}
