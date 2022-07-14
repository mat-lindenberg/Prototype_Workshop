using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPistol : MonoBehaviour
{
    public float handgunTimer;
    public float timeBetweenShots;

    public Transform gunTransform;

    public GameObject bulletPrefab;
    public VampireController player;

    public float damage;

    int xDir, yDir;

    public bool backwards;
    public bool sideways;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xDir = player.xDir;
        yDir = player.yDir;

        if (backwards == true)
        {
            xDir *= -1;
            yDir *= -1;
        }

        if (handgunTimer > 0)
        {
            handgunTimer -= Time.deltaTime;

        }
        else
        {
            shootHandgun();
            handgunTimer = timeBetweenShots;
        }


        if (xDir == -1 && yDir == 0)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = true;
            gunTransform.rotation = Quaternion.Euler(45, 0, 0);
        }
        if (xDir == 1 && yDir == 0)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = false;
            gunTransform.rotation = Quaternion.Euler(45, 0, 0);
        }

        if (yDir == 1 && xDir == 0)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = false;
            gunTransform.rotation = Quaternion.Euler(45, 0, 90);
        }
        if (yDir == -1 && xDir == 0)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = false;
            gunTransform.rotation = Quaternion.Euler(45, 0, -90);
        }

        if (xDir == 1 && yDir == -1)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = false;
            gunTransform.rotation = Quaternion.Euler(45, 0, -45);
        }
        if (xDir == -1 & yDir == -1)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = true;
            gunTransform.rotation = Quaternion.Euler(45, 0, 45);
        }
        if (xDir == -1 && yDir == 1)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = true;
            gunTransform.rotation = Quaternion.Euler(45, 0, -45);
        }
        if (xDir == 1 && yDir == 1)
        {
            gunTransform.GetComponent<SpriteRenderer>().flipX = false;
            gunTransform.rotation = Quaternion.Euler(45, 0, 45);
        }
    }

    public void shootHandgun()
    {
        if (sideways == true)
        {
            if (xDir == 0 && yDir != 0)
            {
                xDir = 1;
                yDir = 0;
            }
            else if (xDir != 0 && yDir == 0)
            {
                xDir = 0;
                yDir = 1;
            }


            GameObject g = Instantiate(bulletPrefab);
            g.transform.position = gunTransform.transform.position;
            g.GetComponent<VampireBullet>().OnShoot(xDir, yDir, damage);

            GameObject g2 = Instantiate(bulletPrefab);
            g2.transform.position = gunTransform.transform.position;
            g2.GetComponent<VampireBullet>().OnShoot(xDir *= -1, yDir *= -1, damage);
        }
        else
        {
            GameObject g = Instantiate(bulletPrefab);
            g.transform.position = gunTransform.transform.position;
            g.GetComponent<VampireBullet>().OnShoot(xDir, yDir, damage);
        }


    }
}
