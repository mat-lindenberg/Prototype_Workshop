using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgeClipper : MonoBehaviour
{

    public GameObject hcSprite;
    public float spriteRotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        if (hcSprite.transform.localScale.x == 1)
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            hcSprite.transform.rotation = Quaternion.Lerp(hcSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle)), spriteRotSpeed);
        }
        else
        {
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            //playerGunSpriteTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            hcSprite.transform.rotation = Quaternion.Lerp(hcSprite.transform.rotation, Quaternion.Euler(new Vector3(90, 0, angle + 180)), spriteRotSpeed);
        }


        if (mouse_pos.x > transform.position.x)
        {
            //gunSprite.transform.localScale = new Vector3(1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            //gunSprite.transform.localScale = new Vector3(-1, 1, 1);
            //gunSprite.GetComponent<SpriteRenderer>().flipY = true;
        }
    }
}
