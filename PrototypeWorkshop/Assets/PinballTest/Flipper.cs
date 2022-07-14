using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    Rigidbody r;

    public Quaternion initialRotation;
    public Quaternion targetRotation;

    public Vector3 initRot;

    public float rotationTimer;
    public float maxRot;

    public bool pressed;

    public bool isRight;
    private void Awake()
    {
        r = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
        initRot = initialRotation.eulerAngles;
    }

    private void FixedUpdate()
    {


        if (isRight)
        {
            if (Input.GetKey(KeyCode.D))
            {
                pressed = true;
            }
            else
            {
                pressed = false;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                pressed = true;
            }
            else
            {
                pressed = false;
            }
        }

        if (pressed == true)
        {
            if (rotationTimer < maxRot)
            {
                rotationTimer += Time.deltaTime;
            }

            if (isRight)
            {
                Quaternion targ = Quaternion.Euler(0, 45, 0);
                Quaternion intt = Quaternion.Euler(initRot);
                r.MoveRotation(Quaternion.Lerp(intt, targ, rotationTimer / maxRot));
            }
            else
            {
                Quaternion targ = Quaternion.Euler(0, -45, 0);
                Quaternion intt = Quaternion.Euler(initRot);
                r.MoveRotation(Quaternion.Lerp(intt, targ, rotationTimer / maxRot));
            }

        }
        else
        {
            rotationTimer = 0;
            r.MoveRotation(initialRotation);//Quaternion.Lerp(initialRotation, targetRotation, 0);
        }
    }
}
