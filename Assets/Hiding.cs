using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    CharacterController controller = null;

    Renderer[] renderers = null; // cache

    public bool hidden { get; private set; }
    public bool detected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void EnterCover()
    {
        if(!hidden)
        {
            hidden = true;
            if(!detected)
                SetVisibility(false);
        }
        
    }

    public void ExitCover()
    {
        Collider[] cover = Physics.OverlapSphere(transform.position,
                                                 controller.radius / 4, // seems to be detecting the exited cover in some cases?
                                                 1 << 11);
//        Debug.Log("Covers:" + cover.Length);
        if(cover == null || cover.Length == 0) // actually left cover
        {
            if (hidden)
            {
                hidden = false;
                SetVisibility(true);
            }
        }
    }

    public void EnterDetectionRange()
    {
        detected = true;
        if(hidden)
            SetVisibility(true);
    }

    public void ExitDetectionRange()
    {
        detected = false;
        if (hidden)
            SetVisibility(false);
    }

    private void SetVisibility(bool visible)
    {
        if (renderers == null)
            renderers = GetComponentsInChildren<Renderer>();
        Debug.Log(renderers.Length);
        foreach (Renderer r in renderers)
            r.enabled = visible;
    }
}
