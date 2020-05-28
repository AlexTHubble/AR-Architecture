using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeTest : ARInteractableObject//, IPointerClickHandler
{
    internal override void OnSelect()
    {
        
        base.OnSelect();
        screenDebugger.LogOnscreen("Working from override");
    }
}
