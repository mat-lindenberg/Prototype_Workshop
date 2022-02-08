using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class seasonManager : MonoBehaviour
{
    public enum SEASON { CLEARING, TILLING, PLANTING, TENDING, HARVESTING, SELLING};
    public SEASON currentSeason;

    public float timer;
    public float maxTime;

    public TextMeshProUGUI seasonText;
    public Image clockface;

    public bool isIterating;

    public GameObject player;
    public Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        currentSeason = SEASON.CLEARING;
        updateSeasonText();
        timer = maxTime;
        player.GetComponent<SimpleSeasonController>().ForceEquipAxe();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isIterating = true;
        }

        if (isIterating == true)
        {
            timer -= Time.deltaTime;

            clockface.fillAmount = timer / maxTime;

            if (timer <= 0)
            {
                isIterating = false;
                iterateSeason();
                //move to next season;
            }
        }
    }

    public void updateSeasonText()
    {
        if (currentSeason == SEASON.CLEARING)
        {
            seasonText.text = "Clearing Season";
        }
        if (currentSeason == SEASON.TILLING)
        {
            seasonText.text = "Tilling Season";
        }
        if (currentSeason == SEASON.PLANTING)
        {
            seasonText.text = "Planting Season";
        }
        if (currentSeason == SEASON.HARVESTING)
        {
            seasonText.text = "Harvesting Season";
        }
        if (currentSeason == SEASON.TENDING)
        {
            seasonText.text = "Tending Season";
        }
        if (currentSeason == SEASON.SELLING)
        {
            seasonText.text = "Selling Season";
        }
    }

    public void iterateSeason()
    {
        timer = maxTime;
        isIterating = false;
        player.transform.position = startPos.transform.position;
        player.transform.position += new Vector3(0, .61f, 0);

        if (currentSeason == SEASON.CLEARING)
        {
            currentSeason = SEASON.TILLING;
            player.GetComponent<SimpleSeasonController>().ForceEquipHoe();
        }
        else if (currentSeason == SEASON.TILLING)
        {
            currentSeason = SEASON.PLANTING;
            player.GetComponent<SimpleSeasonController>().ForceEquipShovel();
        }
        else if(currentSeason == SEASON.PLANTING)
        {
            currentSeason = SEASON.TENDING;
            player.GetComponent<SimpleSeasonController>().ForceEquipBucket();
        }
        else if (currentSeason == SEASON.TENDING)
        {
            currentSeason = SEASON.HARVESTING;
        }
        else if(currentSeason == SEASON.HARVESTING)
        {
            currentSeason = SEASON.SELLING;
        }
        else if(currentSeason == SEASON.SELLING)
        {
            currentSeason = SEASON.CLEARING;
            player.GetComponent<SimpleSeasonController>().ForceEquipAxe();
        }

        updateSeasonText();
    }
}
