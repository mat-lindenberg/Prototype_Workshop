using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledClub : MonoBehaviour
{
    public GameObject golfClub;

    public float oldAngle;
    public float velocity;

    public GameObject mousePosObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody2D r = golfClub.GetComponent<Rigidbody2D>();
        r.MovePosition(transform.position);
        r.velocity = Vector3.zero;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = -3;
        mousePosObject.transform.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
        }
        else if (Input.GetMouseButton(0))
        {
            var mouse = mousePos;
            var screenPoint = transform.position;
            
            var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle);

            Debug.Log(golfClub.GetComponent<Rigidbody2D>().angularVelocity);

            golfClub.GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Lerp(golfClub.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), .9f));
        }
        else
        {
            golfClub.GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Lerp(golfClub.transform.rotation, Quaternion.Euler(new Vector3(0, 0, -90)), .2f));
        }

    }
}
