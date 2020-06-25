using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarehouseObject : MonoBehaviour
{
    [SerializeField]
    Canvas objectUI;

    [HideInInspector]
    public string UUID;
    [HideInInspector]
    public Transform objTransform;

    private BoxCollider2D objCollider;

    Transform camTransform;

    bool held = false;
    bool selected = false;

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
        if(held)
        {
            Vector3 newpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
                Input.mousePosition.y, (camTransform.position.z * -1f)));

            objTransform.position = newpos;
        }
    }

    void OnMouseDown()
    {
        held = true;

        if (selected)
        {
            selected = false;
            //WarehouseManager.instance.SetWarehouseObjectToUpdate(null);
            WarehouseManager.instance.btn_GoToDefaultCanvas();
        }
        else
        {
            selected = true;
            WarehouseManager.instance.SetWarehouseObjectToUpdate(this);
        }
    }

    void OnMouseUp()
    {
        held = false;
    }

    public void ToggleSelected(bool inBool)
    {
        selected = inBool;
    }
}
