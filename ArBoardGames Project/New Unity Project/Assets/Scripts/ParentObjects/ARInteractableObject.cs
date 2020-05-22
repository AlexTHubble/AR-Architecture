using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    //What will be called when the ray hits this object
    internal virtual void OnSelect()
    {
        screenDebugger.LogOnscreen(name + "Game object clicked");
        inputManager.SelectObject(this.gameObject);
    }

    internal virtual void OnDeselect()
    {
        inputManager.DeSelectObject(this.gameObject);
    }
}
