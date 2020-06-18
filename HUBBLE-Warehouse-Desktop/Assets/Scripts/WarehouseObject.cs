using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseObject : MonoBehaviour
{
    [HideInInspector]
    public string UUID;
    [HideInInspector]
    public Transform objTransform;

    private BoxCollider2D objCollider;

    // Start is called before the first frame update
    void Start()
    {
        objTransform = transform;
        objCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMouseInput();
    }

    private void CheckForMouseInput()
    {
        Vector2 mousePos = Input.mousePosition;

        //Mouse is down and within the bounds of the collider
        if(Input.GetMouseButtonDown(0) && objCollider.bounds.Contains(mousePos))
        {
            //TODO: ON click functionality
        }
    }
}
