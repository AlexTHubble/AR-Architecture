using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTool : MonoBehaviour
{
    /* WHAT IS THIS
     * This is a simple timer tool, I use this timer structure alot so I made it easy to use.
     * It simply takes a target in the time by adding a delay to the time of start, then checks Time.time to see
     * if the time has passed the mark set by the timer. This creates a light weight timer that doesn't require
     * Coroutines.
     * 
     * HOW TO USE
     * Thow this onto a manager object somewhere in the Unity Hirearchy for the singleton to work. 
     * In your script, call AddTimer to init the new timer in the script, then call StartTimer whenever you want
     * to start counting.
     * Currently you need to check the timer Manually using the CheckTimer function.
     * 
     * TODO
     * - Make the timer send events when done 
     */


    struct TimerObj
    {
        public float timerLength;
        public float currentTime;
        public bool finished;
    }

    public static TimerTool instance;

    Dictionary<string, TimerObj> timerDictionary = new Dictionary<string, TimerObj>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    //Add a new timer to the tool
    public void AddTimer(string name, float length)
    {
        TimerObj newTimer = new TimerObj();
        newTimer.timerLength = length;
        newTimer.finished = false;

        timerDictionary.Add(name, newTimer);
    }

    public void StartTimer(string name)
    {
        TimerObj temp = timerDictionary[name];

        temp.finished = false;
        temp.currentTime = Time.time + temp.timerLength;

        timerDictionary[name] = temp;
    }

    public bool CheckTimer(string name)
    {
        //If the timer has finished
        if(timerDictionary[name].currentTime > Time.time)
        {
            TimerObj temp = timerDictionary[name];
            temp.finished = true;
            temp.currentTime = 0;
            timerDictionary[name] = temp;

            return true;
        }

        return false;
    }

}
