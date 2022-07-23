using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellPoker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pokerGraphic;

    public AnimationCurve ac;
    public float animTimer = 0;
    public float timeForAnimationToTake = 1f;

    public float pokeTimer;
    public float timeBetweenPokes = 2f;
    public float pitckforkMaxer;
    bool poking;

    public List<HellTestPerson> peopleInRange;
    public int placeInPeopleArray;

    public Transform target;

    bool appliedDamage;

    public float speedMult = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (poking == false)
        {
            if (pokeTimer < timeBetweenPokes)
            {
                pokeTimer += Time.deltaTime * speedMult;
            }
            else
            {
                target = TargetLogic();
                if (target != null)
                {
                    
                    RotatePitchForkToTarget();
                    poking = true;
                    appliedDamage = false;
                }
                else
                {
                   // should we put some sort of timer on here, to keep a million searches from happening?
                }
            }
        }
        else
        {
            if (target != null)
            {
                Poking();
            }
                
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetPeopleInRadiusNEW();
        }

    }

    public void Poking()
    {
        if (animTimer < timeForAnimationToTake)
        {
            animTimer += Time.deltaTime * speedMult;

            Vector3 targetPos = transform.position;
            targetPos += new Vector3(4, 0, 0);

            targetPos.y = transform.position.y;

            Vector3 magn = transform.position - targetPos;
            magn = Vector3.Normalize(magn);

            magn *= pitckforkMaxer;

            if (animTimer > timeForAnimationToTake / 2f)
            {
                if (appliedDamage == false)
                {
                    target.GetComponent<HellTestPerson>().Slap();
                    appliedDamage = true;
                }
            }

            pokerGraphic.transform.localPosition = Vector3.LerpUnclamped(new Vector3(0,.5f,0), new Vector3(0, .5f, 0) + pokerGraphic.transform.right * pitckforkMaxer, ac.Evaluate(animTimer / timeForAnimationToTake));
        }
        else
        {

            pokerGraphic.transform.localPosition = new Vector3(0, .5f, 0);
            poking = false;
            pokeTimer = 0;
            animTimer = 0;
        }
    }


    public void RotatePitchForkToTarget()
    {
        Vector3 target_pos = target.position;

        //target_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = transform.position;
        target_pos.x = target_pos.x - object_pos.x;
        target_pos.z = target_pos.z - object_pos.z;
        float angle = Mathf.Atan2(target_pos.z, target_pos.x) * Mathf.Rad2Deg;
        pokerGraphic.transform.rotation = Quaternion.Lerp(pokerGraphic.transform.rotation, Quaternion.Euler(new Vector3(90, 0f, angle)), 1f);
    }

    public Transform TargetLogic()
    {
        if (peopleInRange.Count > 0)
        {
            if (placeInPeopleArray < peopleInRange.Count)
            {
                if (peopleInRange[placeInPeopleArray].attackable)
                {
                    peopleInRange[placeInPeopleArray].Targeted();
                    return peopleInRange[placeInPeopleArray].transform;
                }
                else
                {
                    placeInPeopleArray++;

                    if (placeInPeopleArray > peopleInRange.Count)
                    {
                        placeInPeopleArray = 0;
                    }

                    return null;
                }
            }
            else
            {
                placeInPeopleArray = 0;
            }
        }

        return null;

    }
    public void GetPeopleInRadius()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));

        peopleInRange.Clear();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<HellTestPerson>())
            {
                peopleInRange.Add(cols[i].GetComponent<HellTestPerson>());
            }
        }

    }

    public void rotateToMouse()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        pokerGraphic.transform.rotation = Quaternion.Lerp(pokerGraphic.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), 1f);
    }

    public void MessageWhenPickedUp()
    {
        GameObject.Find("Canvas").GetComponent<HellUIManager>().ChangTurnedBaseAnguish(-peopleInRange.Count);
        peopleInRange.Clear();
        target = null;
        poking = false;
        //shouldn't really be picked up?
    }

    public void MessageWhenPlacedDown()
    {
        //get people in range that are eligible to poke
        GetPeopleInRadiusNEW();
    }


    public void GetPeopleInRadiusNEW()
    {
        Debug.Log("Calling radius decector...");
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));

        peopleInRange.Clear();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].GetComponent<PeopleSlots>())
            {
                Debug.Log(cols[i].gameObject + " has slots");
                if (cols[i].GetComponent<PeopleSlots>().peopleInMe > 0)
                {
                    Debug.Log(cols[i].gameObject + " has people");
                    for (int j = 0; j < cols[i].GetComponent<PeopleSlots>().peopleInMe; j++)
                    {
                        Debug.Log("adding person");
                        peopleInRange.Add(cols[i].GetComponent<PeopleSlots>().persons[j].GetComponent<HellTestPerson>());
                    }
                }
            }
        }

        GameObject.Find("Canvas").GetComponent<HellUIManager>().ChangTurnedBaseAnguish(peopleInRange.Count);
    }
}
