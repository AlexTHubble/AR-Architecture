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
     * It's also important to note that this is a singleton pattern.
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

    //Add a new timer to the tool, returns the key used
    public string AddTimer(string name, float length)
    {
        TimerObj newTimer = new TimerObj();
        newTimer.timerLength = length;
        newTimer.finished = true; // Set to true as there has been no timer set yet

        //This will correct any duplicate names
        if(timerDictionary.ContainsKey(name))
        {
            Debug.Log("TimerTool.cs | WARNING: Duplicate timer added, function will return a new key that isn't duplicate");
            string newName = name;
            int i = 0;

            //Keep iterating i untill it hits a number that hasn't been useed
            while(timerDictionary.ContainsKey(newName))
            {
                newName = name + i.ToString();
                i++;
            }

            timerDictionary.Add(newName, newTimer);
            return newName;
        }
        else
        {
            timerDictionary.Add(name, newTimer);
            return name;
        }
    }

    //Starts the timer sent in
    public void StartTimer(string name)
    {
        if (timerDictionary.ContainsKey(name))
        {
            TimerObj temp = timerDictionary[name];

            temp.finished = false;
            temp.currentTime = Time.time + temp.timerLength;

            timerDictionary[name] = temp;
        }
        else
        {
            Debug.Log("TimerTool.cs | ERROR: Timer " + name + " not found");
        }
    }

    //Checks to see if the timer has been finished
    public bool CheckTimer(string name)
    {
        if(timerDictionary.ContainsKey(name))
        {
            //If the timer has finished, either though time or if the variable has allready been finished
            if (timerDictionary[name].currentTime < Time.time || timerDictionary[name].finished)
            {
                TimerObj temp = timerDictionary[name];
                temp.finished = true;
                temp.currentTime = 0;
                timerDictionary[name] = temp;

                return true;
            }
        }
        else
        {
            Debug.Log("TimerTool.cs | ERROR: Timer " + name + " not found");
        }

        return false;
    }
    
    //End the timer prematurely
    public void EndTimer(string name)
    {
        if(timerDictionary.ContainsKey(name))
        {
            TimerObj temp = timerDictionary[name];
            temp.finished = true;
            temp.currentTime = 0;
            timerDictionary[name] = temp;
        }
        else
        {
            Debug.Log("TimerTool.cs | ERROR: Timer " + name + " not found");
        }
    }

    //Remove a timer from the system
    public void DeleteTimer(string name)
    {
        if(timerDictionary.ContainsKey(name))
        {
            timerDictionary.Remove(name); 
        }
        else
        {
            Debug.Log("TimerTool.cs | ERROR: Timer " + name + " not found");

        }
    }

}
