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
    }
}
