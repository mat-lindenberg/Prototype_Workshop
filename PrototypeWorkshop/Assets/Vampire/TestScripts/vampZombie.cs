using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vampZombie : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;

    public float moveMult;
    public float maximumVelo;

    Rigidbody r;

    public SpriteRenderer sr;
    public Sprite[] runningSprites;
    public Sprite stillSprite;
    public float timeBetweenFrames;
    public float animationTimer;
    public int placeInMovingArray;

    public float hp;

    void Start()
    {
        playerTransform = GameObject.Find("Player").gameObject.transform;
        r = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputVector = playerTransform.position - transform.position;
        inputVector = inputVector.normalized;

        r.AddForce(inputVector * moveMult * Time.deltaTime);

        if (GetComponent<Rigidbody>().velocity.magnitude > maximumVelo)
        {
            r.velocity = Vector3.ClampMagnitude(r.velocity, maximumVelo);
        }


        if (inputVector.x > 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        if (animationTimer >= 0)
        {
            animationTimer -= Time.deltaTime;
        }
        else
        {
            placeInMovingArray++;
            if (placeInMovingArray > 3)
            {
                placeInMovingArray = 0;
            }
            sr.sprite = runningSprites[placeInMovingArray];
            animationTimer = timeBetweenFrames;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<VampireBullet>())
        {
            TakeDamage(collision.gameObject.GetComponent<VampireBullet>().damage);
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VampireController>())
        {
           
            other.GetComponent<VampireController>().TakeDamage(1);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<VampireController>())
        {
            
            other.GetComponent<VampireController>().TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject.Find("Managers").GetComponent<ExpSpawner>().OnDeathOfMonster(transform.position);
        Destroy(gameObject);
    }
}
