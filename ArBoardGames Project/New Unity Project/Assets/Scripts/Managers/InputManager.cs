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
    }

    public bool SelectObject(GameObject obj)
    {

        return true;
    }

    public void DeSelectObject(GameObject obj)
    {

    }

    public void DeSelectObject()
    {
        selectedObject = null;
    }


}
