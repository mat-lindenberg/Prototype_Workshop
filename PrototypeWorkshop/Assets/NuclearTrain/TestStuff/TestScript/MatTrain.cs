using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatTrain : MonoBehaviour
{
    public float speed;

    public RailroadNode nextNode;
    public RailroadNode currentNode;
    public RailroadNode previousNode;
    public Vector3 direction;
    public Vector3 prevPos;
    public Vector3 currentPos;

    public Vector3 keypressDirection;

    public GameObject mousePoser;

    public GameObject bulletPrefab;
    public Transform shootpoint;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 8;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5;
        }

        placeCursor();

        keypressDirection.x = 0;
        keypressDirection.y = 0;
        keypressDirection.z = 0;

        if (Input.GetKey(KeyCode.W))
        {
            keypressDirection.z = -1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            keypressDirection.z = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            keypressDirection.x = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            keypressDirection.x = -1;
        }

        if (speed > 0)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            Vector3 pos = Vector3.MoveTowards(transform.position, currentNode.transform.position, step);
            pos.y = .25f;
            transform.position = pos;

            currentPos = transform.position;

            direction = currentPos - prevPos;
            transform.rotation = Quaternion.LookRotation(direction);

            if (Vector3.Distance(transform.position,currentNode.gameObject.transform.position) < .1f)
            {
                //within threshold
                nextNode = returnProperConnection();
                previousNode = currentNode;
                currentNode = nextNode;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


        prevPos = currentPos;
    }

    public void Shoot()
    {
        GameObject g = Instantiate(bulletPrefab);
        g.transform.position = shootpoint.transform.position;

        g.GetComponent<ntBullet>().OnCreation(mousePoser.transform.position);
    }
    public void PlaceOnNode(RailroadNode r)
    {
        transform.position = r.gameObject.transform.position;
        currentNode = r;
        previousNode = currentNode.connections[0];
    }

    public void placeCursor()
    {
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(clickRay, out hit, 100))
        {
            //Debug.Log(hit.transform.name);
            //Debug.Log("hit");
            mousePoser.transform.position = hit.point;
        }
    }
   
    public RailroadNode returnProperConnection()
    {


        if (currentNode.connections.Count == 1)
        {
            return currentNode.connections[0];
        }else if (currentNode.connections.Count == 2)
        {
            if (previousNode == currentNode.connections[0])
            {
                return currentNode.connections[1];
            }
            else if (previousNode == currentNode.connections[1])
            {
                return currentNode.connections[0];
            }
            else
            {
                Debug.Log("catastophic logic failure // this should never happen");
            }
        }
        else if (currentNode.connections.Count == 3)
        {
            if (keypressDirection.x != 0 || keypressDirection.z != 0)
            {
                RailroadNode r = currentNode.returnNodeAtDirection(keypressDirection);

                if (r == null)
                {
                    //dang
                }
                else if (r == previousNode)
                {
                    //dang
                }
                else
                {
                    Debug.Log("should be changing directionally");
                    return r;
                }
            }

            if (previousNode == currentNode.connections[0])
            {
                return currentNode.connections[1];
            }
            else if (previousNode == currentNode.connections[1])
            {
                return currentNode.connections[0];
            }
            else if (previousNode == currentNode.connections[2])
            {
                return currentNode.connections[0];
            }
        }




        Debug.Log("catastophic logic failure // this should never happen");
        return null;
    }
}
