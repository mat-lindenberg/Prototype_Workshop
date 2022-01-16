using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlayerController : MonoBehaviour
{
    public float movementFloat;

    public GameObject mousePos;

    public Rigidbody r;

    public GameObject bulletPrefab;

    public float speedMult;
    public float damping;
    public float maximumVelo;

    public AnimationCurve movementAccelerationCurve;

    public SpriteRenderer sr;
    public Sprite[] runningSprites;
    public Sprite[] backSprites;
    public Sprite backStillSprite;
    public Sprite stillSprite;
    public float animationSpeed;
    public float animationTimer;
    public int placeInMovingArray;

    public Transform playerSpriteTransform;
    public Transform playerGunSpriteTransform;

    public Vector3 rotateAxis;

    public GameObject rakeObject;
    public GameObject blowerObject;
    public GameObject scytheObject;
    public GameObject shovelObject;
    public GameObject weedWhackerObject;
    public GameObject boomerangObject;
    public GameObject hedgeClipperObject;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rakeObject.SetActive(true);
            blowerObject.SetActive(false);
            scytheObject.SetActive(false);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = rakeObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(true);
            scytheObject.SetActive(false);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = blowerObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(false);
            scytheObject.SetActive(true);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = scytheObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(false);
            scytheObject.SetActive(false);
            shovelObject.SetActive(true);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = shovelObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(false);
            scytheObject.SetActive(false);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(true);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = weedWhackerObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(false);
            scytheObject.SetActive(false);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(true);
            hedgeClipperObject.SetActive(false);
            playerGunSpriteTransform = boomerangObject.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            rakeObject.SetActive(false);
            blowerObject.SetActive(false);
            scytheObject.SetActive(false);
            shovelObject.SetActive(false);
            weedWhackerObject.SetActive(false);
            boomerangObject.SetActive(false);
            hedgeClipperObject.SetActive(true);
            playerGunSpriteTransform = hedgeClipperObject.transform;
        }


        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        

        if (playerGunSpriteTransform.localScale.x == 1)
        {
           // float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
           // playerGunSpriteTransform.rotation = Quaternion.Lerp(playerGunSpriteTransform.rotation, Quaternion.Euler(new Vector3(90, 0,angle)), .9f);
        }
        else
        {
           // float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
           // playerGunSpriteTransform.rotation = Quaternion.Lerp(playerGunSpriteTransform.rotation, Quaternion.Euler(new Vector3(90,0, angle + 180)), .9f);
        }

        if (mouse_pos.x > transform.position.x)
        {
            //playerGunSpriteTransform.localScale = new Vector3(1, 1, 1);
           // playerGunSpriteTransform.GetComponent<SpriteRenderer>().flipY = false;
            playerSpriteTransform.GetComponent<SpriteRenderer>().flipX = false;

        }
        else
        {
             //playerGunSpriteTransform.localScale = new Vector3(-1, 1, 1);
            //playerGunSpriteTransform.GetComponent<SpriteRenderer>().flipY = true;
            playerSpriteTransform.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (mouse_pos.y > transform.position.y)
        {
            //playerGunSpriteTransform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            //playerGunSpriteTransform.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }



        Vector3 inputVector = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.A))
        {
            inputVector += new Vector3(-movementFloat, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += new Vector3(movementFloat, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += new Vector3(0, 0, movementFloat);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += new Vector3(0, 0, -movementFloat);
        }

        if (inputVector == Vector3.zero)
        {
            sr.sprite = stillSprite;
            GetComponent<Rigidbody>().velocity *= damping;
        }
        else
        {

            animationTimer += Time.deltaTime;

            if (animationTimer > animationSpeed)
            {
                animationTimer = 0;
                placeInMovingArray++;

                if (placeInMovingArray > runningSprites.Length-1)
                {
                    placeInMovingArray = 0;
                }

                if (mouse_pos.y > transform.position.y)
                {
                    sr.sprite = backSprites[placeInMovingArray];
                }
                else
                {
                    sr.sprite = runningSprites[placeInMovingArray];
                }
                    
            }

            if (inputVector.z > 0)
            {
               //playerGunSpriteTransform.GetComponent<SpriteRenderer>().sortingOrder = -1;
            }
            else
            {
                //playerGunSpriteTransform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }

        }

        GetComponent<Rigidbody>().AddForce(inputVector * speedMult);

        if (GetComponent<Rigidbody>().velocity.magnitude > maximumVelo)
        {
            r.velocity = Vector3.ClampMagnitude(r.velocity, maximumVelo);
        }

    }

    void FixedUpdate()
    {

    }





}
