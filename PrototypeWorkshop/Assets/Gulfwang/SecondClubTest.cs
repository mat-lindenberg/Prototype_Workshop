using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondClubTest : MonoBehaviour
{
    public float displayAngle;

    public GameObject golfClub;
    public float oldAngle;
    public float velocity;
    public GameObject mousePosObject;

    public enum swingMode { Nothing, Angling, Releasing }
    public swingMode mySwingState;

    public float releaseTimer;
    public float timeToRelease;

    public GameObject golfBall;

    public float restingAngle = -85;
    public float trueAngle;
    public float releaseAngle;

    public float power;
    public float powerMult;
    public float maxPower = 90;
    // Start is called before the first frame update
    void Start()
    {
        mySwingState = swingMode.Nothing;
    }

    // Update is called once per frame
    void Update()
    {

        if (releaseTimer > 0)
        {

            if (releaseTimer < timeToRelease/2f && power > 0)
            {
                golfBall.GetComponent<Rigidbody2D>().AddForce(new Vector3(1, .2f, 0) * power, ForceMode2D.Impulse);
                power = 0;
            }

            float trueAngle = Remap(releaseTimer, 0, timeToRelease, -15, releaseAngle);

            golfClub.transform.rotation = Quaternion.Euler(0, 0, trueAngle);
            releaseTimer -= Time.deltaTime;
            return;
        }else if (releaseTimer <= 0 && mySwingState == swingMode.Releasing)
        {
            mySwingState = swingMode.Nothing;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //start swing
            mySwingState = swingMode.Angling;

        }

        if (Input.GetMouseButton(0))
        {
            //adjust to angle

            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector3 pointToRotateAround = transform.position;

            // Vector2 offset = new Vector2(mousePos.x - pointToRotateAround.x, mousePos.y - pointToRotateAround.y);
            //float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            power += Time.deltaTime * powerMult;
            if (power > maxPower)
            {
                power = maxPower;
            }

            releaseAngle = Remap(power, 0, maxPower, -90, -360);

            golfClub.transform.rotation = Quaternion.Euler(0, 0, releaseAngle);

            

            //golfClub.GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Lerp(golfClub.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), .9f));
        }

        if (Input.GetMouseButtonUp(0))
        {
            //release swing


            mySwingState = swingMode.Releasing;
            releaseTimer = timeToRelease;
        }

        if (Input.GetMouseButtonUp(1))
        {
            power = 0;
            //cancel swing
            mySwingState = swingMode.Nothing;
        }
    }

    public bool returnTrue()
    {
        return true;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
