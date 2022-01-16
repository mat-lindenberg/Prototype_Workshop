using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public int bState = 0; // 0 is idle, 1 is charging, 2 is thrown and returning

    public bool mouseHeldLastFrame;
    public bool mouseHeldThisFrame;

    public float maxCharge;
    public float chargeMeter;

    public GameObject boomerangVisMarker;

    public GameObject player;

    public Vector3 boomerangTargetPosition;
    public GameObject boomerangObject;
    public GameObject boomer2;

    public int ammo;


    public GameObject handSprite;
    public Vector3 restPos;

    public GameObject boomerSprite;
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

        if (boomerSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            boomerSprite.transform.rotation = Quaternion.Lerp(boomerSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), rotSpeed);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            //boomerSprite.transform.rotation = Quaternion.Lerp(gunSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), rotSpeed);
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

        if (ammo == 0)
        {
            handSprite.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseHeldThisFrame = true;
            if (bState == 0)
            {
                bState = 1;

            }
        }

        if (Input.GetMouseButton(0))
        {
            mouseHeldThisFrame = true;
            if (bState == 0)
            {
                bState = 1;
            }

        }

        if (bState == 1)
        {
           
            if (chargeMeter < maxCharge)
            {
                chargeMeter += Time.deltaTime;
            }

            handSprite.transform.localPosition = Vector3.Lerp(Vector3.zero, (handSprite.transform.position - mouse_pos).normalized * .40f, chargeMeter / maxCharge);
            //handSprite.transform.localPosition = Vector3.Lerp(Vector3.zero, restPos, chargeMeter / maxCharge);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 7;
            if (Physics.Raycast(ray, out hit, 500f,layerMask))
            {
                Vector3 dir = hit.point;
                dir = hit.point - transform.position;
                dir = dir.normalized;
                dir.y = .2f;
                boomerangVisMarker.transform.position = Vector3.Lerp(player.transform.position, player.transform.position + (dir * 4f), chargeMeter / maxCharge);
            }
        }

        if (mouseHeldLastFrame == true && mouseHeldThisFrame == false)
        {
            boomerangTargetPosition = boomerangVisMarker.transform.position;
            //release
            chargeMeter = 0;
            handSprite.transform.localPosition = Vector3.zero;
            if (bState == 1 && ammo >= 1)
            {
                if (boomer2.gameObject.activeSelf == true)
                {
                    bState = 0;
                    boomerangObject.GetComponent<BProjectile>().ActivateProjectile(boomerangTargetPosition);
                    boomerangObject.SetActive(true);
                    ammo -= 1;
                }
                else
                {
                    bState = 0;
                    boomer2.GetComponent<BProjectile>().ActivateProjectile(boomerangTargetPosition);
                    boomer2.SetActive(true);
                    ammo -= 1;
                }

            }

        }

        if (bState == 2)
        {


        }

        if (bState == 3)
        {


        }

        mouseHeldLastFrame = mouseHeldThisFrame;
        mouseHeldThisFrame = false;
    }

    public void ReturnWang()
    {
        bState = 0;
        ammo++;
        handSprite.GetComponent<SpriteRenderer>().enabled = true;
    }
}
