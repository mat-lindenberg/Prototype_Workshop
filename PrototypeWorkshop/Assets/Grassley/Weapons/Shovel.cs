using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : MonoBehaviour
{

    public Transform colPoint;

    public Transform playerPoint;

    public Transform rake;
    public ShovelCollider shovelCollider;

    public float lowZ;
    public float highZ;

    public float maxCharge;
    public float chargeMeter;

    int myState = 0; //0 is idle, 1 is charge, 2 swing, 3 is retract

    public bool mouseHeldLastFrame;
    public bool mouseHeldThisFrame;

    public GameObject shovelSprite;

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

        if (shovelSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            shovelSprite.transform.rotation = Quaternion.Lerp(shovelSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), .9f);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            shovelSprite.transform.rotation = Quaternion.Lerp(shovelSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), .9f);
        }


        if (mouse_pos.x > transform.position.x)
        {
            //playerGunSpriteTransform.localScale = new Vector3(1, 1, 1);
            shovelSprite.GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            //playerGunSpriteTransform.localScale = new Vector3(-1, 1, 1);
            shovelSprite.GetComponent<SpriteRenderer>().flipY = true;
        }


        if (Input.GetMouseButtonDown(0))
        {
            mouseHeldThisFrame = true;
            if (myState == 0)
            {
                myState = 1;

                float z = colPoint.transform.position.z;
                z -= transform.position.z;
                lowZ = z;
                highZ = z;
                
            }
        }

        if (Input.GetMouseButton(0))
        {
            mouseHeldThisFrame = true;

            if (myState == 1)
            {
                if (chargeMeter < maxCharge)
                {
                    chargeMeter += Time.deltaTime;
                }
            }

        }

        if (myState == 1)
        {
            float z = colPoint.transform.position.z;
            z -= transform.position.z;
            if (z < lowZ)
            {
                lowZ = z;
            }
            if (z > highZ)
            {
                highZ = z;
            }

            if (Mathf.Abs(lowZ - highZ) > .5f)
            {
                Debug.Log("WE GOOD!");
                lowZ = 0;
                highZ = 0;

                for (int i = 0; i < shovelCollider.weeds.Count; i++)
                {
                    if (shovelCollider.weeds[i].GetComponent<WeedScript>())
                    {
                        shovelCollider.weeds[i].GetComponent<WeedScript>().PopUp(playerPoint.transform);
                    }
                    else if (shovelCollider.weeds[i].GetComponent<FlowerScript>())
                    {
                        shovelCollider.weeds[i].GetComponent<FlowerScript>().PopUp(playerPoint.transform);
                    }
                        
                    
                }

                shovelCollider.weeds.Clear();

            }
        }

        if (mouseHeldLastFrame == true && mouseHeldThisFrame == false)
        {

            if (myState == 1)
            {
                myState = 0;
                chargeMeter = 0;
                rake.transform.localPosition = Vector3.zero;
            }
        }

        mouseHeldLastFrame = mouseHeldThisFrame;
        mouseHeldThisFrame = false;


    }
}
