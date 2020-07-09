using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseObjectEdgeDetector : MonoBehaviour
{
    SpriteRenderer spRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public bool TestIsVisiable()
    {
        //Returns true if the sprite rendere has been disabled (more of a safty precaution)
        if (!spRenderer.enabled)
            return false;

        return spRenderer.isVisible;
    }

    public void DisableSpriteRenderer()
    {
        spRenderer.enabled = false;
    }
}
