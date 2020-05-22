using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

//This is the manager to keep track of the scene being used for AR
public class ArScenemanager : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin arSessionOrigin;
    [SerializeField]
    private ARRaycastManager arRaymanager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject cube;
    [SerializeField]
    GameObject canvas;

    [HideInInspector]
    public bool boardActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //arSessionOrigin = GetComponent<ARSessionOrigin>();
        //arRaymanager = GetComponent<ARRaycastManager>();
        cube.SetActive(false);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player has no input, break
        if (Input.touchCount == 0 || boardActive)
            return;

        Touch touch = Input.GetTouch(0);


        if (arRaymanager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon)/* && IsPointerOverUiObject(touch.position)*/)
        {
            Pose pose = hits[0].pose;

            if (!cube.activeInHierarchy)
            {
                cube.SetActive(true);
                cube.transform.position = pose.position;
                cube.transform.rotation = pose.rotation;

                canvas.SetActive(true);
                boardActive = true;
            }
            else
            {
                cube.transform.position = pose.position;
                boardActive = true;
            }
        }
    }
}
