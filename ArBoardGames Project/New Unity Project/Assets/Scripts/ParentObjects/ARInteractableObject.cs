using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Use this as a parent for any AR interactable object
public class ARInteractableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected OnScreenDebugLogger screenDebugger;

    [SerializeField]
    InputManager inputManager;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnSelect();
    }

    //A public wrapper if you want to deselct this object without a refrence of the input manager
    public void DeselectObject()
    {
        inputManager.DeSelectObject(this);
    }

    //What will be called when the ray hits this object
    internal virtual void OnSelect()
    {
        screenDebugger.LogOnscreen(name + "Game object clicked");
        inputManager.SelectObject(this);
    }

    //What will be called when the object is deselected
    internal virtual void OnDeselect()
    {
        screenDebugger.LogOnscreen(name + " deselected");
    }
}
