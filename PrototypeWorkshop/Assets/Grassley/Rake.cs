using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rake : MonoBehaviour
{
    public Transform colPoint;

    public Transform playerPoint;

    public Transform rake;

    public float maxCharge;
    public float chargeMeter;

    public Vector3 rakeRestPos;

    int myState = 0; //0 is idle, 1 is charge, 2 swing, 3 is retract

    public bool mouseHeldLastFrame;
    public bool mouseHeldThisFrame;

    public float pullMove;
    public float pullForce;
    public float pushMove;
    public float pushForce;
    public RakeCollider rakeCollider;

    public GameObject rakeSprite;
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


        if (rakeSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rakeSprite.transform.rotation = Quaternion.Lerp(rakeSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), rotSpeed);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            rakeSprite.transform.rotation = Quaternion.Lerp(rakeSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), rotSpeed);
        }


        if (mouse_pos.x > transform.position.x)
        {
            rakeSprite.transform.localScale = new Vector3(1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            rakeSprite.transform.localScale = new Vector3(-1, 1, 1);
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


        if (Input.GetMouseButton(1))
        {
            //Debug.Log("fuck you");
            for (int i = 0; i < rakeCollider.leafs.Count; i++)
            {
                Vector3 dir = colPoint.transform.position - playerPoint.transform.position;
                dir = dir.normalized;
                dir.y = .3f;
                //rakeCollider.leafs[i].GetComponent<Rigidbody>().AddForce(pull * leafForce * Time.deltaTime, ForceMode.Impulse);
                //rakeCollider.leafs[i].gameObject.GetComponent<Rigidbody>().MovePosition((rakeCollider.leafs[i].gameObject.transform.position + pull )* leafForce);
                Vector3 pos = rakeCollider.leafs[i].transform.position;
                rakeCollider.leafs[i].gameObject.GetComponent<Rigidbody>().MovePosition(pos + (dir * pushMove));
                rakeCollider.leafs[i].gameObject.GetComponent<Rigidbody>().AddForce((dir * pushForce), ForceMode.Impulse);
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

                rake.transform.localPosition = Vector3.Lerp(Vector3.zero,(rake.transform.position - mouse_pos).normalized * .40f, chargeMeter / maxCharge);

                for (int i = 0; i < rakeCollider.leafs.Count; i++)
                {
                    Vector3 dir = playerPoint.transform.position - colPoint.transform.position;
                    dir = dir.normalized;
                    dir.y = .3f;
                    //rakeCollider.leafs[i].GetComponent<Rigidbody>().AddForce(dir * pullForce * Time.deltaTime,ForceMode.Impulse);
                    Vector3 pos = rakeCollider.leafs[i].transform.position;
                    rakeCollider.leafs[i].gameObject.GetComponent<Rigidbody>().MovePosition(pos + (dir * pullMove));
                    rakeCollider.leafs[i].gameObject.GetComponent<Rigidbody>().AddForce((dir * pullForce), ForceMode.Impulse);
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
                rake.transform.localPosition = Vector3.zero;
            }
        }

        mouseHeldLastFrame = mouseHeldThisFrame;
        mouseHeldThisFrame = false;
    }

    public void CheckForLeaves()
    {

    }
}
