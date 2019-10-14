using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    CharacterController controller = null;

    [SerializeField]
    GameObject characterVisual = null;

    Renderer[] renderers = null; // cache

    bool hidden = false;

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
            if(renderers == null)
                renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                r.enabled = false;
        }
        
    }

    public void ExitCover()
    {
        Collider[] cover = Physics.OverlapSphere(transform.position,
                                                 controller.radius,
                                                 1 << 11);
//        Debug.Log("Covers:" + cover.Length);
        if(cover == null || cover.Length == 0) // actually left cover
        {
            if (hidden)
            {
                hidden = false;
                if(renderers == null)
                renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                    r.enabled = true;
            }
        }
    }

    public void EnterDetectionRange()
    {

    }

    public void ExitDetectionRange()
    {

    }
}
