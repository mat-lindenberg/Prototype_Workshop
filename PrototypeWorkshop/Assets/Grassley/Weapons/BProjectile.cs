using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BProjectile : MonoBehaviour
{
    public Vector3 targetPos;
    public Transform player;


    public float rotSpeed;

    public float yPos;

    public int myState = 0; //0 is off, 1 is moving towards, 2 is moving towards player

    public float veloMod;
    public float veloMax;
    public float veloMin;

    public AnimationCurve bCurve;

    // Start is called before the first frame update
    void Start()
    {
        DeactivateProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        if (myState == 1)
        {
            transform.Rotate(0, 0, rotSpeed, Space.Self);

            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance < .3f)
            {
                myState = 2;
                veloMod = veloMin;
            }
            if (distance > 15)
            {
                distance = 15;
            }

            float dThresh = 5f;
            float dDif = dThresh - distance;

            Vector3 dir = targetPos - transform.position;
            dir.y = .2f;
            dir = dir.normalized;

            transform.position = transform.position + ((dir * bCurve.Evaluate(dDif / 5f) * Time.deltaTime));
        }
        
        if (myState == 2)
        {
            transform.Rotate(0, 0, rotSpeed, Space.Self);

            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < .3f)
            {
                myState = 0;
                player.transform.Find("Boomerang").GetComponent<Boomerang>().ReturnWang();
                DeactivateProjectile();
            }
            else
            {
                Vector3 dir = player.transform.position - transform.position;
                dir = dir.normalized;
                transform.position = transform.position + ((dir * veloMod) * Time.deltaTime);

                if (veloMod < veloMax)
                {
                    veloMod *= 1.01f;
                }
            }
        }
        
    }

    public void ActivateProjectile(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        transform.position = player.transform.position;
        gameObject.SetActive(true);
        myState = 1;
    }

    public void DeactivateProjectile()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grass")
        {
            other.GetComponent<GrassScript>().CutMe();
        }
        if (other.tag == "Weed")
        {
            other.GetComponent<WeedScript>().CutTop();
        }
        if (other.tag == "Flower")
        {
            other.GetComponent<FlowerScript>().CutTop();
        }
        if (other.tag == "Bush")
        {
            other.GetComponent<BushScript>().ClipMe();
        }
    }
}
