using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeTest : ARInteractableObject//, IPointerClickHandler
{
    [SerializeField]
    Material testMat;

    [SerializeField]
    Material testMat2;

    MeshRenderer mr;

    bool isMat1 = true;

    internal override void OnSelect()
    {

        screenDebugger.LogOnscreen("Working from override");
        base.OnSelect();
    }
}
