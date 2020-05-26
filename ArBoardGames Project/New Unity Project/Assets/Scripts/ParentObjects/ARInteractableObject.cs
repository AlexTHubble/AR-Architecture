using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Use this as a parent for any AR interactable object
public class ARInteractableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected ArScenemanager arSceneManagerObj;

    [SerializeField]
    protected OnScreenDebugLogger screenDebugger;

    //Use this to save some time searching for components
    [HideInInspector]
    public Transform objTransform;

    //Use this to save some time searching for components
    [HideInInspector]
    public MeshRenderer objectMR;

    bool canBeInteracted = false;

    private void Start()
    {
        objTransform = gameObject.transform;
        objectMR = gameObject.GetComponent<MeshRenderer>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(canBeInteracted)
            OnSelect();
    }

    //In sets the object to active, meaning it can now be interacted with
    public void SetToActive()
    {
        canBeInteracted = true;

        //TODO: Swap the MR to an active renderer
    }

    //A public wrapper if you want to deselct this object without a refrence of the input manager
    public void DeselectObject()
    {
        arSceneManagerObj.DeSelectObject(this);
    }

    //What happens when the object is selected
    internal virtual void OnSelect()
    {
        if (!arSceneManagerObj.SelectObject(this))
            return;

        screenDebugger.LogOnscreen(name + "Game object clicked");
    }

    //What will be called when the object is deselected
    internal virtual void OnDeselect()
    {
        screenDebugger.LogOnscreen(name + " deselected");
    }
}
