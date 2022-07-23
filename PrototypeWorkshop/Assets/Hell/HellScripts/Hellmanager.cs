using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hellmanager : MonoBehaviour
{
    public float Anguish;
    public Text anguishUi;

    public float Housing;
    public Text housingUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAnguish(float f)
    {
        Anguish += f;
        anguishUi.text = Anguish + "";
    }

    public void RemoveAnguish(float f)
    {
        Anguish -= f;
        anguishUi.text = Anguish + "";
    }

    public void AddHousing(float f)
    {
        Housing += f;
        housingUi.text = Housing + "";
    }

    public void RemoveHousing(float f)
    {
        Housing -= f;
        housingUi.text = Housing + "";
    }
}
