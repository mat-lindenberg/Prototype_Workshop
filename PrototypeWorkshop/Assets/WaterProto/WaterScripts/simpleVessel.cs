using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleVessel : MonoBehaviour
{

    public int waterInMe;
    public int maxWater;

    public Vector3 waterInitialScale;
    public Vector3 waterFullScale;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Water").localScale = Vector3.Lerp(waterInitialScale, waterFullScale, (float)waterInMe / (float)maxWater);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recieveWater(int amount)
    {
        waterInMe += amount;

        transform.Find("Water").localScale = Vector3.Lerp(waterInitialScale, waterFullScale, (float)waterInMe / (float)maxWater);

        if (waterInMe > maxWater)
        {
            waterInMe = maxWater;

        }
    }
}
