using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : MonoBehaviour
{

    int state = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CutMe()
    {
        if (state == 0)
        {
            GetComponent<SpriteRenderer>().sprite = GameObject.Find("GrassManager").GetComponent<GrassManager>().cutGrassSprite;
            GameObject.Find("GrassManager").GetComponent<GrassManager>().MakeParticleAtLocation(transform.position + new Vector3(0, .1f, 0));
            GameObject.Find("LooseGrassManager").GetComponent<LooseGrassManagerScript>().SpawnAtPoint(transform.position + new Vector3(0, .1f, 0));
            GameObject.Find("LandscapeGameRunner").GetComponent<LandscapeGameRunner>().AddToTimer(.1f);
            state = 1;

        }
        else
        {

        }

    }
}
