using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vGrenadeHolster : MonoBehaviour
{

    public float grenadeTimer;
    public float timeBetweenGrenades;

    public Vector3 forceMult;
    public float radius;

    public GameObject grenadePrefab;
    public Transform handTransform;
    public VampireController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (grenadeTimer > 0)
        {
            grenadeTimer -= Time.deltaTime;

        }
        else
        {
            throwGrenade();
            grenadeTimer = timeBetweenGrenades;
        }
    }

    public void throwGrenade()
    {
        GameObject g = Instantiate(grenadePrefab);
        g.transform.position = handTransform.transform.position;
        if (g.GetComponent<vGrenade>()){
            g.GetComponent<vGrenade>().Throw();
        }

        if (g.GetComponent<vMolotov>())
        {
            g.GetComponent<vMolotov>().Throw();
        }

    }
}
