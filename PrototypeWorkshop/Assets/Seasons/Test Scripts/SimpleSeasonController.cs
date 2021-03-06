using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleSeasonController : MonoBehaviour
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
    public Transform handTransform;

    public Vector3 rotateAxis;

    public seasonToolAnimation tool;

    public Sprite shovelSprite;
    public Sprite axeSprite;
    public Sprite waterbucketSprite;
    public Sprite hoeSprite;

    public enum TOOL_STATES { Shovel, Axe, Hoe, Bucket};
    public TOOL_STATES selected_tool;

    public Tilemap tilemap;

    public Tile wetDirt;
    public Tile tilledDirt;
    public Tile dirt;
    public Tile plantableDirt;

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
            tool.GetComponent<SpriteRenderer>().sprite = shovelSprite;
            selected_tool = TOOL_STATES.Shovel;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tool.GetComponent<SpriteRenderer>().sprite = axeSprite;
            selected_tool = TOOL_STATES.Axe;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tool.GetComponent<SpriteRenderer>().sprite = waterbucketSprite;
            selected_tool = TOOL_STATES.Bucket;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            tool.GetComponent<SpriteRenderer>().sprite = hoeSprite;
            selected_tool = TOOL_STATES.Hoe;
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            if (tool.animationTimer <= 0)
            {
                tool.animationTimer = tool.timeAnimationTakes;

                if (selected_tool == TOOL_STATES.Axe)
                {
                    DamageInArea();
                }
                else if (selected_tool == TOOL_STATES.Shovel)
                {
                    Vector3 worldPoint = mousePos.transform.position;
                    var tpos = tilemap.WorldToCell(worldPoint);
                    Debug.Log(tpos);
                    tilemap.SetTile(tpos, plantableDirt);
                    //place planting hole on tile
                }
                else if (selected_tool == TOOL_STATES.Hoe)
                {
                    Vector3 worldPoint = mousePos.transform.position;
                    var tpos = tilemap.WorldToCell(worldPoint);
                    Debug.Log(tpos);
                    tilemap.SetTile(tpos, tilledDirt);
                    //turn ground to dirt
                }
                else if (selected_tool == TOOL_STATES.Bucket)
                {
                    //turn tile wet
                    Vector3 worldPoint = mousePos.transform.position;
                    var tpos = tilemap.WorldToCell(worldPoint);
                    Debug.Log(tpos);
                    tilemap.SetTile(tpos, wetDirt);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //item in hands throw
            
            if (handTransform.childCount == 0)
            {
                Transform t = returnClosestGrabbableObject();
                if (t != null)
                {
                    t.transform.parent = handTransform;
                    t.transform.position = handTransform.position;
                    t.GetComponent<Rigidbody>().isKinematic = true;
                    t.GetComponent<Collider>().enabled = false;
                }
                
            }
            else
            {
                ThrowObject();
            }
        }

        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        Vector3 inputVector = new Vector3(0, 0, 0);

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

                if (placeInMovingArray > runningSprites.Length - 1)
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

    public void DigTile()
    {

    }

    public void ThrowObject()
    {
        Transform t = handTransform.GetChild(0);

        t.SetParent(null);

        t.GetComponent<Rigidbody>().isKinematic = false;
        t.GetComponent<Collider>().enabled = true;

        Vector3 dir = mousePos.transform.position - transform.position;
        dir = dir.normalized;

        t.GetComponent<Rigidbody>().AddForce(dir * 13, ForceMode.Impulse);

    }

    public void ForceEquipShovel()
    {
        tool.GetComponent<SpriteRenderer>().sprite = shovelSprite;
        selected_tool = TOOL_STATES.Shovel;
    }

    public void ForceEquipHoe()
    {
        tool.GetComponent<SpriteRenderer>().sprite = hoeSprite;
        selected_tool = TOOL_STATES.Hoe;
    }

    public void ForceEquipBucket()
    {
        tool.GetComponent<SpriteRenderer>().sprite = waterbucketSprite;
        selected_tool = TOOL_STATES.Bucket;
    }

    public void ForceEquipAxe()
    {
        tool.GetComponent<SpriteRenderer>().sprite = axeSprite;
        selected_tool = TOOL_STATES.Axe;
    }
    public void DamageInArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, .75f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.GetComponent<seasonStump>())
            {
                hitColliders[i].GetComponent<seasonStump>().TakeDamage(1f);
            }

        }
    }
    public Transform returnClosestGrabbableObject()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

        float dist = 100;
        Transform toReturn = null;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.GetComponent<seasonPickupable>())
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].gameObject.transform.position);

                if (distance < dist)
                {
                    dist = distance;
                    toReturn = hitColliders[i].transform;
                }
            }

        }

        return toReturn;
    }
}
