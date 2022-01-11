using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeather : MonoBehaviour
{
    public float timeBetween;
    public float timer;

    public GameObject waterDropletPrefab;

    public int waterPoolSize;
    public int placeInWaterPool;

    public GameObject[] waterPool;

    // Start is called before the first frame update
    void Start()
    {
        waterPool = new GameObject[waterPoolSize];
        for (int i = 0; i < waterPoolSize; i++)
        {
            GameObject g = Instantiate(waterDropletPrefab);
            g.transform.SetParent(transform);
            g.SetActive(false);
            waterPool[i] = g;
        }
    }

    public GameObject returnWater()
    {
        placeInWaterPool++;
        if (placeInWaterPool >= waterPoolSize)
        {
            placeInWaterPool = 0;
        }
        GameObject g = waterPool[placeInWaterPool];

        return g;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBetween)
        {
            timer = 0;
            GameObject g = returnWater();
            g.transform.position = new Vector3(Random.Range(-5.00f, 5.00f), 4, Random.Range(-5.00f, 5.00f));
            g.SetActive(true);
        }
    }
}
