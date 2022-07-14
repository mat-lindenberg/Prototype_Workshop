using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VTimer : MonoBehaviour
{
    public float playtimeSinceStart;

    public TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        playtimeSinceStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playtimeSinceStart += Time.deltaTime;
        int minutes = Mathf.FloorToInt(playtimeSinceStart / 60F);
        int seconds = Mathf.FloorToInt(playtimeSinceStart - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = niceTime;

    }
}
