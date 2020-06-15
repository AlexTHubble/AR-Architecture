using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaconManager : MonoBehaviour
{
    enum SetupSteps
    {
        NULL = -1,
        GETDEFAULTROT,
        LOOKINGATBEACON,
        FINISHED
    }

    SetupSteps currentSetupStep = SetupSteps.NULL;

    //We're useing the phone's gyroscope to help get the exact position of the beacons
    bool gyroEnabled = false;
    Gyroscope gyro;
    Quaternion baseLookRot; //The look rotation inited with the program, we will compare the 

    //private List<Beacon> enabledBeacons = new List<Beacon>(); //List of currently found beacons (within 10 seconds)

    [SerializeField]
    List<BeaconSpawnObject> beaconObjects = new List<BeaconSpawnObject>();
    //KEY: Beacon UUID | this is just for quick find since dictionarys don't play nice with being Serialized
    Dictionary<string, BeaconSpawnObject> beaconObjectDictionary = new Dictionary<string, BeaconSpawnObject>();

    // Start is called before the first frame update
    void Start()
    {
        gyroEnabled = EnableGyro();
        InitDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupBeacon()
    {
        switch(currentSetupStep)
        {
            case SetupSteps.NULL:
                return;
                break;
            case SetupSteps.GETDEFAULTROT:
                //1. Display button

                //2. Wait for button call to swap setup step

                break;
            case SetupSteps.LOOKINGATBEACON:
                //1. Display button
                foreach(BeaconSpawnObject bso in beaconObjects)
                {
                    if(bso.beaconEnabled && bso.inited)
                    {

                    }
                }

                //2. wait for the button call to swap step

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
    { // 
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

    public void SetLookRotToCurrentGyro()
    {
        baseLookRot = gyro.attitude * new Quaternion(0, 0, 1, 0);
    }

    public void btn_BeginBeaconSetup()
    {
        currentSetupStep = SetupSteps.GETDEFAULTROT;
    }

    public void btn_GetDefaultRotation(Button buttonPressed)
    {
        if(gyroEnabled)
        {
            baseLookRot = gyro.attitude;
            currentSetupStep = SetupSteps.LOOKINGATBEACON;
            buttonPressed.gameObject.SetActive(false); //turn off the button once this phase is done
        }
    }

    public void btn_LookingAtBeacon(Button buttonPressed)
    {

    }
}
