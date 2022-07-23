using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrummerDemon : MonoBehaviour
{

    public float timer;


    public SpriteRenderer sr;
    public Sprite drummer_idle_Sprite;
    public Sprite drummer_hit_sprite;

    bool idle;

    public List<HellPoker> pokerInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer < .5f)
        {
            timer += Time.deltaTime;

            if (idle == true)
            {
                if (timer > .4f)
                {
                    idle = false;
                    sr.sprite = drummer_hit_sprite;
                }
            }
        }
        else
        {
            idle = true;
            sr.sprite = drummer_idle_Sprite;
            timer = 0;
        }

    }

    public void AddSpeedToDemonsInRadius()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));

        pokerInRange.Clear();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<HellPoker>())
            {
                cols[i].GetComponent<HellPoker>().speedMult += .5f;
            }

            if (cols[i].GetComponent<LasherDemon>())
            {
                cols[i].GetComponent<LasherDemon>().speedMult += .5f;
            }
        }
    }

    public void SubtractSpeedFromDemonsInRadius()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));

        pokerInRange.Clear();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<HellPoker>())
            {
                cols[i].GetComponent<HellPoker>().speedMult -= .5f;
            }

            if (cols[i].GetComponent<LasherDemon>())
            {
                cols[i].GetComponent<LasherDemon>().speedMult -= .5f;
            }
        }
    }

    public void MessageWhenPickedUp()
    {
        Debug.Log("PICKED up Drumma");
    }

    public void MessageWhenPlacedDown()
    {
        AddSpeedToDemonsInRadius();
        Debug.Log("Placed Down Drummer");
        //get people in range that are eligible to poke
  
    }
}
