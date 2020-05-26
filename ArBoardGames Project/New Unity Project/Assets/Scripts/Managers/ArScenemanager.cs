using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;


//This is the manager to keep track of the scene being used for AR
public class ArScenemanager : MonoBehaviour
{
    /* What is this doing?
 * -This is a script to handle the AR scene, mananging the AR objects, ect.
 * -This wraps all of the AR functions needed in various game objects (such as raycasting, creating planes, ect)
 */

    [SerializeField]
    private ARSessionOrigin arSessionOrigin;
    [SerializeField]
    private ARRaycastManager arRaymanager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    ARInteractableObject selectedObject;

    [SerializeField]
    OnScreenDebugLogger screenDebugger;

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
        selectedObject = null;

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

    //Select an inputed object
    public bool SelectObject(ARInteractableObject obj)
    {
        //Call on deselect
        if (selectedObject != obj)
        {
            DeSelectObject(selectedObject);
        }

        selectedObject = obj;

        return true;
    }

    public void DeSelectObject(ARInteractableObject obj) //Checks to see if the object is selected, if so deselect
    {
        //Checks to see if the object is actully selected
        if (obj != selectedObject)
        {
            //Call the on deselect function of the selected object
            selectedObject.OnDeselect();
            selectedObject = null;
        }
    }

    public void DeSelectObject() //Deselect the current object regardless of what it is
    {
        //Call the on deselect function of the selected object
        selectedObject.OnDeselect();

        selectedObject = null;
    }
}
