using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarehouseObject : ARInteractableObject
{

    public string UUID;

    string description;
    bool isActivated = false;

    [SerializeField]
    Canvas operationsCanvas;

    internal override void OnSelect()
    {
        base.OnSelect();
        
        //operationsCanvas.enabled = true;
    }

    internal override void OnDeselect()
    {
        base.OnDeselect();

        //operationsCanvas.enabled = false;
    }

    public void DisplayDescription(TextMesh display)
    {
        display.text = description;
    }

    public void DisplayObject()
    {
        objectMR.enabled = true;
    }

    public void HideObject()
    {
        objectMR.enabled = false;
    }

    public void btn_MoveObject()
    {
        //TODO: This
    }

    public void btn_RemoveObject()
    {
        //TODO: This
    }
}
