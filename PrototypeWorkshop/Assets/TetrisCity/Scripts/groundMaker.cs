using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundMaker : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject groundPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject g = Instantiate(groundPrefab);
                g.transform.SetParent(this.transform);
                g.transform.position = new Vector3(x, 0, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
