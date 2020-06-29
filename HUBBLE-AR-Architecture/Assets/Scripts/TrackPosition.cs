using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackPosition : MonoBehaviour
{
    //Just something quick to let me track an object on screen

    [SerializeField]
    Transform positionToTrack;

    [SerializeField]
    TextMeshProUGUI displayOutput;

    [SerializeField]
    string objName;

    Vector3 lastPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        //If the position moved, display the new postion
        if(positionToTrack.position != lastPos)
        {
            lastPos = positionToTrack.position;
            displayOutput.text = objName + " X: " + lastPos.x.ToString() + " Y: " + lastPos.y.ToString() + " Z: " + lastPos.z.ToString();
        }
    }
}
