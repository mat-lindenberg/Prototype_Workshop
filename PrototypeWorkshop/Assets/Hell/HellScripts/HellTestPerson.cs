using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTestPerson : MonoBehaviour
{
    // Start is called before the first frame update

    public float cooldownTimer;
    public float cooldownAmount = 2f;

    public Color damageColor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer < cooldownAmount)
        {
            cooldownTimer += Time.deltaTime;
            Color c = Color.Lerp(damageColor, Color.white, cooldownTimer / cooldownAmount);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
        }
    }



    public void TrySlap()
    {
        if (cooldownTimer >= cooldownAmount)
        {
            cooldownTimer = 0;
            GameObject.Find("Manager").GetComponent<Hellmanager>().AddAnguish(1);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = damageColor;
        }


    }

    public void MessageWhenPickedUp()
    {

    }

    public void MessageWhenPlacedDown()
    {

    }
}
