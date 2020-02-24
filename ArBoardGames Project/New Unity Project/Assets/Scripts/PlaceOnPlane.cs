using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


public class PlaceOnPlane : MonoBehaviour
{
    private ARSessionOrigin arSessionOrigin;
    private ARRaycastManager arRaymanager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject cube;
    [SerializeField]
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        arSessionOrigin = GetComponent<ARSessionOrigin>();
        arRaymanager = GetComponent<ARRaycastManager>();
        cube.SetActive(false);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player has no input, break
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        
        if (arRaymanager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon) && IsPointerOverUiObject(touch.position))
        {
            Pose pose = hits[0].pose;

            if(!cube.activeInHierarchy)
            {
                cube.SetActive(true);
                cube.transform.position = pose.position;
                cube.transform.rotation = pose.rotation;

                canvas.SetActive(true);
            }
            else
            {
                cube.transform.position = pose.position;
            }
        }
    }

    bool IsPointerOverUiObject(Vector2 pos)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
