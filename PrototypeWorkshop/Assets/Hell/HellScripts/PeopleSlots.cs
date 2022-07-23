using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSlots : MonoBehaviour
{
    // Start is called before the first frame update
    public int peopleSlots;
    public int peopleInMe;

    public Transform[] slotPositions;

    public GameObject[] persons;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanAddAPerson()
    {
        if (peopleInMe < peopleSlots)
        {
            return true;
        }
        return false;
    }

    public void AddPerson(GameObject g)
    {

        persons[peopleInMe] = g;
        g.transform.SetParent(transform);
        g.transform.position = slotPositions[peopleInMe].transform.position;
        peopleInMe++;
        AddToDemonsNearMe(g);
        //check demons near me and add if in range
    }

    public void MessageWhenPickedUp()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<HellPoker>())
            {
                //cols[i].GetComponent<HellPoker>().peopleInRange.Add(g.GetComponent<HellTestPerson>());
                GameObject.Find("Canvas").GetComponent<HellUIManager>().ChangTurnedBaseAnguish(-peopleInMe);
            }
        }
    }


    public void AddToDemonsNearMe(GameObject g)
    {

        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));



        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<HellPoker>())
            {
                cols[i].GetComponent<HellPoker>().peopleInRange.Add(g.GetComponent<HellTestPerson>());
                GameObject.Find("Canvas").GetComponent<HellUIManager>().ChangTurnedBaseAnguish(1);
            }
        }
    }
}
