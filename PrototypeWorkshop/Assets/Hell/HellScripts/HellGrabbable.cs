using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellGrabbable : MonoBehaviour
{
    public bool inHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WhenPickUp()
    {
        SendMessage("MessageWhenPickedUp");
    }

    public void WhenPlaceDown()
    {
        
        if (gameObject.GetComponent<FenceBlock>())
        {
            SendMessage("MessageWhenPlacedDown");
        }
        
    }
}
