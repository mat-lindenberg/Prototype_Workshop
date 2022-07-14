using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBlock : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject aboveGraphic;
    public GameObject belowGraphic;
    public GameObject rightGraphic;
    public GameObject leftGraphic;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MessageWhenPickedUp()
    {
        aboveGraphic.gameObject.SetActive(true);
        belowGraphic.gameObject.SetActive(true);
        rightGraphic.gameObject.SetActive(true);
        leftGraphic.gameObject.SetActive(true);

        //update all around
        Vector3 tileAbove = transform.position + new Vector3(0, 0, 1);
        Vector3 tileRight = transform.position + new Vector3(1, 0, 0);
        Vector3 tileLeft = transform.position + new Vector3(-1, 0, 0);
        Vector3 tileBelow = transform.position + new Vector3(0, 0, -1);

        if (doesTileContainFence(tileAbove))
        {
            returnFenceAtPos(tileAbove).GetComponent<FenceBlock>().belowGraphic.gameObject.SetActive(true);
        }

        if (doesTileContainFence(tileRight))
        {
            returnFenceAtPos(tileRight).GetComponent<FenceBlock>().leftGraphic.gameObject.SetActive(true);
        }

        if (doesTileContainFence(tileLeft))
        {
            returnFenceAtPos(tileLeft).GetComponent<FenceBlock>().rightGraphic.gameObject.SetActive(true);
        }

        if (doesTileContainFence(tileBelow))
        {
            returnFenceAtPos(tileBelow).GetComponent<FenceBlock>().aboveGraphic.gameObject.SetActive(true);
        }

    }
    public void MessageWhenPlacedDown()
    {

        //update all around
        Vector3 tileAbove = transform.position + new Vector3(0, 0, 1);
        Vector3 tileRight = transform.position + new Vector3(1, 0, 0);
        Vector3 tileLeft = transform.position + new Vector3(-1, 0, 0);
        Vector3 tileBelow = transform.position + new Vector3(0, 0, -1);

        if (doesTileContainFence(tileAbove))
        {
            aboveGraphic.gameObject.SetActive(false);
            returnFenceAtPos(tileAbove).GetComponent<FenceBlock>().belowGraphic.gameObject.SetActive(false);
        }

        if (doesTileContainFence(tileRight))
        {
            rightGraphic.gameObject.SetActive(false);
            returnFenceAtPos(tileRight).GetComponent<FenceBlock>().leftGraphic.gameObject.SetActive(false);
        }

        if (doesTileContainFence(tileLeft))
        {
            leftGraphic.gameObject.SetActive(false);
            returnFenceAtPos(tileLeft).GetComponent<FenceBlock>().rightGraphic.gameObject.SetActive(false);
        }

        if (doesTileContainFence(tileBelow))
        {
            belowGraphic.gameObject.SetActive(false);
            returnFenceAtPos(tileBelow).GetComponent<FenceBlock>().aboveGraphic.gameObject.SetActive(false);
        }


    }

    public bool doesTileContainFence(Vector3 position)
    {


        Collider[] cols = Physics.OverlapBox(position,new Vector3(.5f,.5f,.5f));

        Debug.Log(position);

        for (int i = 0; i < cols.Length; i++)
        {
           

            if (cols[i].gameObject == gameObject)
            {

            }
            else
            {
                if (cols[i].GetComponent<FenceBlock>())
                {
                    
                    Debug.Log(cols[i].gameObject.name + "is returned by overlap");
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject returnFenceAtPos(Vector3 position)
    {
        Collider[] cols = Physics.OverlapBox(position, new Vector3(.5f, .5f, .5f));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject == gameObject)
            {

            }
            else
            {
                if (cols[i].GetComponent<FenceBlock>())
                {
                    return cols[i].gameObject;
                }
            }
        }
        return null;
    }
}
