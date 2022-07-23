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

    public Hellmanager manager;

    //prefabs
    [Header("Temp Prefabs")]
    public GameObject personPrefab;
    public GameObject pokerPrefab;
    public GameObject lasherPrefab;
    public GameObject flagPrefab;
    public GameObject fencePrefab;
    public GameObject firepitPrefab;
    public GameObject judicatorPrefab;
    public GameObject drummerPrefab;
    public GameObject bankPrefab;
    public GameObject housePrefab;

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
                if (hitData.collider.GetComponent<HellGrabbable>().pickUpAble == true)
                {
                    carryingAnything = true;
                    objectInHand = hitData.collider.gameObject;
                    objectInHand.GetComponent<Collider>().enabled = false;
                    objectInHand.GetComponent<HellGrabbable>().WhenPickUp();
                }
            }
            if (hitData.collider.GetComponent<HellTestPortal>())
            {
                if (hitData.collider.GetComponent<HellTestPortal>().doWeHavePeopleInUs()){
                    GameObject g = hitData.collider.GetComponent<HellTestPortal>().returnFirstPersonGameObject();
                    carryingAnything = true;
                    objectInHand = g;
                    g.GetComponent<Collider>().enabled = false;
                    g.GetComponent<HellGrabbable>().WhenPickUp();
                }
            }
        }
        else if (carryingAnything == true)
        {
            if (objectMouseOver == null)
            {
                //placing an object down on empty ground in theory
                if (objectInHand.GetComponent<HellGrabbable>())
                {

                    if (objectInHand.GetComponent<HelTestCost>())
                    {
                        if (manager.Anguish >= objectInHand.GetComponent<HelTestCost>().anguishCost)
                        {
                            manager.RemoveAnguish(objectInHand.GetComponent<HelTestCost>().anguishCost);
                        }
                        else
                        {
                            return;
                        }
                    }


                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        GameObject g = Instantiate(objectInHand);
                        g.GetComponent<Collider>().enabled = true;
                        Vector3 v = mousePosVisualizer.transform.position;
                        v.y = 0f;
                        g.transform.position = v;
                        //carryingAnything = false;
                        g.GetComponent<HellGrabbable>().WhenPlaceDown();
                        //objectInHand = null;



                    }
                    else
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
            }
            else
            {
                if (objectInHand.GetComponent<HellTestPerson>())
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (objectMouseOver.GetComponent<PeopleSlots>().CanAddAPerson())
                        {
                            GameObject g = Instantiate(objectInHand);
                            objectMouseOver.GetComponent<PeopleSlots>().AddPerson(g);
                        }
                    }
                    else
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
    }
    public void ClickedRight()
    {
        if (carryingAnything == true)
        {
            if (objectInHand.GetComponent<HellTestPerson>())
            {
                GameObject.Find("Portal").GetComponent<HellTestPortal>().AddPersonToPortal(objectInHand);
                objectInHand.GetComponent<Collider>().enabled = true;
                objectInHand = null;
                carryingAnything = false;                   
            }
            else
            {

                Destroy(objectInHand);
                carryingAnything = false;
                objectInHand = null;
            }
        }

        Debug.Log("Left clicked Registereed");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        Debug.Log(hitData.collider.gameObject);

        if (hitData.collider.GetComponent<HellTestPerson>())
        {
            hitData.collider.GetComponent<HellTestPerson>().Slap();
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

    public void PlaceNewObjectInHand(GameObject _g)
    {
        GameObject g = Instantiate(_g);

        carryingAnything = true;
        objectInHand = g;
        g.GetComponent<Collider>().enabled = false;
        g.GetComponent<HellGrabbable>().WhenPickUp();

    }
    public void TryAddBuildingToMouse(string s)
    {
        if (carryingAnything == true)
        {
            return;
        }

        if (s == "Flag")
        {
            PlaceNewObjectInHand(flagPrefab);
        }
        else if (s == "Person")
        {
            PlaceNewObjectInHand(personPrefab);
        }
        else if (s == "Poker")
        {
            PlaceNewObjectInHand(pokerPrefab);
        }
        else if (s == "Fence")
        {
            PlaceNewObjectInHand(fencePrefab);
        }
        else if (s == "Firepit")
        {
            PlaceNewObjectInHand(firepitPrefab);
        }
        else if (s == "Lasher")
        {
            PlaceNewObjectInHand(lasherPrefab);
        }
        else if (s == "Drummer")
        {
            PlaceNewObjectInHand(drummerPrefab);
        }
        else if (s == "Judicator")
        {
            PlaceNewObjectInHand(judicatorPrefab);
        }
        else if (s == "Bank")
        {
            PlaceNewObjectInHand(bankPrefab);
        }
        else if (s == "House")
        {
            PlaceNewObjectInHand(housePrefab);
        }
    }
}
