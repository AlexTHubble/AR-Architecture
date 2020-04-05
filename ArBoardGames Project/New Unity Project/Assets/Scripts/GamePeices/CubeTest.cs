using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CubeTest : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Material testMat;

    [SerializeField]
    Material testMat2;

    MeshRenderer mr;

    bool isMat1 = true;

    [SerializeField]
    OnScreenDebugLogger screenDebugger;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        screenDebugger.LogOnscreen(name + "Game object clicked");
    }

    //public override void OnSelect()
    //{
    //    if(isMat1)
    //    {
    //        mr.material = testMat;
    //        isMat1 = false;
    //    }
    //    else
    //    {
    //        mr.material = testMat2;
    //        isMat1 = true;
    //    }
    //    base.OnSelect();
    //}
}
