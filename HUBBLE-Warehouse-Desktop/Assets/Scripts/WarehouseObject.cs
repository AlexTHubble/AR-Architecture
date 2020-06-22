using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarehouseObject : MonoBehaviour
{
    [HideInInspector]
    public string UUID;
    [HideInInspector]
    public Transform objTransform;

    private BoxCollider2D objCollider;

    bool held = false;

    private void Awake()
    {
        objTransform = transform;
        objCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(held)
        {
            Vector3 newpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 25.4f));
            objTransform.position = newpos;
        }
    }

    void OnMouseDown()
    {
        held = true;
    }

    void OnMouseUp()
    {
        held = false;
    }
}
