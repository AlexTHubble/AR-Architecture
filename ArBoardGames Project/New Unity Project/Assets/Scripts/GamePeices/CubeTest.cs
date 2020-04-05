using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : GamePiece
{
    [SerializeField]
    Material testMat;

    [SerializeField]
    Material testMat2;

    MeshRenderer mr;

    bool isMat1 = true;

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
