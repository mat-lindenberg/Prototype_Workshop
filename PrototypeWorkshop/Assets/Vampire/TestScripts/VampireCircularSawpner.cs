using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireCircularSawpner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform zombieContainer;

    public Transform playerTransform;

    public GameObject zPrefab;

    public float timer;
    public float timeBetweenSpawns;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            SpawnZombie();
            timer = timeBetweenSpawns + Random.Range(-.2f, .2f);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void SpawnZombie()
    {
        GameObject g = Instantiate(zPrefab);

        float rando = Random.Range(0, 20);

        float xPos = playerTransform.position.x;
        float zPos = playerTransform.position.z;
        float yPos = 0;


        if (rando <= 1)
        {
            xPos += 12;
            float zPosMod = Random.Range(0,10);
            zPos += zPosMod;
        }
        else if (rando <= 2)
        {
            xPos -= 12;
            float zPosMod = Random.Range(0, 10);
            zPos += zPosMod;
        }
        else if (rando <= 14)
        {
            zPos += 14;
            float xposMod = Random.Range(-20, 20);
            xPos += xposMod;
        }
        else if (rando <= 20)
        {
            zPos -= 8;
            float xposMod = Random.Range(-20, 20);
            xPos += xposMod;
        }





        //g.transform.position
        g.transform.position = new Vector3(xPos,0,zPos);
        g.transform.SetParent(zombieContainer);
    }
}
