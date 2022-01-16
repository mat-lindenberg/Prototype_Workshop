using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandscapeGameRunner : MonoBehaviour
{
    public float startingTime = 10f;
    public float currentScore = 0f;

    public float maxScoreToDisplay = 35f; 
    public float scoreForGold = 30f;
    public float scoreForSilver = 20f;
    public float scoreForBronze = 10f;

    public bool timerRunning;

    public float levelPoints;
    public float levelMaxPoints;
    public float levelThreshold = 75;

    public Text timerText;
    public GameObject lossText;

    public Transform playerTransform;
    public GameObject startObject;

    public bool playerIsInStart;

    public Image barContainer;
    public Image goldNotch;
    public Image silverNotch;
    public Image bronzeNotch;
    public Image progressBar;
    public float f;

    public Image completeContainer;
    public Image completeNotch;
    public Image completeProgress;

    public GameObject grassManager;
    public GameObject weedManager;

    public GenFromTile gen;
    public GenFromTile gen2;
    // Start is called before the first frame update
    void Start()
    {
        timerRunning = false;
        playerTransform.transform.position = startObject.transform.position;

        Vector3[] v = new Vector3[4];
        barContainer.rectTransform.GetWorldCorners(v);

        Debug.Log("World Corners");
        for (var i = 0; i < 4; i++)
        {
            Debug.Log("World Corner " + i + " : " + v[i]);
        }

        Vector3 yPos = Vector3.Lerp(v[0], v[2], .5f);

        Vector3 goldPos = Vector3.Lerp(v[0], v[2], scoreForGold / maxScoreToDisplay);
        goldPos.y = yPos.y;
        goldNotch.rectTransform.position = goldPos;

        Vector3 silverPos = Vector3.Lerp(v[0], v[2], scoreForSilver / maxScoreToDisplay);
        silverPos.y = yPos.y;
        silverNotch.rectTransform.position = silverPos;

        Vector3 bronzePos = Vector3.Lerp(v[0], v[2], scoreForBronze / maxScoreToDisplay);
        bronzePos.y = yPos.y;
        bronzeNotch.rectTransform.position = bronzePos;

        //goldNotch.rectTransform.position = v[0];
        //silverNotch.rectTransform.position = v[1];
        //bronzeNotch.rectTransform.position = v[2];
        currentScore = startingTime;

        gen.MakeGrass();
        gen2.MakeGrass();
        // public void calculate total
        for (int i = 0; i < grassManager.transform.childCount; i++)
        {
            if (grassManager.transform.GetChild(i).GetComponent<GrassScript>())
            {
                levelMaxPoints += .1f;
            }

        }

        for (int i = 0; i < weedManager.transform.childCount; i++)
        {
            if (weedManager.transform.GetChild(i).GetComponent<WeedScript>())
            {
                levelMaxPoints += .5f;
            }
        }

        completeProgress.fillAmount = 0;

        v = new Vector3[4];
        completeContainer.rectTransform.GetWorldCorners(v);
        Vector3 completePoz = Vector3.Lerp(v[0], v[3], levelThreshold / levelMaxPoints);
        //completePoz.y = yPos.y;
        completeNotch.rectTransform.position = completePoz;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            timerRunning = false;
            lossText.SetActive(false);
        }

        if (timerRunning == true)
        {

            float c = levelPoints / levelMaxPoints;
            completeProgress.fillAmount = c;




            f = currentScore / maxScoreToDisplay;
            progressBar.fillAmount = f;

            currentScore -= Time.deltaTime;


            timerText.text = currentScore.ToString("0");
            if (currentScore < 0)
            {
                Loss();
                timerRunning = false;
            }
        }

        if (playerIsInStart == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //end
                lossText.SetActive(true);
                lossText.GetComponent<Text>().text = "VICTORY!";
                timerRunning = false;
            }
        }
    }

    public void AddToTimer(float f)
    {
        levelPoints += f;
        currentScore += f;
    }

    public void Loss()
    {
        lossText.gameObject.SetActive(true);
    }

    public void PlayerEnteredStartArea()
    {
        playerIsInStart = true;
    }

    public void PlayerLeftEndZone()
    {
        if (timerRunning == false)
        {
            timerRunning = true;
        }
        playerIsInStart = false;
    }
}
