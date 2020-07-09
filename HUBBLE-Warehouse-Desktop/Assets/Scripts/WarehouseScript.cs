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
    SpriteRenderer sr;

    bool zoomIn = true;
    bool active = true;
    bool firstUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        cameraTranform = Camera.main.transform;
        cameraTranform.position = new Vector3(cameraTranform.position.x, cameraTranform.position.y, cameraZStartPos);
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(firstUpdate) //Doing this because unity won't let me change start ordering
        {
            //If the corners are allready on screen, zoom till they arnt. If they're off screen. Zoom till they are
            zoomIn = TestIfCornersAreOnScreen();
            firstUpdate = false;
        }

        //Adjust the z position untill all of the corners are off screen

        if (zoomIn && active)
        {
            if (TestIfCornersAreOnScreen())
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

                //We need to adjust again since the corners are on the same z plane as the object, so doing a quick back track will fix that
                if(sr.isVisible) //If it's visiable we don't need to update anymore
                {
                    active = false;
                }
                else //Else move back untill it is
                {
                    cameraTranform.position = new Vector3(cameraTranform.position.x,
                        cameraTranform.position.y, cameraTranform.position.z - cameraMoveSpeed);
                }
            }
        }
        else if (active)
        {
            if (!TestIfCornersAreOnScreen())
            {
                cameraTranform.position = new Vector3(cameraTranform.position.x,
                    cameraTranform.position.y, cameraTranform.position.z - cameraMoveSpeed);
            }
            else
            {
                foreach (WarehouseObjectEdgeDetector corner in warehouseCorneres)
                {
                    corner.DisableSpriteRenderer();
                }
                active = false;

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
