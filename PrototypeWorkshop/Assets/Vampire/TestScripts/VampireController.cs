using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class VampireController : MonoBehaviour
{
    public float movementFloat;

    public GameObject mousePos;

    public Rigidbody r;

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

    public int xDir;
    public int yDir;

    public float exp = 0;
    public int level;
    public Image expDisplay;

    public float maxHealth = 100;
    public float health;
    public Image healthDisplay;

    public ParticleSystem hurtSystem;

    public LevelupManager levelup;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        Vector3 inputVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.A))
        {
            inputVector += new Vector3(-movementFloat, 0, 0);
            xDir = -1;
            yDir = 0;
            handTransform.GetComponent<SpriteRenderer>().flipX = true;
            handTransform.rotation = Quaternion.Euler(45, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += new Vector3(movementFloat, 0, 0);
            xDir = 1;
            yDir = 0;
            handTransform.GetComponent<SpriteRenderer>().flipX = false;
            handTransform.rotation = Quaternion.Euler(45, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            inputVector += new Vector3(0, 0, movementFloat);
            yDir = 1;
            xDir = 0;
            handTransform.GetComponent<SpriteRenderer>().flipX = false;
            handTransform.rotation = Quaternion.Euler(45, 0, 90);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += new Vector3(0, 0, -movementFloat);
            yDir = -1;
            xDir = 0;
            handTransform.GetComponent<SpriteRenderer>().flipX = false;
            handTransform.rotation = Quaternion.Euler(45, 0, -90);
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            xDir = 1;
            yDir = -1;
            handTransform.GetComponent<SpriteRenderer>().flipX = false;
            handTransform.rotation = Quaternion.Euler(45, 0, -45);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            xDir = -1;
            yDir = -1;
            handTransform.GetComponent<SpriteRenderer>().flipX = true;
            handTransform.rotation = Quaternion.Euler(45, 0, 45);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            xDir = -1;
            yDir = 1;
            handTransform.GetComponent<SpriteRenderer>().flipX = true;
            handTransform.rotation = Quaternion.Euler(45, 0, -45);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            xDir = 1;
            yDir = 1;
            handTransform.GetComponent<SpriteRenderer>().flipX = false;
            handTransform.rotation = Quaternion.Euler(45, 0, 45);
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

            if (inputVector.x > 0)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
        GetComponent<Rigidbody>().AddForce(inputVector * speedMult);

        if (GetComponent<Rigidbody>().velocity.magnitude > maximumVelo)
        {
            r.velocity = Vector3.ClampMagnitude(r.velocity, maximumVelo);
        }
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vExp>())
        {
            exp += other.GetComponent<vExp>().exp;
            Destroy(other.gameObject);

            if (exp >= level * 10)
            {
                level++;
                levelup.StartLevelUp();
                exp = 0;
            }

            expDisplay.fillAmount = (float)exp / ((float)level * 10f);

        }
        else if (other.GetComponent<vMedkit>())
        {
            health += other.GetComponent<vMedkit>().healthValue;
            Destroy(other.gameObject);

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            healthDisplay.fillAmount = (float)health / ((float)maxHealth);

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthDisplay.fillAmount = health / maxHealth;
        hurtSystem.Play();
    }
    public void Die()
    {

    }
}
