using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

//This is currently being used to manage what object is being selected, 
//this should be moved to the scene manager since the AR objects are managing their own input now
public class InputManager : MonoBehaviour
{
    ARInteractableObject selectedObject;

    [SerializeField]
    private ARRaycastManager arRaymanager;

    [SerializeField]
    OnScreenDebugLogger screenDebugger;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
    }

    // Update is called once per frame
    void Update()
    { 
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
