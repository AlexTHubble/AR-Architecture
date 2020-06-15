using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeaconManager : MonoBehaviour
{
    public enum SetupSteps
    {
        NULL = -1,
        GETDEFAULTROT,
        SEARCHINGFORUNINTEDBEACON,
        INITBEACON,
        FINISHED
    }

    SetupSteps currentSetupStep = SetupSteps.NULL;

    //We're useing the phone's gyroscope to help get the exact position of the beacons
    bool gyroEnabled = false;
    Gyroscope gyro;
    Quaternion baseLookRot; //The look rotation inited with the program, we will compare the 

    [SerializeField]
    List<BeaconSpawnObject> beaconObjects = new List<BeaconSpawnObject>();

    //KEY: Beacon UUID | this is just for quick find since dictionarys don't like being Serialized
    Dictionary<string, BeaconSpawnObject> beaconObjectDictionary = new Dictionary<string, BeaconSpawnObject>();
    
    BeaconSpawnObject lastUnitedBeaconFound; //This is the last beacon found

    [Header("UI Elements")]
    [SerializeField]
    Canvas canvas_FindDefaultRot;
    [SerializeField]
    Canvas canvas_SearchingForBeacon;
    [SerializeField]
    TextMeshProUGUI tmp_FoundBeaconCount;
    [SerializeField]
    Button btn_InitBeaconButton;
    [SerializeField]
    Canvas canvas_InitBeacon;

    List<Canvas> allCanvas = new List<Canvas>();

    // Start is called before the first frame update
    void Start()
    {
        gyroEnabled = EnableGyro();
        InitDictionary();
        InitAllCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBeaconStateLogic();
    }

    /*  STATE PROGRESSION 
     *  1. Something external calls either setup or start. 
     *      1a. If calls start we'll swap straight to SEARCHINGFORUNINTEDBEACON.
     *      1b. If calls setup we'll swap to GETDEFAULTROT and init the canvas used for getting the default rotation
     *  2. GETDEFAULTROT: Get's the default rotation of the phone. This is saved inside a JSON File for future use. 
     *     Once this is compleated go to step 3
     *  3. SEARCHINGFORUNINTEDBEACON: Our default state, this searches for uninited beacons then prompts the user to init the beacon.
     *      3a. If a beacon is found this way, show an alert on screen and prompt the user to init it
     *      3b. Once a user hit's the Init Beacon button, init the last beacon found
     *  4. INITBEACON: Setup the canvas and wait for the button press to confirm the user inited the beacon, once this is done
     *     move back to SEARCHINGFORUNINTEDBEACON (step 3).
     */
    void HandleBeaconStateLogic()
    {
        switch(currentSetupStep)
        {
            case SetupSteps.NULL:
                return;
                break;
            case SetupSteps.SEARCHINGFORUNINTEDBEACON:
                //This is our default state for the manager, if it's not activly setting up a beacon it's searching for any that may not
                //be initialized yet

                bool found = false;
                int count = 0;

                foreach (BeaconSpawnObject bso in beaconObjects)
                {
                    if (bso.beaconEnabled && !bso.inited)
                    {
                        lastUnitedBeaconFound = bso;
                        found = true;
                        count++;
                    }
                }

                if (found)
                {
                    /* We found a beacon that needs to be inited, push a notification onto the default canvas.
                     * The next step is started though a button press by the user
                     * 
                     * This will only count the last beacon found, once the inited progress is done we'll return to this
                     * default state and search again
                     */

                    tmp_FoundBeaconCount.text = count.ToString();
                    btn_InitBeaconButton.enabled = true; //Enable the beacon
                }
                else
                {
                    tmp_FoundBeaconCount.text = "0";
                    btn_InitBeaconButton.enabled = false; //Enable the beacon
                }

                break;

            case SetupSteps.GETDEFAULTROT:
                //This state is handled entirly in the buttons, there isn't any code that needs run on update
                break;

            case SetupSteps.INITBEACON:

                break;
        }
    }

    bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    void InitDictionary()
    {
        //Add each object in the list to the dictionary
        foreach(BeaconSpawnObject bso in beaconObjects)
        {
            if(bso.beaconUUID != "-1")
            {
                beaconObjectDictionary.Add(bso.beaconUUID, bso);
            }
        }
    }

    //This is just so I have an easy way to disable all the canvas without having a bunch of code every time I swap to a new canvas
    void InitAllCanvas()
    {
        allCanvas.Add(canvas_FindDefaultRot);
        allCanvas.Add(canvas_SearchingForBeacon);
        allCanvas.Add(canvas_InitBeacon);
    }

    //Disables all the canvas
    void DisableAllCanvas()
    {
        foreach (Canvas canvas in allCanvas)
            canvas.enabled = false;
    }

    //Swap the currently enabled canvas to the one passed in
    void SwapCanvas(Canvas canvasToSwapTo)
    {
        foreach(Canvas canvas in allCanvas)
        {
            if(canvas == canvasToSwapTo)
            {
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
            }
        }
    }

    //We're calculating this based on the beacon's distance and the angle between the current look rot and the base
    private Vector3 CalculateBeaconLocation(Beacon beacon)
    {
        Quaternion currentLookRot = gyro.attitude * new Quaternion(0, 0, 1, 0);

        double angle = Quaternion.Angle(currentLookRot, baseLookRot);

        //X = Distance * cos(angle) || Y = Distance * sin(angle) || z = 0
        Vector3 beaconPosition = new Vector3((float)(beacon.accuracy * Math.Cos(angle)), (float)(beacon.accuracy * Math.Sin(angle)), 0);

        return beaconPosition;
    }

    //This event is called when a beacon's status is updated, this keeps track of all the current beacons in the list
    private void OnBeaconRangeChanged(Beacon[] beacons)
    {  
        foreach (Beacon b in beacons)
        {
            //Beacon was found & has an object to spawn. Enable the beacon
            if (beaconObjectDictionary.ContainsKey(b.UUID))
            {
                BeaconSpawnObject bso = beaconObjectDictionary[b.UUID];
                bso.theBeacon = b;
                bso.beaconEnabled = true; 
            }
        }
        foreach(BeaconSpawnObject bso in beaconObjects)
        {
            //If there is a beacon, it's enabled, and it hasn't been seen for 10 seconds. Disable it
            if(bso.theBeacon != null && bso.beaconEnabled && bso.theBeacon.lastSeen.AddSeconds(10) < DateTime.Now)
            {
                bso.beaconEnabled = false;
            }
        }
    }

    //------------------------------------------------------------------------------------------------------
    //--------------------------------------UI BUTTON FUNCTIONS---------------------------------------------
    //------------------------------------------------------------------------------------------------------

    //Swap the state of the manager
    public void btnFunction_SwapState(SetupSteps stepToSwapTo)
    {
        currentSetupStep = stepToSwapTo;

        switch(stepToSwapTo)
        {
            case SetupSteps.INITBEACON:
                SwapCanvas(canvas_InitBeacon);
                break;
            case SetupSteps.GETDEFAULTROT:
                SwapCanvas(canvas_FindDefaultRot); 
                break;
            case SetupSteps.SEARCHINGFORUNINTEDBEACON:
                SwapCanvas(canvas_SearchingForBeacon);
                break;
        }
    }

    //Get the default rotation
    public void btnFunction_GetDefaultRotation()
    {
        if(gyroEnabled)
        {
            baseLookRot = gyro.attitude * new Quaternion(0, 0, 1, 0); //Set the base look rot 
        }
        else
        {
            OnScreenDebugLogger.instance.LogOnscreen("GYRO NOT ENABLED");
        }

        btnFunction_SwapState(SetupSteps.SEARCHINGFORUNINTEDBEACON);
    }

    //Button function to init the beacon, sets position, look rot, and then spawns the object
    public void btnFunction_InitBeacon()
    {
        if (gyroEnabled)
        {
            lastUnitedBeaconFound.beaconPosition = CalculateBeaconLocation(lastUnitedBeaconFound.theBeacon);
            lastUnitedBeaconFound.inited = true;
            lastUnitedBeaconFound.lookRot = gyro.attitude * new Quaternion(0, 0, 1, 0);
            lastUnitedBeaconFound.SpawnObject();
        }
        else
        {
            OnScreenDebugLogger.instance.LogOnscreen("GYRO NOT ENABLED");
        }

        btnFunction_SwapState(SetupSteps.SEARCHINGFORUNINTEDBEACON);
    }
}
