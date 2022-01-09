using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarScript : MonoBehaviour
{
    public float gasPedal;

    public float speedActual;

    public float maxSpeed;
    public float brakePedal;
    public float maxBrake;

    public float steeringWheel;
    public float maxTurningSpeed;


    public float vTwo;
    public float vThree;
    public float vleftFront;
    public float vrightFront;
    public float vNine;
    public float vEleven;

    public float brakeThreshold;
    public float offGasThreshold;
    public float effectOfSpeedOnBrakeThreshold;
    public float effectOfSpeedOnGasThreshold;


    public Vector3 velocity;
    public Vector3 lastPos;
    public Vector3 direction;

    public bool pushGas;
    public bool pushBrake;

    public bool steerLeft;
    public bool steerRight;

    public GameObject leftFrontPoint;
    public GameObject rightFrontPoint;

    public bool isAiControl;
    public ParticleSystem ps;
    public Rigidbody r;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayerControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pushGas = true;
        }
        else
        {
            pushGas = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            pushBrake = true;
        }
        else
        {
            pushBrake = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            steerLeft = true;
        }
        else
        {
            steerLeft = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            steerRight = true;
        }
        else
        {
            steerRight = false;
        }

        


    }

    public void AiControl()
    {


        if (IsFrontClear())
        {
            pushGas = true;
        }
        else
        {
            pushGas = false;

        }

        if (shouldBrake())
        {
            pushBrake = true;
        }
        else
        {
            pushBrake = false;
        }

        float f = returnTurnAngle();
        if (f < 0)
        {
            steerLeft = true;
            steerRight = false;
        }else if ( f > 0)
        {
            steerRight = true;
            steerLeft = false;
        }else if (f == 0)
        {
            steerLeft = false;
            steerRight = false;
        }

    }


    public float returnTurnAngle()
    {


        if (vTwo > vThree && vTwo > vleftFront && vTwo > vrightFront && vTwo > vNine && vTwo > vEleven)
        {
            //Debug.Log("V TWO");
            return 15f;
        }else if (vThree > vTwo && vThree > vleftFront && vThree > vrightFront && vThree > vNine && vThree > vEleven)
        {
            //Debug.Log("V Three");
            return 15f;
        }
        else if (vleftFront > vTwo && vleftFront > vThree && vleftFront > vrightFront && vleftFront > vNine && vleftFront > vEleven)
        {
            if (vleftFront > 15 && vrightFront > 15)
            {
                return 0;
            }
            //Debug.Log("Center Left");
            return -15f;
        }
        else if (vrightFront > vTwo && vrightFront > vleftFront && vrightFront > vThree && vrightFront > vNine && vrightFront > vEleven)
        {
            if (vleftFront > 15 && vrightFront > 15)
            {
                return 0;
            }
            //Debug.Log("Center Right");
            return 15f;
        }
        else if (vNine > vTwo && vNine > vleftFront && vNine > vrightFront && vNine > vThree && vNine > vEleven)
        {
            //Debug.Log("Nine");
            return -15f;
        }
        else if (vEleven > vTwo && vEleven > vleftFront && vEleven > vrightFront && vEleven > vThree && vEleven > vNine)
        {
            //Debug.Log("Eleven");
            return -15f;
        }





        return 0;
    }



    public bool shouldBrake()
    {
        if (vleftFront < brakeThreshold * (speedActual * effectOfSpeedOnBrakeThreshold) || vrightFront < brakeThreshold * (speedActual * effectOfSpeedOnBrakeThreshold))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsFrontClear()
    {
        if (vleftFront < offGasThreshold * (speedActual * effectOfSpeedOnGasThreshold) || vrightFront < offGasThreshold * (speedActual * effectOfSpeedOnGasThreshold))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool testForCollision()
    {
        if (vEleven <= 0 || vThree <= 0 || vleftFront <= 0 || vrightFront <= 0 || vNine <= 0 || vTwo <= 0)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {


        if (isDead == true)
        {
            return;
        }

        if (isAiControl == true)
        {
            AiControl();
        }
        else
        {
            PlayerControl();
        }
        

        if (pushGas == true)
        {
            gasPedal += Time.deltaTime*.225f;
            if (gasPedal > 1f)
            {
                gasPedal = 1f;
            }
        }
        else
        {

            if (gasPedal > 0)
            {
                gasPedal = 0;
            }


        }

        if (gasPedal > 0)
        {
            speedActual += gasPedal/15f;
            if (speedActual > maxSpeed)
            {
                speedActual = maxSpeed;
            }
        }
        else
        {
            speedActual *= .975f;
        }
        
        if (pushBrake == true)
        {
            brakePedal = 1;
        }
        else
        {
            brakePedal = 0;
        }

        if (brakePedal > 0)
        {
            speedActual *= .92f;
        }

        if (steerLeft == true)
        {
            steeringWheel -= Time.deltaTime*3;
            if (steeringWheel < -4f)
            {
                steeringWheel = -4f;
            }
        }
        else if (steerRight == true)
        {
            steeringWheel += Time.deltaTime*3;
            if (steeringWheel > 4f)
            {
                steeringWheel = 4f;
            }
        }
        else
        {
            steeringWheel *= .5f;
        }

        transform.Rotate(transform.up, steeringWheel);

        //r.MovePosition(r.position + transform.forward * speedActual);
        //r.AddForce(transform.forward * speedActual);
        transform.position += transform.forward * speedActual;
        r.velocity = Vector3.zero;

        RaycastArray();

        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        direction = transform.eulerAngles;



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
    }

    public void RaycastArray()
    {

        Vector3 rayOrigin = transform.position + new Vector3(0, .5f, 0);

        int layerMask = 1 << 9;
        RaycastHit hit;
        //FRONT
        if (Physics.Raycast(leftFrontPoint.transform.position, leftFrontPoint.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(leftFrontPoint.transform.position, leftFrontPoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            vleftFront = hit.distance;
        }
        else
        {
            Debug.DrawRay(leftFrontPoint.transform.position, leftFrontPoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            vleftFront = 1000;
        }

        //FRONT
        if (Physics.Raycast(rightFrontPoint.transform.position, rightFrontPoint.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rightFrontPoint.transform.position, rightFrontPoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            vrightFront = hit.distance;
        }
        else
        {
            Debug.DrawRay(leftFrontPoint.transform.position, leftFrontPoint.transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            vrightFront = 1000;
        }

        //9pm
        if (Physics.Raycast(rayOrigin, transform.forward + (transform.right * -.50f), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * -.50f)) * hit.distance, Color.red);
            vNine = hit.distance;
        }
        else
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * -.50f)) * 1000, Color.white);
            vNine = 1000;
        }



        //11pm
        if (Physics.Raycast(rayOrigin, transform.forward + (transform.right * -0.20f), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * -0.20f)) * hit.distance, Color.red);
            vEleven = hit.distance;
        }
        else
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * -0.20f)) * 1000, Color.white);
            vEleven = 1000;
        }


        //2pm
        if (Physics.Raycast(rayOrigin, transform.forward + (transform.right * 0.20f), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * 0.20f)) * hit.distance, Color.red);
            vTwo = hit.distance;
        }
        else
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * 0.20f)) * 1000, Color.white);
            vTwo = 1000;
        }

        //3pm
        if (Physics.Raycast(rayOrigin, transform.forward + (transform.right * .50f), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * .50f)) * hit.distance, Color.red);
            vThree = hit.distance;
        }
        else
        {
            Debug.DrawRay(rayOrigin, (transform.forward + (transform.right * .50f)) * 1000, Color.white);
            vThree = 1000;
        }

    }

    public void Explode()
    {
        ps.transform.position = transform.position;
        ps.Play();
        r.isKinematic = false;
        r.AddExplosionForce(750f, transform.position, 10f);
        isDead = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isDead == true)
        {
            return;
        }

        if (collision.collider.tag == "Collider")
        {
            Explode();
        }
    }
}
