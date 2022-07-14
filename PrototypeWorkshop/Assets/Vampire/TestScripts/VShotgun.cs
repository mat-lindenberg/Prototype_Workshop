using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VShotgun : MonoBehaviour
{
    // Start is called before the first frame update

    public float cooldownTimer;
    public float cooldownModification;
    public float cooldown;

    public int bulletsInClip;
    public int shotsPerClip;

    public float reloadTimer;
    public float reloadModification;
    public float timeItTakesToReload;

    public Transform gunTransform;

    public GameObject bulletPrefab;
    public VampireController player;

    public float pellets;
    public float spreadAngle;

    public Sprite normalPic;
    public Sprite reloadPic;

    public float damage;

    float xDir, yDir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xDir = player.xDir;
        yDir = player.yDir;


        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime * reloadModification;

            if (reloadTimer <= 0)
            {
                bulletsInClip = shotsPerClip;
                cooldownTimer = 0;
                GetComponent<SpriteRenderer>().sprite = normalPic;
            }
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime * cooldownModification;
        }
        else
        {
            cooldownTimer = cooldown;
            shootHandgun();
            bulletsInClip--;

            if (bulletsInClip <= 0)
            {
                reloadTimer = timeItTakesToReload;
                GetComponent<SpriteRenderer>().sprite = reloadPic;
            }
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
        for (int i = 0; i < pellets; i++)
        {
            GameObject g = Instantiate(bulletPrefab);
            g.transform.position = gunTransform.transform.position;
            //g.GetComponent<VampireBullet>().OtherShoot(FindAngleFacing());
            g.GetComponent<VampireBullet>().OnShoot(xDir + Random.Range(-spreadAngle, spreadAngle), yDir + Random.Range(-spreadAngle, spreadAngle),damage);
        }

    }

    public float FindAngleFacing()
    {
        if (xDir == -1 && yDir == -1)
        {
            return 225;
        }
        if (xDir == -1 && yDir == 0)
        {
            return 180;
        }
        if (xDir == -1 && yDir == 1)
        {
            return 135;
        }

        if (xDir == 0 && yDir == -1)
        {
            return 270f;
        }
        if (xDir == 0 && yDir == 0)
        {
            return 90f;
        }
        if (xDir == 0 && yDir == 1)
        {
            return 90f;
        }

        if (xDir == 1 && yDir == -1)
        {
            return 315;
        }
        if (xDir == 1 && yDir == 0)
        {
            return 0;
        }
        if (xDir == 1 && yDir == 1)
        {
            return 45;
        }

        return 0;
    }
}
