using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTestPerson : MonoBehaviour
{
    // Start is called before the first frame update

    public float cooldownTimer;
    public float cooldownAmount = 2f;

    public bool attackable;



    void Start()
    {
        cooldownTimer = cooldownAmount;
    }

    // Update is called once per frame
    void Update()
    {


        if (attackable == false)
        {
            if (cooldownTimer < cooldownAmount)
            {
                cooldownTimer += Time.deltaTime;
                //Color c = Color.Lerp(Color.grey, Color.white, cooldownTimer / cooldownAmount+.1f);
                //transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
            }
            else
            {
                cooldownTimer = 0;
                attackable = true;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else
        {

        }

    }

    public void Targeted()
    {
        attackable = false;
    }

    public void Slap()
    {

            cooldownTimer = 0;
            GameObject.Find("Manager").GetComponent<Hellmanager>().AddAnguish(1);
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.grey;
            attackable = false;
    }

    public void MessageWhenPickedUp()
    {

    }

    public void MessageWhenPlacedDown()
    {

    }
}
