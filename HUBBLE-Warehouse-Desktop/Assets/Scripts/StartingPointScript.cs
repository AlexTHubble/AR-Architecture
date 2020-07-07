using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPointScript : MonoBehaviour
{
    [HideInInspector]
    public Transform objTransform;

    private BoxCollider2D objCollider;

    Transform camTransform;

    bool held = false;

    private void Awake()
    {
        objTransform = transform;
        objCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (held)
        {
            Vector3 newpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, (camTransform.position.z * -1f)));

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
