using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Use this as a parent for any AR interactable object
public class ARInteractableObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    /* SELECTING AND DE SELECTING
     * This is a bit convoluted so I'm putting it in a note
     * 
     * So this script will handle SELECTING, this is so we can use Unity's build in OnPointerClick function
     * for our input, this will call the AR manager to set this object as selected
     * 
     * However, the AR manager will deterimine when the object gets deselected (calling our on deselect function)
     * This is because the AR scene manager is keeping track of what object is currently active and all of the 
     * objects.
     * 
     * There is a public wrapper in this function to deselect this AR object, this will call the on deselect in
     * the scene manager. This is just so if a script knows about this object but doesn't know about the scene manager
     * it can still deselct an object manualy if wanted.
     */

    public enum OBJTYPE
    {
        NULL = -1,
        TAPSELECT = 0, //Selecting this object is on a tap, will deselct when tapped again or another object is selected
        HOLDSELECT, //Selecting this object is on hold, will deselect on hold off
        COUNT
    }

    public OBJTYPE objectType = OBJTYPE.TAPSELECT;

    //Use this to save some time searching for components
    [HideInInspector]
    public Transform objTransform;

    //Use this to save some time searching for components
    [HideInInspector]
    public MeshRenderer objectMR;

    bool canBeInteracted = false;
    Material defaultMat;
    Material selectedMat;

    float clickDelay = 1f;
    string timerName = "ARInteractableClickDelay";

    bool selected = false;

    //Use this in the virtual fucntions for various reasons
    protected Touch currentTouch;

    private void Start()
    {
        OnStartFunctions();
        timerName = TimerTool.instance.AddTimer(timerName, clickDelay); //Sets up the new timer
    }

    public void OnPointerClick(PointerEventData pointerEventData) // Triggers ONCE at the end of press (finger release)
    {
        if(canBeInteracted && TimerTool.instance.CheckTimer(timerName) && objectType == OBJTYPE.TAPSELECT) //This is a redudent check, but should make things faster when spawning
        {
            OnSelect();
        }

    }

    public void OnPointerDown(PointerEventData pointerEventData) //Called continuesly while finger is on the object
    {
        OnScreenDebugLogger.instance.LogOnscreen("POINTER DOWN IS BEING CALLED");

        //If the object is hold select and can be interacted
        if(canBeInteracted && objectType == OBJTYPE.HOLDSELECT)
        {
            if (!selected) //Select the object on the first moment of being held
                OnSelect();

            OnHeld();
        }
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //If it's currently being held & it's selected, call on deselct
        if (canBeInteracted && objectType == OBJTYPE.HOLDSELECT && selected)
            ArScenemanager.instance.DeSelectObject(this);
    }

    //In sets the object to active, meaning it can now be interacted with
    public void SetToActive()
    {
        canBeInteracted = true;
        objectMR.material = defaultMat;
    }

    //A public wrapper if you want to deselct this object without a refrence to the AR scene manager
    public void DeselectObject()
    {
        ArScenemanager.instance.DeSelectObject(this);
    }

    //What happens when the object is selected, use this override in children
    internal virtual void OnSelect()
    {
        if (!ArScenemanager.instance.SelectObject(this)) //Checks to see if it can be selected
            return;

        if (Input.touchCount != 0)
            currentTouch = Input.GetTouch(0); //If there is a touch, get it here

        objectMR.material = selectedMat; //Sets the material to the "selected" material
        OnScreenDebugLogger.instance.LogOnscreen(name + "Game object clicked");
        TimerTool.instance.StartTimer(timerName); //Start the delay timer

        selected = true;
    }

    //What will be called when the object is deselected, use this override in children
    internal virtual void OnDeselect()
    {
        objectMR.material = defaultMat;
        TimerTool.instance.StartTimer(timerName);
        OnScreenDebugLogger.instance.LogOnscreen(name + " deselected");

        selected = false;
    }

    //What will be called if the object is being held, use this override in children
    internal virtual void OnHeld()
    {
        OnScreenDebugLogger.instance.LogOnscreen(name + " Held");

        if (Input.touchCount != 0)
            currentTouch = Input.GetTouch(0); //If there is a touch, get it here
    }

    //Stuff that needs to be called on start, use this override in children
    internal virtual void OnStartFunctions()
    {
        objTransform = gameObject.transform;
        objectMR = gameObject.GetComponent<MeshRenderer>();
        //Saves the default mat as the material on the object, then sets the material to the selecting pos mat
        defaultMat = objectMR.material;
        objectMR.material = Resources.Load<Material>("Materials/SelectingPosMat");
        //Load the material for selection
        selectedMat = Resources.Load<Material>("Materials/SelectedMat"); 
    }
}
