using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleShadow : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = Mathf.Lerp(.4f,0f, transform.parent.position.y / 6);
        GetComponent<SpriteRenderer>().color = tmp;
        transform.position = new Vector3(transform.position.x, .025f, transform.position.z);
    }
}
