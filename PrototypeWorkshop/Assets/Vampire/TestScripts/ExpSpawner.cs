using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSpawner : MonoBehaviour
{
    public GameObject expPrefab;
    public GameObject medkitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeathOfMonster(Vector3 pos)
    {
        float r = Random.Range(0, 100);

        if (r < 35)
        {
            GameObject g = Instantiate(expPrefab);
            pos.y = .5f;
            g.transform.position = pos;
        }else if (r > 99)
        {
            GameObject g = Instantiate(medkitPrefab);
            pos.y = .5f;
            g.transform.position = pos;
        }
    }
}
