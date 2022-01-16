using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxCharge;
    public float chargeMeter;

    int myState = 0; //0 is idle, 1 is charge, 2 swing, 3 is retract

    public bool mouseHeldLastFrame;
    public bool mouseHeldThisFrame;

    public float leafForce;

    public Transform player;

    public Vector3 lastPos;
    public Vector3 curPos;

    public float veloThresh;
    public float velo;

    public GameObject gunSprite;
    public float rotSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curPos = Input.mousePosition;
        velo = (curPos - lastPos).magnitude;
        velo = Mathf.Abs(velo);

        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        if (gunSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            gunSprite.transform.rotation = Quaternion.Lerp(gunSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), rotSpeed);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            //gunSprite.transform.rotation = Quaternion.Lerp(gunSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), rotSpeed);
        }


        if (mouse_pos.x > transform.position.x)
        {
           //gunSprite.transform.localScale = new Vector3(1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            //gunSprite.transform.localScale = new Vector3(-1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = true;
        }


        if (Input.GetMouseButtonDown(0))
        {
            mouseHeldThisFrame = true;
            if (myState == 0)
            {
                myState = 1;
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

        if (mouseHeldLastFrame == true && mouseHeldThisFrame == false)
        {
            //relesase
            if (myState == 1)
            {
                myState = 0;
                chargeMeter = 0;
            }
        }

        mouseHeldLastFrame = mouseHeldThisFrame;
        mouseHeldThisFrame = false;




        lastPos = curPos;
    }
}
