using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour
{
    public int myState = 1;

    public Sprite messySprite;
    public Sprite clippedSprite;

    public int Hp;

    public SpriteRenderer sr;
    public Color hitColor;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClipMe()
    {
        if (Hp > 0)
        {
            Hp--;
            StartCoroutine("MyCoroutine");
        }

        if (Hp <= 0)
        {
            GameObject.Find("LandscapeGameRunner").GetComponent<LandscapeGameRunner>().AddToTimer(.5f);
            GetComponent<SpriteRenderer>().sprite = clippedSprite;
            myState = 0;
            
        }


    }

    IEnumerator MyCoroutine()
    {
        float i = 0;

        sr.color = hitColor;

        while (i < 25f)
        {
            // Count to Ten
            i++;
            sr.color = Color.Lerp(hitColor, Color.white, i / 25f);
            yield return null;
        }

        sr.color = Color.white;

    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }
}
