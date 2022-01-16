using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafVolume : MonoBehaviour
{
    public GameObject leafPrefab;
    public GameObject leafPrefab2;
    public GameObject leadPrefab3;

    

    // Start is called before the first frame update
    void Start()
    {
        Spawnleafs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawnleafs()
    {
        Vector3 pos = transform.position;
        Vector3 size = GetComponent<Renderer>().bounds.size;

        for (int i = 0; i < 50; i++)
        {
            GameObject g = Instantiate(leafPrefab);

            g.transform.position = pos + new Vector3(Random.Range(-size.x/2f,size.x/2f),.2f, Random.Range(-size.z/2f, size.z/2f));
        }

        

    }
}
