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
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private bool HandleInput()
    {
        //If the player has no input, there is an object to spawn, and no object is currently selected
        if (Input.touchCount == 0 && !spawningObject)
            return false;

        Touch touch = Input.GetTouch(0);

        //Finger has been lifted && it's been placed somewhere, activate the gameobject
        if(touch.phase == TouchPhase.Ended && readyToSpawn)
        {
            objectToSpawn.SetToActive();
            objectToSpawn = null;
            spawningObject = false;
            readyToSpawn = false;
        }
        else if(arRaymanager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))//Finger is being held, move the position
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

        return true;
    }

    //Select an inputed object, returns if the object can be selected or not
    public bool SelectObject(ARInteractableObject obj)
    {
        //Return false if the AR manager is attempting to spawn an object
        //This is to prevent objects frombeing interacted with while placing one
        if (spawningObject)
            return false;

        if(selectedObject == null) //If the object hasn't been set yet
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
}
