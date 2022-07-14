using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floraGun1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float handgunTimer;
    public float timeBetweenShots;

    public Transform gunTransform;

    public GameObject bulletPrefab;
    public FPlayerController player;

    public float damage;

    public Transform mousePos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        GameObject g = Instantiate(bulletPrefab);
        g.transform.position = gunTransform.position;

        Vector3 v = mousePos.position - transform.position;
        v.y = 0;
        v = v.normalized;
        v.y = 0;
        g.GetComponent<fBullet>().dir = v;

    }
}
