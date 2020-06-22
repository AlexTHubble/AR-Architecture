using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCanvasTool : MonoBehaviour
{
    public static AllCanvasTool instance = null;

    [SerializeField]
    bool disableAllOnStart = false;

    [SerializeField]
    List<Canvas> canvasList = new List<Canvas>();

    List<Canvas> enabledCanvas = new List<Canvas>();

    Dictionary<string, Canvas> quickFindCanvas = new Dictionary<string, Canvas>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (disableAllOnStart)
        {
            DisableAllCanvas();
        }
        else
        {
            foreach(Canvas canvas in canvasList)
            {
                if (canvas.enabled)
                    enabledCanvas.Add(canvas);

                quickFindCanvas.Add(canvas.name, canvas);
            }
        }
    }

    public void DisableAllCanvas()
    {
        foreach(Canvas canvas in enabledCanvas)
        {
            canvas.enabled = false;
        }

        //Clear the canvas
        enabledCanvas.Clear();
    }

    public void DisableCanvas(string CanvasToDisable)
    {
        Canvas canvas = quickFindCanvas[CanvasToDisable];
        if(canvas.enabled)
        {
            enabledCanvas.Remove(canvas);
            canvas.enabled = false;
        }
    }

    public void EnableCanvas(string canvasTOEnable, bool disableRest)
    {
        if (disableRest)
            DisableAllCanvas();

        Canvas canvas = quickFindCanvas[canvasTOEnable];
        canvas.enabled = true;
        enabledCanvas.Add(canvas);
    }
}
