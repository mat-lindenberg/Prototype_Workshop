using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTalkScript : MonoBehaviour
{
    public Sprite booImage;
    public Sprite restImage;
    public Sprite blinkImage;
    public Sprite scaredImage;

    public AudioClip booClip;
    public AudioClip ahhClip;

    public float timer;

    public enum ghostStates { Booing,Resting,Blinking,Ahhing, PreScared}
    public ghostStates myState;

    public GhostTalkScript otherGhost;

    public bool isLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isLeft == true)
            {
                TriggerBoo();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isLeft == false)
            {
                TriggerBoo();
            }
        }


        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (myState == ghostStates.Booing)
            {
                myState = ghostStates.Resting;
                timer = 2f;
                GetComponent<SpriteRenderer>().sprite = restImage;
            }
            else if (myState == ghostStates.Ahhing)
            {
                myState = ghostStates.Resting;
                timer = 2f;
                GetComponent<SpriteRenderer>().sprite = restImage;
            }
            else if (myState == ghostStates.Blinking)
            {
                myState = ghostStates.Resting;
                timer = 2f;
                GetComponent<SpriteRenderer>().sprite = restImage;
            }
            else if (myState == ghostStates.PreScared)
            {
                myState = ghostStates.Ahhing;
                timer = .75f;
                GetComponent<SpriteRenderer>().sprite = scaredImage;
                GetComponent<AudioSource>().clip = ahhClip;
                GetComponent<AudioSource>().Play();
            }
            else if (myState == ghostStates.Resting)
            {
                float chance = Random.Range(0, 100);
                if (chance < 80)
                {
                    myState = ghostStates.Resting;
                    timer = 1f;
                    GetComponent<SpriteRenderer>().sprite = restImage;
                }
                else if (chance < 100)
                {
                    myState = ghostStates.Blinking;
                    timer = .25f;
                    GetComponent<SpriteRenderer>().sprite = blinkImage;
                }
                else
                {
                    TriggerBoo();
                }

            }


        }


    }

    public void TriggerBoo()
    {
        myState = ghostStates.Booing;
        timer = .5f;
        GetComponent<SpriteRenderer>().sprite = booImage;
        otherGhost.TriggerAhh();
        GetComponent<AudioSource>().clip = booClip;
        GetComponent<AudioSource>().Play();
    }

    public void TriggerAhh()
    {
        myState = ghostStates.PreScared;
        timer = .75f;

        GetComponent<SpriteRenderer>().sprite = restImage;
    }
}
