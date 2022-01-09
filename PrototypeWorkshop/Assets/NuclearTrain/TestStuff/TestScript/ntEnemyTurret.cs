using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ntEnemyTurret : MonoBehaviour
{
    // Start is called before the first frame update

    public int health = 5;

    public Transform player;

    public GameObject bulletPrefab;

    public Transform shootPoint;
    public Transform turretTracking;

    public float timer;
    public float timeBetweenShots = 5;
    void Start()
    {
        timer = Random.Range(1, 4);
        timeBetweenShots = Random.Range(4, 6);

        player = GameObject.Find("MidnightMatTrain").GetComponent<Transform>();
        turretTracking = transform.parent.Find("TurretRot").GetComponent<Transform>();
        shootPoint = turretTracking.Find("ShootPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        turretTracking.LookAt(player);


        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            Shoot();
            timer = 0;
        }
    }

    public void Shoot()
    {
        GameObject g = Instantiate(bulletPrefab);
        g.transform.position = shootPoint.transform.position;

        g.GetComponent<ntBullet>().OnCreation(player.transform.position);
    }
    public void TakeDamage(int i)
    {
        health -= i;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(transform.parent.gameObject);
    }
}
