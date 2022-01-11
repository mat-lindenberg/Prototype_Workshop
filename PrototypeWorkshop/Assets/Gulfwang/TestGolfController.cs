using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGolfController : MonoBehaviour
{


    Rigidbody2D r;
    GameObject groundCheckPos;
    bool isGrounded;

    [Tooltip("X Axis force add.")]
    public float moveSpeed;

    [Tooltip("Value at which we stop adding X-axis force")]
    public float maxSpeed;

    [Tooltip("Speed with which a charecter with no inputs has X-axis velocity reduced")]
    public float damping;

    [Tooltip("Value for impule force of jump")]
    public float jumpForce;

    [Tooltip("a downard force which helps gravity react a little better")]
    public float gravityHelper;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        groundCheckPos = transform.Find("FeetPos").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float moveFloat;
        if (xAxis > 0)
        {
            moveFloat = moveSpeed;
            Vector2 force = new Vector2(1, 0);

            if (r.velocity.x < maxSpeed)
            {
                r.AddForce(force * moveFloat, ForceMode2D.Force);
            }
        }
        else if (xAxis < 0)
        {
            moveFloat = -moveSpeed;
            Vector2 force = new Vector2(1, 0);
            if (r.velocity.x > -maxSpeed)
            {
                r.AddForce(force * moveFloat, ForceMode2D.Force);
            }
        }
        else
        {
            moveFloat = 0;
            r.AddForce(new Vector2(r.velocity.x * damping,0), ForceMode2D.Impulse);
        }

        if (isGroundedCheck())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                r.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
            }
        }
        else
        {
            r.AddForce(new Vector2(0, gravityHelper));
        }
    }

    public bool isGroundedCheck()
    {
        if (Physics2D.OverlapCircle(groundCheckPos.transform.position, 0.15f, 1 << 8))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
