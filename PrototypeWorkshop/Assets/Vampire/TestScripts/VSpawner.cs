using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSpawner : MonoBehaviour
{
    public Transform zombieContainer;

    public Transform playerTransform;

    public GameObject zPrefab;

    public float timer;
    public float timeBetweenSpawns;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            SpawnZombie(transform.position);
            timer = timeBetweenSpawns + Random.Range(-.2f, .2f);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void SpawnZombie(Vector3 pos)
    {
        GameObject g = Instantiate(zPrefab);
        g.transform.position = pos;

        g.transform.SetParent(zombieContainer);
    }
}
