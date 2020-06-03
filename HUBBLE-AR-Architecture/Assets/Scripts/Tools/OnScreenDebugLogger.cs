using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//A quick on screen debugger to help with android builds
//TODO: Make this a singleton for ease of use
public class OnScreenDebugLogger : MonoBehaviour
{

    public static OnScreenDebugLogger instance = null;

    List<string> logList = new List<string>();

    [SerializeField]
    float delay = 1;

    [SerializeField]
    List<TextMeshProUGUI> UIList = new List<TextMeshProUGUI>();

    //bool isWaiting = false;
    float currentTimer = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > currentTimer)
        {
            currentTimer = Time.time + delay;
            UpdateUI();
        }
    }

    //Displays the top N results (where N is the amount of logger room in the list)
    private void UpdateUI()
    {
        //Display the text & remove them from the log
        if (logList.Count >= 1)
        {
            UIList[0].text = logList[0];
            logList.RemoveAt(0);
        }
        if (logList.Count >= 2)
        {
            UIList[1].text = logList[1];
            logList.RemoveAt(1);
        }
        if (logList.Count >= 3)
        {
            UIList[2].text = logList[2];
            logList.RemoveAt(2);
        }
    }

    public void LogOnscreen(string message)
    {
        logList.Add(message);
    }
}
