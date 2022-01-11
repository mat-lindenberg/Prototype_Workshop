using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGolfClubSCript : MonoBehaviour
{
    public GameObject golfClub;

    public float oldAngle;
    public float velocity;

    public Vector2 mouseClickPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePos.x + .4f, mousePos.y + .9f);
            golfClub.transform.rotation = Quaternion.Euler(0, 0, -95);
           // golfClub.GetComponent<Rigidbody2D>().rotation = -90;
        }
        else if (Input.GetMouseButton(0))
        {
            //rotate club towards mouse
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 dir = mousePos - this.transform.position;
            // Quaternion q = Quaternion.FromToRotation(this.transform.right, dir);

            // this.transform.right = dir;
            velocity = Mathf.Abs(oldAngle - golfClub.transform.eulerAngles.z);
            Debug.Log(oldAngle + " " + golfClub.transform.eulerAngles.z + " " + velocity);
            oldAngle = golfClub.transform.eulerAngles.z;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 clubPos = golfClub.transform.position;
            mousePos.x = mousePos.x - clubPos.x;
            mousePos.y = mousePos.y - clubPos.y;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
 
           // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)),.5f);
            golfClub.GetComponent<Rigidbody2D>().MoveRotation(Quaternion.Lerp(golfClub.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), .4f));







        }

    }

    public void recieveCollisionMessage(Collision2D collision)
    {
        Debug.Log("yall hite theb all");
        //get velocity
        //apply to ball
        //apply upwards of whatever force
        //try to really make it physics based in a proper way that doesn't allow for frameskipping bullshit

    }
}
