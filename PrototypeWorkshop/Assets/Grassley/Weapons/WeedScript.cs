using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class WeedScript : MonoBehaviour
{
    public GameObject top;
    public GameObject rootBall;

    public bool topCut;
    public bool popped;

    public float popForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PopUp(Transform playerPos)
    {
        if (popped == true)
        {
            return;
        }

        GetComponent<Rigidbody>().isKinematic = false;
        Vector3 dir = transform.position - playerPos.position;
        dir = dir.normalized;
        dir.y = 1f;

        GetComponent<Rigidbody>().AddForce(dir * popForce);
        GetComponent<Rigidbody>().AddTorque((dir * popForce)/2f);
        popped = true;
        GameObject.Find("LandscapeGameRunner").GetComponent<LandscapeGameRunner>().AddToTimer(.5f);

        GameObject.Find("GrassManager").GetComponent<GrassManager>().MakeDirtParticlesAtLocation(transform.position + new Vector3(0, .1f, 0),dir);
        rootBall.GetComponent<SpriteRenderer>().sortingOrder = 3;
        popped = true;
    }

    public void CutTop()
    {
        if (popped == true)
        {
            return;
        }

        if (topCut == false)
        {
            GameObject.Find("GrassManager").GetComponent<GrassManager>().MakeParticleAtLocation(transform.position + new Vector3(0, .1f, 0));
            GameObject.Find("LooseGrassManager").GetComponent<LooseGrassManagerScript>().SpawnAtPoint(transform.position + new Vector3(0, .1f, 0));
            top.SetActive(false);
            rootBall.GetComponent<SpriteRenderer>().sortingOrder = 3;
            topCut = true;
            GetComponent<BoxCollider>().isTrigger = true;
        }

    }
}
