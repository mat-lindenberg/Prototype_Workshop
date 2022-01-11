using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManScript : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;

    public float jumpForce;

    public LayerMask groundLayer;

    public float stoppingPower;

    public float maxX;

    public GameObject hands;
    public GameObject ball;

    public Vector2 ballFoce;
    public float ballMult;

    public float inheritVelocity;

    public int facing;
    public GameObject guySprite;

    bool holdingBall = false;

    public float chargeUpCounter;
    public float chargeSpeed;

    public float pickupTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButtonDown("Jump"))
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded() == true)
            {
                Jump();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (holdingBall == true)
            {
                //start charging up
                chargeUpCounter = 0;
            }
            else
            {
                checkIfCanGrabBasketball();
            }

        }


        if (Input.GetButton("Fire1"))
        {
            if (holdingBall == true)
            {
                ball.transform.localPosition = Vector2.Lerp(new Vector2(.25f * (float)facing, 0.0f), new Vector2(0, 0), chargeUpCounter / 1f);
                chargeUpCounter += chargeSpeed;
                if (chargeUpCounter > 1f)
                {

                    chargeUpCounter = 1;
                    
                }
            }
        }

        if (holdingBall == true)
        {
            if (pickupTimer > 0)
            {
                pickupTimer -= Time.deltaTime;
            }
            
        }

        if (Input.GetButtonUp("Fire1"))
        {

            if (holdingBall == true && pickupTimer <= 0)
            {
                throwBasketball();
            }
            
        }
    }

    public void checkIfCanGrabBasketball()
    {
        if (Vector2.Distance(transform.position, ball.transform.position) < 1f)
        {
            pickupBasketball();
        }
    }

    public void pickupBasketball()
    {
        ball.GetComponent<Collider2D>().enabled = false;
        ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.transform.SetParent(hands.transform);
        ball.transform.localPosition = Vector3.zero;
        holdingBall = true;
        pickupTimer = 1f;
    }

    public void throwBasketball()
    {
        ball.GetComponent<Collider2D>().enabled = true;
        ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ball.transform.SetParent(null);

        holdingBall = false;
        Vector2 manVelo = rb.velocity;
        ballFoce.x = facing;
        ballFoce.y = chargeUpCounter;


        ball.GetComponent<Rigidbody2D>().AddForce((ballFoce * (ballMult * chargeUpCounter)) + (manVelo * inheritVelocity));

        chargeUpCounter = 0;
    }

    public void Flip()
    {
        facing *= -1;
        hands.transform.localPosition *= -1f;
        guySprite.transform.localScale = new Vector3(guySprite.transform.localScale.x * -1f,1,1);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (facing == -1)
        {
            if (Input.GetAxis("Horizontal") > 0f){
                Flip();
            }
        }else if (facing == 1)
        {
            if (Input.GetAxis("Horizontal") < 0f)
            {
                Flip();
            }
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if(IsGrounded() == true)
        {
            rb.AddForce(movement * speed);
        }
        else
        {
            rb.AddForce(movement * speed);
        }

        if (rb.velocity.x > 0)
        {
            if (rb.velocity.x > maxX)
            {
                Vector2 velo = rb.velocity;
                rb.velocity = new Vector2(maxX, velo.y);
            }
        }
        else
        {
            if (rb.velocity.x < -maxX)
            {
                Vector2 velo = rb.velocity;
                rb.velocity = new Vector2(-maxX, velo.y);
            }
        }


        
        //damp movement if not in the air
        if (moveHorizontal == 0f && IsGrounded() == true)
        {
            Vector3 move = new Vector3(rb.velocity.x, 0.0f, 0.0f);
            rb.AddForce(move * -stoppingPower);
        }
    }
    
    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
