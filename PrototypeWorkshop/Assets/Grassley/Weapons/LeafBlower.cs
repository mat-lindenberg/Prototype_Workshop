using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : MonoBehaviour
{

    int myState = 0; //0 is idle, 1 is charge, 2 swing, 3 is retract

    public bool mouseHeldLastFrame;
    public bool mouseHeldThisFrame;

    public float blowerMaxForce;
    public float blowerUpwardForce;
    public float torqueForce;
    public float blowerForce;
    public float blowerSpeed;

    public Transform blowerTransform;

    public RakeCollider triggerVolume;

    public ParticleSystem ps;


    public Sprite[] lfWindArray;
    public int placeInArray;
    public float lengthOfTimeBetweenSpriteChanges;
    public float spriteChangeTimer;

    public GameObject blowerSprite;
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        if (blowerSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            blowerSprite.transform.rotation = Quaternion.Lerp(blowerSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), rotSpeed);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            blowerSprite.transform.rotation = Quaternion.Lerp(blowerSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), rotSpeed);
        }


        if (mouse_pos.x > transform.position.x)
        {
            blowerSprite.transform.localScale = new Vector3(1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            blowerSprite.transform.localScale = new Vector3(-1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = true;
        }


        if (Input.GetMouseButtonDown(0))
        {
            mouseHeldThisFrame = true;
            if (myState == 0)
            {
               //ps.transform.position = blowerTransform.position;
               // ps.transform.rotation = blowerTransform.rotation;
                ps.Play();
                myState = 1;
            }
        } 

        if (Input.GetMouseButton(0))
        {
            mouseHeldThisFrame = true;
           // ps.transform.position = blowerTransform.position;
          //  ps.transform.rotation = blowerTransform.rotation;

            if (myState == 1)
            {
                if (blowerForce < blowerMaxForce)
                {
                    blowerForce += Time.deltaTime * blowerSpeed;
                }

                for (int i = 0; i < triggerVolume.leafs.Count; i++)
                {
                    Vector3 dir = triggerVolume.transform.position - transform.position;
                    dir = dir.normalized;
                    dir.y = blowerUpwardForce;
                   
                    Vector3 pos = triggerVolume.leafs[i].transform.position;
                    triggerVolume.leafs[i].gameObject.GetComponent<Rigidbody>().AddForce((dir * blowerForce));
                    triggerVolume.leafs[i].gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.up * torqueForce);
                }
            }

        }

        if (mouseHeldLastFrame == true && mouseHeldThisFrame == false)
        {
            //relesase
            if (myState == 1)
            {
                myState = 0;
                blowerForce = 0;
                ps.Stop();
                
            }
        }

        mouseHeldLastFrame = mouseHeldThisFrame;
        mouseHeldThisFrame = false;
    }
}
