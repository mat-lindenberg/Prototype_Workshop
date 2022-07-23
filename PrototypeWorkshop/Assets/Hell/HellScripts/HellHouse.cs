using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellHouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MessageWhenPickedUp()
    {
        //GameObject.Find("Manager").GetComponent<Hellmanager>().RemoveHousing(1);

    }

    public void MessageWhenPlacedDown()
    {
        //get people in range that are eligible to poke
        GameObject.Find("Manager").GetComponent<Hellmanager>().AddHousing(1);
    }
}
