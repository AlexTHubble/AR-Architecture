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

    public static ArScenemanager instance = null;

    [SerializeField]
    private ARSessionOrigin arSessionOrigin;
    [SerializeField]
    private ARRaycastManager arRaymanager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<ARInteractableObject> activeObjects = new List<ARInteractableObject>();
    
    ARInteractableObject selectedObject = null;

    ARInteractableObject objectToSpawn = null;

    bool spawningObject = false;
    bool readyToSpawn = false;

    string heldTimerName = "TimerHeldTimer";
    float heldTimerLength = 1f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
        TimerTool.instance.AddTimer(heldTimerName, heldTimerLength);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private bool HandleInput()
    {
        //If the player has no input
        if (Input.touchCount == 0)
            return false;

        Touch touch = Input.GetTouch(0);

        if (spawningObject) //Logic for spawning an object
        {
            //Finger has been lifted && it's been placed somewhere, activate the gameobject
            if (touch.phase == TouchPhase.Ended && readyToSpawn)
            {
                objectToSpawn.SetToActive();
                objectToSpawn = null;
                spawningObject = false;
                readyToSpawn = false;
            }
            else if (arRaymanager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))//Finger is being held, move the position
            {
                Pose pose = hits[0].pose;

                //Set the gameobject back to active now that it's being place
                if (!objectToSpawn.gameObject.activeInHierarchy)
                {
                    objectToSpawn.gameObject.SetActive(true);
                    readyToSpawn = true;
                }

                //Sets the object transform and rotation to the pose postion
                objectToSpawn.gameObject.transform.position = pose.position;
                objectToSpawn.gameObject.transform.rotation = pose.rotation;
            }
        }
        else if(selectedObject && selectedObject.objectType == ARInteractableObject.OBJTYPE.HOLDSELECT)
        {
            switch(touch.phase) //Call the appopriate functions based on the touch phase
            {
                case TouchPhase.Stationary:
                    selectedObject.OnHeld();
                    break;
                case TouchPhase.Moved:
                    selectedObject.OnHeldMove();
                    break;
                case TouchPhase.Ended:
                    DeSelectObject();
                    break;
            }
        }

        return true;
    }

    //Select an inputed object, returns if the object can be selected or not
    public bool SelectObject(ARInteractableObject obj)
    {
        //Return false if the AR manager is attempting to spawn an object
        //This is to prevent objects frombeing interacted with while placing one
        if (spawningObject)
            return false;

        switch(obj.objectType)
        {
            case ARInteractableObject.OBJTYPE.TAPSELECT:
                return TapSelectLogic(obj);
                break;
            case ARInteractableObject.OBJTYPE.HOLDSELECT:
                return HoldSelectLogic(obj);
                break;
        }

        //If we don't get to this point we need to return false
        return false; 
    }

    public void DeSelectObject(ARInteractableObject obj) //Checks to see if the object is selected, if so deselect
    {
        //Checks to see if the object is actully selected
        if (obj == selectedObject)
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

    //Sets the object sent in to the object to spawn
    public void SelectObjectToSpawn(GameObject prefabToSpawn)
    {
        //Create and spawn in the new object
        objectToSpawn = Instantiate(prefabToSpawn).GetComponent<ARInteractableObject>();
        objectToSpawn.gameObject.SetActive(false); //Sets the gameobject to not active on first spawn
        spawningObject = true;

    }

    //The logic for tap select, mostly here for readablity
    private bool TapSelectLogic(ARInteractableObject obj)
    {
        if (selectedObject == null) //If the object hasn't been set yet
        {
            selectedObject = obj;
        }
        else if (selectedObject != obj) //If the object is a new object, deselcect then set the in object
        {
            DeSelectObject();
            selectedObject = obj;
        }
        else //If it's clicking on the same object, deselect it
        {
            DeSelectObject();
            return false; //Return false sence we're deselecting the object
        }

        return true;
    }

    private bool HoldSelectLogic(ARInteractableObject obj)
    {
        // If it's a hold selection object, stay selected untill touch is realeased
        if (selectedObject == null) //If the object hasn't been set yet
        {
            selectedObject = obj;
        }
        return true;
    }
}
