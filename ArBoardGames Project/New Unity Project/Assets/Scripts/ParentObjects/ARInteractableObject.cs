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
    Material defaultMat;
    Material selectedMat;

    private void Start()
    {
        OnStartFunctions();
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
        objectMR.material = defaultMat;
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

        objectMR.material = selectedMat; //Sets the material to the "selected" material
        screenDebugger.LogOnscreen(name + "Game object clicked");
    }

    //What will be called when the object is deselected
    internal virtual void OnDeselect()
    {
        screenDebugger.LogOnscreen(name + " deselected");
        objectMR.material = defaultMat;
    }

    //Stuff that needs to be called on start
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
