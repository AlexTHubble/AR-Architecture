using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

// POTENTIAL FIX FOR RAYCAST ISSUE
//https://forum.unity.com/threads/raycasting-objects-by-its-tag-with-arfoundation.578989/

public class InputManager : MonoBehaviour
{
    GamePiece selectedObject;

    [SerializeField]
    BoardManager bm;

    [SerializeField]
    private ARRaycastManager arRaymanager;

    [SerializeField]
    OnScreenDebugLogger screenDebugger;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();



    // Start is called before the first frame update
    void Start()
    {
        selectedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        TouchBegin();
        TouchEnd();
        TouchMove();

        //if(bm.boardActive)
        //{
        //    if (selectedObject == null) //Null check to make sure no more than one object is selected
        //        TouchBegin();
        //
        //    if (selectedObject != null)
        //        TouchMove();
        //
        //    if (selectedObject != null)
        //        TouchEnd();
        //}
    }

    void TouchMove()
    {
        //TODO: Move object
    }

    void TouchEnd()
    {
        Touch touch = Input.GetTouch(0);

        //RaycastHit hit;


        //On tap, raycast, on hit with object, select it 
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            if (selectedObject != null)
                selectedObject.OnDeselect();

            selectedObject = null;
        }
    }

    void TouchBegin()
    {
        Touch touch = Input.GetTouch(0);

        //RaycastHit hit;


        //On tap, raycast, on hit with object, select it 
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
        
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        
            // Create a particle if hit
        
        
            if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Interactable")))
            {
        
                GameObject objHit = hit.collider.gameObject;
                if (objHit.tag == "GamePeice")
                {
                    if (selectedObject != null)
                        selectedObject.OnDeselect();
                    selectedObject = null;
                    selectedObject = gameObject.GetComponent<GamePiece>();
                    selectedObject.OnSelect();
                }
            }
        }
    }



}
