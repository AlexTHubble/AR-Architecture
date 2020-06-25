using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingCube : ARInteractableObject
{
    //Move a cube while held based on the touch position
    Vector2 previousTouchPostion = Vector2.zero;
    Vector3 startingPos;

    [SerializeField]
    float movementPerTouchPixelMovement = 10; //Since we're just shifting the object based on the touch movement to a unit in unity space

    internal override void OnStartFunctions()
    {
        base.OnStartFunctions();
        startingPos = objTransform.position;
    }

    internal override void OnSelect()
    {
        base.OnSelect();
        previousTouchPostion = currentTouch.position; //Setup the previous touch to the first instance of touch in the held sequence
    }

    internal override void OnHeld()
    {
        base.OnHeld();

        OnScreenDebugLogger.instance.LogOnscreen(currentTouch.position.ToString() + " "  + Time.time.ToString()); 

       if (currentTouch.position != previousTouchPostion)
       {
           ShiftUpwards(currentTouch.position);
           ShiftSideways(currentTouch.position);
       }
    }

    internal override void OnDeselect()
    {
        base.OnDeselect();
        objTransform.position = startingPos; //When it's deselcted, reset it's postion
    }

    //Touch position is in screen coords, so the top left corner is (0,0). 
    //This means that if the y goes down in coords, it's going up on the screen
    void ShiftUpwards(Vector2 updatedTouchPos)
    {
        float yDiff = previousTouchPostion.y - updatedTouchPos.y; 

        Vector3 tempPos = objTransform.position;
        tempPos.y += (yDiff * movementPerTouchPixelMovement); //Converts and adds
        objTransform.position = tempPos;
    }

    //Touch position is in screen coords, so the top left corner is (0,0). 
    //This means that if the y goes down in coords, it's going up on the screen
    void ShiftSideways(Vector2 updatedTouchPos)
    {

    }
}
