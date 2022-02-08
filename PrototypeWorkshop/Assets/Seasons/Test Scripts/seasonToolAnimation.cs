using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seasonToolAnimation : MonoBehaviour
{
    public float animationTimer;
    public float timeAnimationTakes;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;

            if (animationTimer > timeAnimationTakes/2f)
            {
                transform.Rotate(0, 0, -4);
            }
            else
            {
                transform.Rotate(0, 0, 4);
            }

            if (animationTimer <= 0)
            {
                transform.rotation = Quaternion.Euler(45, 0, 0);
            }
        }
    }
}
