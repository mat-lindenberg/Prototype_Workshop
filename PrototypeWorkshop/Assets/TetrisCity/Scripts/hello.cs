using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hello : MonoBehaviour
{
    public GameObject cube;

    public LayerMask mask;

    public GameObject[] prefabs;

    public Transform buildingContainer;

    public Vector3 cent;
    public Vector3 gsize;

    // Start is called before the first frame update
    void Start()
    {
        cube = newThing();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50f, mask))
        {
            cube.transform.position = returnGridPoint(hit.point);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotateCurrentBuilding(cube);
        }

        if (canPlaceHere())
        {
            foreach (Renderer r in cube.GetComponentsInChildren<Renderer>())
            {
                r.material.color = Color.white;
            }
        }
        else
        {
            foreach (Renderer r in cube.GetComponentsInChildren<Renderer>())
            {
                r.material.color = Color.red;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //place building
            if (canPlaceHere())
            {
                placeBuilding();
                cube = newThing();
            }

        }
    }

    public void placeBuilding()
    {
        foreach (BoxCollider b in cube.GetComponents<BoxCollider>())
        {
            b.enabled = true;
        }

        Color c = Random.ColorHSV();
        foreach (Renderer r in cube.GetComponentsInChildren<Renderer>())
        {
            r.material.color = c;
        }

        cube.transform.SetParent(buildingContainer);
    }

    public GameObject newThing()
    {
        GameObject g = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        g.transform.SetParent(buildingContainer);
        foreach(BoxCollider b in g.GetComponents<BoxCollider>())
        {
            b.enabled = false;
        }
        return g;
    }

    public void rotateCurrentBuilding(GameObject g)
    {
        g.transform.Rotate(0f, 90f, 0f);
    }

    public Vector3 returnGridPoint(Vector3 point)
    {

        float x = point.x;
        float y = point.y;
        float z = point.z;

        x = Mathf.Round(x);
        y = y;
        z = Mathf.Round(z);

        return new Vector3(x, y, z);

    }

    public bool canPlaceHere()
    {
        bool isGood = true;

        foreach( Transform t in cube.GetComponentsInChildren<Transform>())
        {
            Vector3 rayStart = t.position;
            rayStart += new Vector3(0, 5, 0);

            RaycastHit hit;
            Ray ray = new Ray(rayStart, new Vector3(0f, -1f, 0f));
            if (Physics.Raycast(ray, out hit))
            {
                //cube.transform.position = returnGridPoint(hit.point);
                if (hit.collider.gameObject.layer == 8)
                {

                }else if (hit.collider.gameObject.layer == 9)
                {
                    
                    return false;
                }
                
            }
            else
            {
                //Debug.Log(hit.collider.gameObject.name);
                //isGood = false;
            }

        }

        return isGood;

    }
}