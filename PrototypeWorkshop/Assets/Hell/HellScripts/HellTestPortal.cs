using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTestPortal : MonoBehaviour
{
    public float timer;
    public float speedMult;
    public float timeBetweenPeople;
    public int maxPeople;

    public List<HellTestPerson> peopleInMe;

    public GameObject personPrefab;
    public Transform transformWhereToPutPeople;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < timeBetweenPeople)
        {
            timer += Time.deltaTime * speedMult;
        }
        else {
            timer = 0;
             if (peopleInMe.Count < maxPeople)
            {
                AddPersonToPortal();
            }
        }
    }

    public bool doWeHavePeopleInUs()
    {
        if (peopleInMe.Count > 0)
        {
            return true;
        }
        return false;
    }

    public GameObject returnFirstPersonGameObject()
    {
        GameObject g = peopleInMe[0].gameObject;
        peopleInMe.RemoveAt(0);
        return g;
    }

    public void AddPersonToPortal()
    {
        GameObject g = Instantiate(personPrefab);
        g.transform.position = transformWhereToPutPeople.transform.position + new Vector3(Random.Range(-0.50f, 0.50f), 0, Random.Range(-0.50f, 0.50f));
        peopleInMe.Add(g.GetComponent<HellTestPerson>());
    }

    public void AddPersonToPortal(GameObject g)
    {
        g.transform.position = transformWhereToPutPeople.transform.position + new Vector3(Random.Range(-0.50f, 0.50f), 0, Random.Range(-0.50f, 0.50f));
        peopleInMe.Add(g.GetComponent<HellTestPerson>());
    }
}
