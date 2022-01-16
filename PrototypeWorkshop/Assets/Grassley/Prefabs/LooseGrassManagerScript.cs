using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseGrassManagerScript : MonoBehaviour
{
    public GameObject pooledObject;
    public int pooledAmount = 20;



    public List<GameObject> pooledObjects;

    public bool willGrow = false;

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAtPoint(Vector3 point)
    {
        GetPooledObject().transform.position = point;
        GameObject obj = GetPooledObject();
        obj.transform.position = point;
        obj.transform.rotation = Random.rotation;
        obj.SetActive(true);
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                GameObject obj = (GameObject)Instantiate(pooledObject);
                obj.SetActive(false);
                pooledObjects[i] = obj;
                pooledObjects[i].transform.parent = this.transform;
                return pooledObjects[i];
            }
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
