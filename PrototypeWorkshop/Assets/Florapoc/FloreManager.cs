using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject seedPrefab;
    public GameObject weedPrefab;

    public int weedCount;
    public int maxWeeds = 100;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SeedHere(FloraSeedScript seed)
    {
        if (weedCount < maxWeeds)
        {
            GameObject g = Instantiate(weedPrefab);
            g.transform.position = seed.transform.position;
            weedCount++;
            Destroy(seed.gameObject);
        }
        else
        {
            Destroy(seed.gameObject);
        }
    }
}
