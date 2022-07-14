using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellCameraMover : MonoBehaviour
{
    public float moveMult;

    public Transform mousePosVisualizer;

    public int gridSize;

    public GameObject objectInHand;
    public bool carryingAnything;

    public GameObject objectMouseOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastFromMouseEveryFrame();

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1f * moveMult, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1f * moveMult, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, 1f * moveMult);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -1f * moveMult);
        }

        MouseRaycast();

        if (Input.GetMouseButtonDown(1))
        {
            ClickedRight();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ClickedLeft();
        }

        if (carryingAnything == true)
        {
            Vector3 v = mousePosVisualizer.transform.position;
            v.y += 1f;
            objectInHand.transform.position = v;


            if (objectInHand.GetComponent<HellTestPerson>())
            {
                //see if under mouse is something we can put a person in
            }
        }

        

    }

    public void RaycastFromMouseEveryFrame()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);

        if (hitData.collider.GetComponent<HellGrabbable>()){
            objectMouseOver = hitData.collider.gameObject;
        }
        else
        {
            objectMouseOver = null;
        }
    }

    public void ClickedLeft()
    {
        //if hand is empty, pick up free range person
        if (carryingAnything == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);

            if (hitData.collider.GetComponent<HellGrabbable>())
            {
                carryingAnything = true;
                objectInHand = hitData.collider.gameObject;
                objectInHand.GetComponent<Collider>().enabled = false;
                objectInHand.GetComponent<HellGrabbable>().WhenPickUp();
            }
        }
        else if (carryingAnything == true)
        {
            if (objectMouseOver == null)
            {
                //placing an object down on empty ground in theory
                if (objectInHand.GetComponent<HellGrabbable>())
                {
                    objectInHand.GetComponent<Collider>().enabled = true;
                    Vector3 v = mousePosVisualizer.transform.position;
                    v.y = 0f;
                    objectInHand.transform.position = v;
                    carryingAnything = false;
                    objectInHand.GetComponent<HellGrabbable>().WhenPlaceDown();
                    objectInHand = null;
                }
            }
            else
            {
                if (objectInHand.GetComponent<HellTestPerson>())
                {
                    if (objectMouseOver.GetComponent<PeopleSlots>().CanAddAPerson())
                    {
                        objectMouseOver.GetComponent<PeopleSlots>().AddPerson(objectInHand);
                        carryingAnything = false;
                        objectInHand = null;
                    }
                }
            }
        }
    }
    public void ClickedRight()
    {
        Debug.Log("Left clicked Registereed");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        Debug.Log(hitData.collider.gameObject);

        if (hitData.collider.GetComponent<HellTestPerson>())
        {
            hitData.collider.GetComponent<HellTestPerson>().TrySlap();
        }
    }

    public void MouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 target = hit.point;

            target.x = Mathf.RoundToInt(target.x);
            target.z = Mathf.RoundToInt(target.z);

            //target.x -= .5f;
            //target.z -= .5f;

            mousePosVisualizer.transform.position = target;


        }
    }
}
