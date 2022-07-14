using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vROcketLauncher : MonoBehaviour
{
    public float rocketTimer;
    public float timeBetweenShots;

    public Transform gunTransform;

    public GameObject bulletPrefab;
    public VampireController player;

    public float damage;

    int xDir, yDir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        xDir = player.xDir;
        yDir = player.yDir;

        if (rocketTimer > 0)
        {
            rocketTimer -= Time.deltaTime;

        }
        else
        {
            shootHandgun();
            rocketTimer = timeBetweenShots;
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
        GameObject g = Instantiate(bulletPrefab);
        g.transform.position = gunTransform.transform.position;
        g.GetComponent<vMissleAmmo>().OtherShoot(xDir, yDir, damage,gunTransform.transform.rotation.eulerAngles.z);
    }
}
