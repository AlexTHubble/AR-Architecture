using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCubeColor : MonoBehaviour
{
    [SerializeField]
    Transform cube;

    [SerializeField]
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateModel()
    {
        cube.transform.rotation = Quaternion.Euler(transform.rotation.x, slider.value, transform.rotation.z);
    }
}
