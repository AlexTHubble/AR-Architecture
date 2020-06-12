using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    struct initedBeacon
    {
        string beaconUUID;
        GameObject gameObjectToSpawn;
        Vector3 location;
        bool inited;
    }

    //We're useing the phone's gyroscope to help get the exact position of the beacons
    bool gyroEnabled = false;
    Gyroscope gyro;
    Quaternion baseLookRot; //The look rotation inited with the program, we will compare the 

    private List<Beacon> mybeacons = new List<Beacon>(); //List of currently found beacons (within 10 seconds)

    [SerializeField]
    List<initedBeacon> initedBeacons;

    // Start is called before the first frame update
    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //We're calculating this based on the beacon's distance and the angle between the current look rot and the base
    private void CalculateBeaconLocation(Beacon beacon)
    {
        Quaternion currentLookRot = gyro.attitude * new Quaternion(0, 0, 1, 0);

        double angle = Quaternion.Angle(currentLookRot, baseLookRot);

        //X = Distance * cos(angle) || Y = Distance * sin(angle) || z = 0
        Vector3 beaconPosition = new Vector3((float)(beacon.accuracy * Math.Cos(angle)), (float)(beacon.accuracy * Math.Sin(angle)), 0);


    }

    //This event is called when a beacon's status is updated, this keeps track of all the current beacons in the list
    private void OnBeaconRangeChanged(Beacon[] beacons)
    { // 
        foreach (Beacon b in beacons)
        {
            var index = mybeacons.IndexOf(b);
            if (index == -1)
            {
                mybeacons.Add(b);
            }
            else
            {
                mybeacons[index] = b;
            }
        }
        for (int i = mybeacons.Count - 1; i >= 0; --i)
        {
            if (mybeacons[i].lastSeen.AddSeconds(10) < DateTime.Now)
            {
                mybeacons.RemoveAt(i);
            }
        }
    }

    public void SetLookRotToCurrentGyro()
    {
        baseLookRot = gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}
