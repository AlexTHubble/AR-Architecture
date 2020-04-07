using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePiece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    OnScreenDebugLogger screenDebugger;

    [SerializeField]
    InputManager inputManager;

    virtual public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnSelect();
    }

    //What will be called when the ray hits this object
    public virtual void OnSelect()
    {
        screenDebugger.LogOnscreen(name + "Game object clicked");
        inputManager.SelectObject(this.gameObject);
    }

    public virtual void OnDeselect()
    {
        inputManager.DeSelectObject(this.gameObject);
    }
}
