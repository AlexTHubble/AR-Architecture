using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BeaconSpawnObject", order = 1)]
public class BeaconSpawnObject : ScriptableObject
{
    public string beaconUUID; //The id of the beacon that this object is tied to
    public GameObject prefabToSpawn; //The gameobject that will spawn at the location

    [HideInInspector]
    public Vector3 beaconPosition;
    [HideInInspector]
    public Quaternion lookRot;
    [HideInInspector]
    public float savedAngle;
    [HideInInspector]
    public GameObject spawnedObject;
    [HideInInspector]
    public bool inited = false;
    [HideInInspector]
    public bool beaconEnabled = false;
    [HideInInspector]
    public Beacon theBeacon;
}
