using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeTest : GamePiece, IPointerClickHandler
{
    [SerializeField]
    Material testMat;

    [SerializeField]
    Material testMat2;

    MeshRenderer mr;

    bool isMat1 = true;

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        OnSelect();
    }

    public override void OnSelect()
    {
        if(isMat1)
        {
            mr.material = testMat;
            isMat1 = false;
        }
        else
        {
            mr.material = testMat2;
            isMat1 = true;
        }
        base.OnSelect();
    }
}
