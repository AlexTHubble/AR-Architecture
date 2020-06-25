using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseScript : MonoBehaviour
{
    [SerializeField]
    List<WarehouseObjectEdgeDetector> warehouseCorneres = new List<WarehouseObjectEdgeDetector>();

    [SerializeField]
    float cameraZStartPos;

    [SerializeField]
    float cameraMoveSpeed;

    Transform cameraTranform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTranform = Camera.main.transform;
        cameraTranform.position = new Vector3(cameraTranform.position.x, cameraTranform.position.y, cameraZStartPos);
    }

    // Update is called once per frame
    void Update()
    {
        //Adjust the z position untill all of the corners are off screen

        if(!TestIfCornersAreOnScreen())
        {
            cameraTranform.position = new Vector3(cameraTranform.position.x, 
                cameraTranform.position.y, cameraTranform.position.z + cameraMoveSpeed);
        }
        else
        {
            foreach (WarehouseObjectEdgeDetector corner in warehouseCorneres)
            {
                corner.DisableSpriteRenderer();
            }
        }
    }

    bool TestIfCornersAreOnScreen()
    {
        foreach (WarehouseObjectEdgeDetector corner in warehouseCorneres)
        {
            if (!corner.TestIsVisiable())
                return false;
        }

        return true;
    }
}
