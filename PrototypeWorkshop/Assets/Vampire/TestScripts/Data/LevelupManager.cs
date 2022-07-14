using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelupManager : MonoBehaviour
{
    public Canvas upgradeCanvas;

    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    public vRelic relic1;
    public vRelic relic2;
    public vRelic relic3;

    public RelicBank relicBank;

    public vPlayerInventory player;
    // Start is called before the first frame update
    void Start()
    {
        // upgradeCanvas.enabled = false;
        StartLevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicked1()
    {
        if (doesPlayerHaveRelic(relic1))
        {
            relic1.level++;
        }
        else
        {
            vRelic v = new vRelic(relic1.RelicName,relic1.level,relic1.trait,relic1.modification);
            v.level++;
            player.playerRelics.Add(v);
        }
        closeWindow();
    }

    public void clicked2()
    {
        if (doesPlayerHaveRelic(relic2))
        {
            relic2.level++;
        }
        else
        {
            vRelic v = new vRelic(relic2.RelicName, relic2.level, relic2.trait, relic2.modification);
            v.level++;
            player.playerRelics.Add(v);
        }
        closeWindow();
    }

    public void clicked3()
    {
        if (doesPlayerHaveRelic(relic3))
        {
            relic3.level++;
        }
        else
        {
            vRelic v = new vRelic(relic3.RelicName, relic3.level, relic3.trait, relic3.modification);
            v.level++;
            player.playerRelics.Add(v);
        }


        closeWindow();
    }
    public void StartLevelUp()
    {
        upgradeCanvas.enabled = true;
        relic1 = relicBank.relics[Random.Range(0, relicBank.relics.Count)];
        relic2 = relicBank.relics[Random.Range(0, relicBank.relics.Count)];
        relic3 = relicBank.relics[Random.Range(0, relicBank.relics.Count)];

        if (doesPlayerHaveRelic(relic1))
        {
            relic1 = returnRelicFromPlayer(relic1);
        }

        if (doesPlayerHaveRelic(relic2))
        {
            relic2 = returnRelicFromPlayer(relic2);
        }

        if (doesPlayerHaveRelic(relic3))
        {
            relic3 = returnRelicFromPlayer(relic3);
        }

        panel1.transform.Find("RelicName").GetComponent<TextMeshProUGUI>().text = relic1.RelicName;
        panel2.transform.Find("RelicName").GetComponent<TextMeshProUGUI>().text = relic2.RelicName;
        panel3.transform.Find("RelicName").GetComponent<TextMeshProUGUI>().text = relic3.RelicName;

        panel1.transform.Find("RelicLevel").GetComponent<TextMeshProUGUI>().text = relic1.RelicName;
        panel2.transform.Find("RelicLevel").GetComponent<TextMeshProUGUI>().text = relic2.RelicName;
        panel3.transform.Find("RelicLevel").GetComponent<TextMeshProUGUI>().text = relic3.RelicName;

        panel1.transform.Find("Relic Boost").GetComponent<TextMeshProUGUI>().text = relic1.modification[relic1.level].ToString();
        panel2.transform.Find("Relic Boost").GetComponent<TextMeshProUGUI>().text = relic2.modification[relic2.level].ToString();
        panel3.transform.Find("Relic Boost").GetComponent<TextMeshProUGUI>().text = relic3.modification[relic3.level].ToString();

        //get three random relics
        //put them in panels
        //update them in player already has picked them
        //
    }
    
    public void closeWindow()
    {
        upgradeCanvas.enabled = false;
    }

    public vRelic returnRelicFromPlayer(vRelic r)
    {
        for (int i = 0; i < player.playerRelics.Count; i++)
        {
            if (r.RelicName == player.playerRelics[i].RelicName)
            {
                return player.playerRelics[i];
            }
        }

        return null;
    }
    public bool doesPlayerHaveRelic(vRelic r)
    {
        for (int i = 0; i < player.playerRelics.Count; i++)
        {
            if (r.RelicName == player.playerRelics[i].RelicName)
            {
                return true;
            }
        }

        return false;
    }
}
