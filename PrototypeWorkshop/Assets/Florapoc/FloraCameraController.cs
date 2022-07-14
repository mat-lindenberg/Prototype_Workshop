using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraCameraController : MonoBehaviour
{
    public GameObject target;

    public float distance;
    public float height;

    public float followStrength;

    public Vector3 mousePos;
    public GameObject mousePosLook;
    public float lookstrengthAffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            mousePosLook.transform.position = hit.point;
        }

        Vector3 mid = Vector3.Lerp(target.transform.position, mousePosLook.transform.position, lookstrengthAffect);
        //mousePosLook.transform.position = mid;

        Vector3 lerp = Vector3.Lerp(transform.position, mid + new Vector3(0, height, distance), followStrength * Time.deltaTime);
        //Vector3 lerp = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0, height, distance), followStrength);
        transform.position = lerp;
    }
}
