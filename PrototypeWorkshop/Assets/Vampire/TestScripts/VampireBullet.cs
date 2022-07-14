using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBullet : MonoBehaviour
{
    public float speedMult;

    public float xDir;
    public float yDir;

    public float damage;

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

    public void OnShoot(float _xDir, float _yDir, float _damage)
    {
        xDir = _xDir;
        yDir = _yDir;
        damage = _damage;
    }

    public void OtherShoot(float _xDir, float _yDir, float _damage, float f)
    {
        xDir = _xDir;
        yDir = _yDir;
        damage = _damage;
        transform.rotation = Quaternion.Euler(45, 0, f);
    }
}
